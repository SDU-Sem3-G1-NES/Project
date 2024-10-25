#include<Arduino.h>

// Define rotary encoder pins
#define ROT_A 11 // Left Turn
#define ROT_B 12 // RIght Turn
#define ROT_C 13 // Push Button

volatile bool rotary_encoder_states[2] = {0, 0};
volatile int rotary_encoder_position = 0;

void read_rotary_encoder(); // Function declaration

void setup() {

  // Set encoder pins and attach interrupts
  pinMode(ROT_A, INPUT_PULLUP);
  pinMode(ROT_B, INPUT_PULLUP);
  PCICR |= B00000001; // Enable Port B interrupt
  PCMSK0 |= B00111000; // Enable interrupt on pins 11, 12, 13

  // Start the serial monitor to show output
  Serial.begin(115200);
}

void loop() {
  //delay(2500);
  //read_rotary_encoder();
}

ISR(PCINT0_vect) {
  read_rotary_encoder();
}

void read_rotary_encoder() {
  //Serial.println(String(digitalRead(ROT_A)) + " " + String(digitalRead(ROT_B)));

  bool ROT_A_STATE = digitalRead(ROT_A);
  bool ROT_B_STATE = digitalRead(ROT_B);
  switch(rotary_encoder_position) 
  {
    case 0:
      // Beginning of rotation measurement
      if(!ROT_A_STATE && !ROT_B_STATE) rotary_encoder_position = 1; 
      break;
      
    case 1:
      if(!ROT_A_STATE && !ROT_B_STATE) 
      {
        break;
      }
      else if(ROT_A_STATE != ROT_B_STATE) {
        rotary_encoder_states[0] = ROT_A_STATE;
        rotary_encoder_states[1] = ROT_B_STATE;
        rotary_encoder_position = 2;
      }
      break;
    
    case 2:
      if(!ROT_A_STATE && !ROT_B_STATE) 
        {
          rotary_encoder_position = 1;
          break;
        }
      else if(ROT_A_STATE && ROT_B_STATE) {
        rotary_encoder_position = 3; // End of rotation measurement
        if(rotary_encoder_states[0] == 0 && rotary_encoder_states[1] == 1) {
          Serial.println("Right Turn");
        } else if(rotary_encoder_states[0] == 1 && rotary_encoder_states[1] == 0) {
          Serial.println("Left Turn");
        }

        //delay(100); // Some mild debouncing
        rotary_encoder_states[0] = 0;
        rotary_encoder_states[1] = 0;
        rotary_encoder_position = 0;
      }
      break;
    
    default:
      break;
  }
} 