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

Adafruit_ST7789 tft = Adafruit_ST7789(&SPI, TFT_CS, TFT_DC, TFT_RST);

// http://blog.dzl.dk/2020/04/04/crazy-fast-tft-action-with-adafruit-gfx-and-esp32/
class  aFrameBuffer : public Adafruit_GFX {
  public:
    uint16_t *buffer;
    aFrameBuffer(int16_t w, int16_t h): Adafruit_GFX(w, h)
    {
      buffer = (uint16_t*)malloc(2 * h * w);
      for (int i = 0; i < h * w; i++)
        buffer[i] = 0;

      tft.init(w, h);
      tft.setRotation(1);
      tft.invertDisplay(false);
      tft.fillScreen(ST77XX_BLACK);
    }
    void drawPixel( int16_t x, int16_t y, uint16_t color)
    {
      if (x > 159)
        return;
      if (x < 0)
        return;
      if (y > 127)
        return;
      if (y < 0)
        return;
      buffer[x + y * _width] = color;
    }

    void display()
    {
      tft.setAddrWindow(0, 0, 160, 128);
      digitalWrite(TFT_DC, HIGH);
      digitalWrite(TFT_CS, LOW);
      SPI.beginTransaction(SPISettings(80000000, MSBFIRST, SPI_MODE0));
      for (uint16_t i = 0; i < 160 * 128; i++)
      {
        SPI.transfer16(buffer[i]);
      }
      SPI.endTransaction();
      digitalWrite(TFT_CS, HIGH);
    }
};


aFrameBuffer frame(240, 320);

// Setup1 is used to use the second CPU core
void setup() {
  // Debug Probe
  Serial.begin(250000);
  Serial.println("Setup Starting");

  SPI.setCS(TFT_CS);
  SPI.setSCK(TFT_SCLK);
  SPI.setTX(TFT_MOSI);

  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  pinMode(ROT_C, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(ROT_A), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_B), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_C), pot_interrupt, CHANGE);

  // Display
  pinMode(TFT_LED, OUTPUT);
  digitalWrite(TFT_LED, HIGH);

  frame.display();

  Serial.println("Setup Complete");

}

void loop() {
  Serial.println("test");
  delay(2500);
  //rotary_encoder_input.read_rotary_encoder();
}

