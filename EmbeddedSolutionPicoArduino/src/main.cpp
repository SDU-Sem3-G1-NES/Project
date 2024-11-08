#include <Arduino.h>
#include <SPI.h>
#include <Adafruit_GFX.h>
#include <Adafruit_ST7789.h>
#include "rotary_encoder.h"



// Rotary Encoder Definitions
#define ROT_A 22
#define ROT_B 21
#define ROT_C 20
rotary_encoder::input rotary_encoder_input(ROT_A, ROT_B, ROT_C);

void pot_interrupt() {
  rotary_encoder_input.read_rotary_encoder();
}

#define TFT_DC 14
#define TFT_RST 12
#define TFT_CS 13
#define TFT_MOSI 11
#define TFT_LED 15
#define TFT_SCLK 10

float p = 3.1415926;
void tft_test();
Adafruit_ST7789 tft = Adafruit_ST7789(TFT_CS, TFT_DC, TFT_MOSI, TFT_SCLK, TFT_RST);

// Setup1 is used to use the second CPU core
void setup() {
  // Debug Probe
  Serial1.setRX(1);
  Serial1.setTX(0);
  Serial1.begin(250000);

  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  pinMode(ROT_C, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(ROT_A), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_B), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_C), pot_interrupt, CHANGE);

  // Display
  pinMode(TFT_LED, OUTPUT);
  digitalWrite(TFT_LED, HIGH);

  tft.init(240, 320);
  tft.invertDisplay(false);
  tft.fillScreen(ST77XX_RED);
  delay(500);
  tft.fillScreen(ST77XX_GREEN);
  delay(500);
  tft.fillScreen(ST77XX_BLUE);
}

void loop() {
  Serial1.println("test");
  delay(2500);
  //rotary_encoder_input.read_rotary_encoder();
}

