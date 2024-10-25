#include<Arduino.h>
#include "rotary_encoder.h"

#define ROT_A 8
#define ROT_B 9
#define ROT_C 10
rotary_encoder::input rotary_encoder_input(ROT_A, ROT_B, ROT_C);

void setup() {
  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  pinMode(ROT_C, INPUT_PULLUP);
  PCICR |= B00000001;
  PCMSK0 |= B00000111;

  Serial.begin(115200);
}

void loop() {
  //delay(2500);
  //read_rotary_encoder();
}

ISR(PCINT0_vect) {
  rotary_encoder_input.read_rotary_encoder();
}