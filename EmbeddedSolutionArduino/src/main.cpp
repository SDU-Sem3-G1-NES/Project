#include<Arduino.h>

// Define rotary encoder pins
#define ROT_A 2 // Left Turn
#define ROT_B 3 // RIght Turn
#define ROT_C 4 // Push Button

unsigned long _lastIncReadTime = micros(); 
unsigned long _lastDecReadTime = micros(); 
int _pauseLength = 25000;
int _fastIncrement = 10;

void read_rotary_encoder(); // Function declaration

void setup() {

  // Set encoder pins and attach interrupts
  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  pinMode(ROT_C, INPUT_PULLUP);
  //attachInterrupt(digitalPinToInterrupt(ROT_A), read_rotary_encoder, CHANGE);
  //attachInterrupt(digitalPinToInterrupt(ROT_B), read_rotary_encoder, CHANGE);

  // Start the serial monitor to show output
  Serial.begin(115200);
}

void loop() {
  delay(1000);
  read_rotary_encoder();
}

void read_rotary_encoder() {
  //static uint8_t old_AB = 3;  // Lookup table index
  //static int8_t encval = 0;   // Encoder value  
  //static const int8_t enc_states[]  = {0,-1,1,0,1,0,0,-1,-1,0,0,1,0,1,-1,0}; // Lookup table

  //Serial.println("\nRotary Encoder Interrupt Triggered");
  Serial.print("ROT_A: " + String(digitalRead(ROT_A)) + "\n");
  Serial.print("ROT_B: " + String(digitalRead(ROT_B)) + "\n");
  Serial.print("ROT_C: " + String(digitalRead(ROT_C)) + "\n");
} 