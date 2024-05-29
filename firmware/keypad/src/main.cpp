#include <Arduino.h>
#include <WS2812FX.h>
#include <OneButton.h>

#define LED_COUNT 3
#define LED_PIN D4
#define BUTTON_PIN_1 D3
#define BUTTON_PIN_2 D2
#define BUTTON_PIN_3 D1

#define SHORT_PRESS_DURATION 200
#define LONG_PRESS_DURATION 1000
#define FIRMWARE_VERSION "keypad3b 1.0.0"


// Method declarations
OneButton setupButton(byte pin);
String getButtonIdentifier(OneButton& btn);
void handleSinglePress(void *param);
void handleDoublePress(void *param);
void handleLongPress(void *param);
void serialLoop();
void parseCommand(String command);
void sendVersion();
void sendLedState(int ledIndex);
void setLedState(int ledIndex, uint8_t mode, byte r, byte g, byte b, uint16_t speed);

String getValue(String data, char separator, int index);


// LED
WS2812FX ws2812fx = WS2812FX(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);

// Buttons
OneButton button1 = setupButton(BUTTON_PIN_1);
OneButton button2 = setupButton(BUTTON_PIN_2);
OneButton button3 = setupButton(BUTTON_PIN_3);


void setup() {
  Serial.begin(9600);

  // LED
  ws2812fx.init();
  ws2812fx.setBrightness(255);
  ws2812fx.setSegment(0, 0, 0, FX_MODE_STATIC, (uint32_t)0x000000, 1000, false);
  ws2812fx.setSegment(1, 1, 1, FX_MODE_STATIC, (uint32_t)0x000000, 1000, false);
  ws2812fx.setSegment(2, 2, 2, FX_MODE_STATIC, (uint32_t)0x000000, 1000, false);

  ws2812fx.start();
}

void loop() {

  // Led
  ws2812fx.service();

  // Buttons
  button1.tick();
  button2.tick();
  button3.tick();

  // Read commands
  serialLoop();
}

// - Buttons

OneButton setupButton(byte pin) {
  OneButton button = OneButton(pin, true, true);
  button.attachClick(handleSinglePress, &button);
  button.attachDoubleClick(handleDoublePress, &button);
  button.attachLongPressStart(handleLongPress, &button);
  button.setPressMs(LONG_PRESS_DURATION);
  button.setClickMs(SHORT_PRESS_DURATION);
  return button;
}

void handleSinglePress(void *param) {
  OneButton button = *((OneButton*)param);
  Serial.print("sclick ");
  Serial.println(getButtonIdentifier(button));
}

void handleDoublePress(void *param) {
  OneButton button = *((OneButton*)param);
  Serial.print("dclick ");
  Serial.println(getButtonIdentifier(button));
}

void handleLongPress(void *param) {
  OneButton button = *((OneButton*)param);
  Serial.print("lclick ");
  Serial.println(getButtonIdentifier(button));
}

String getButtonIdentifier(OneButton& btn) {

  if (btn.pin() == button1.pin()) {
    return "1";
  }
  if (btn.pin() == button2.pin()) {
    return "2";
  }
  if (btn.pin() == button3.pin()) {
    return "3";
  }

  return "unknown";
}

// Serial
void serialLoop() {

  if(Serial.available())
  {
    String command = Serial.readStringUntil('\n');
    parseCommand(command);
  }
}

/*
  Command                     | Name              | Description
  ----------------------------------------------------------------
  v                           | version           | Returns firmware version
  getled N                    | Get led N state   | Get state for led witn number N. Return: led N mode r g b speed
  setled N mode r g b speed   | Set ledn N state  | Set state for led witn number N. Return: led N mode r g b speed
*/
void parseCommand(String input) {

  const char separator = ' ';
  String command = getValue(input, separator, 0);

  // Execute
  if(command == "v")
  { 
    sendVersion();
  }

  // Execute

  // getled N
  if(command == "getled")
  { 
    String argString = getValue(input, separator, 1);
    if (argString.isEmpty()) { return; }
    sendLedState(argString.toInt());
  }
  
  // setled N mode r g b speed
  if(command == "setled")
  { 
    int argNString = getValue(input, separator, 1).toInt();
    uint8_t argModeString = getValue(input, separator, 2).toInt();
    byte argRString = getValue(input, separator, 3).toInt();
    byte argGString = getValue(input, separator, 4).toInt();
    byte argBString = getValue(input, separator, 5).toInt();
    uint16_t argSpeedString = getValue(input, separator, 6).toInt();
    setLedState(argNString, argModeString, argRString, argGString, argBString, argSpeedString);
  }
}



void sendVersion() {
  Serial.println(FIRMWARE_VERSION);
}

void sendLedState(int ledIndex) {

  // to set
  // uint32_t c = (uint32_t)strtoul(scmd + 2, NULL, 16);

  uint8_t mode = ws2812fx.getMode(ledIndex);
  uint32_t color = ws2812fx.getColor(ledIndex);
  byte red = color >> 16;
  byte green = (color & 0x00ff00) >> 8;
  byte blue = (color & 0x0000ff);
  uint16_t speed = ws2812fx.getSpeed(ledIndex);

  String resp = String("led ") + ledIndex; // Index
  resp += (" " + String(mode)); // Mode
  resp += (" " + String(red) + " " + String(green) + " " + String(blue)); // Color
  resp += (" " + String(speed)); // Speed

  // led (N) (mode) (r) (g) (b) (speed)
  // led 1 1 255 255 255 255
  Serial.println(resp);
}

// setled N mode r g b speed
// setled 0 2 255 100 0 2000
void setLedState(int ledIndex, uint8_t mode, byte r, byte g, byte b, uint16_t speed) {

  ws2812fx.setMode(ledIndex, mode);
  uint32_t color = ((long)r << 16L) | ((long)g << 8L) | (long)b;
  ws2812fx.setColor(ledIndex, color);
  ws2812fx.setSpeed(ledIndex, speed);
  sendLedState(ledIndex);
}

// Utility

// Get value at index of string separated by character
// https://stackoverflow.com/a/14824108
String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length()-1;

  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }

  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}