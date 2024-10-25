#include "rotary_encoder.h"
#include <Arduino.h>

namespace rotary_encoder {
    input::input(int a, int b, int c) 
    {
        ROT_A = a;
        ROT_B = b;
        ROT_C = c;

        rotary_encoder_position = 0;
        ROT_C_LAST_STATE = 1;
        last_button_push_time = 0;
    }

    void input::read_rotary_encoder() 
    {
        if(ROT_C_LAST_STATE) 
        {
            if(digitalRead(ROT_C) == 0) 
            {
                Serial.println("Button Pressed");
                last_button_push_time = millis();
                ROT_C_LAST_STATE = 0;
                return;
            }
        } else 
        {
            if(digitalRead(ROT_C) == 1 && millis() - last_button_push_time > 150) ROT_C_LAST_STATE = 1;
        }

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
}