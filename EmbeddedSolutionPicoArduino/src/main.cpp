#include<Arduino.h>
#include "rotary_encoder.h"

#define ROT_A 9
#define ROT_B 10
#define ROT_C 11
rotary_encoder::input rotary_encoder_input(ROT_A, ROT_B, ROT_C);

void pot_interrupt() {
  rotary_encoder_input.read_rotary_encoder();
}

void setup() {

}

void loop() {

}


// Setup1 is used to use the second CPU core
void setup1() {
  // Debug Probe
  Serial1.setRX(1);
  Serial1.setTX(0);
  Serial1.begin(115200);



  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  pinMode(ROT_C, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(ROT_A), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_B), pot_interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(ROT_C), pot_interrupt, CHANGE);

  Serial.begin(115200);
}

void loop1() {
  //delay(2500);
  //rotary_encoder_input.read_rotary_encoder();
}