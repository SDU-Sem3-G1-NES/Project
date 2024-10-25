#include "rotary_encoder.h"

#include <stdio.h>
#include "pico/stdlib.h"
#include "hardware/clocks.h"
#include "pico/cyw43_arch.h"
#include "pico/time.h"
#include "hardware/gpio.h"

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
        check_button();

        bool ROT_A_STATE = gpio_get(ROT_A);
        bool ROT_B_STATE = gpio_get(ROT_B);
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
                printf("Right Turn\n");
                } else if(rotary_encoder_states[0] == 1 && rotary_encoder_states[1] == 0) {
                printf("Left Turn\n");
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

    void input::check_button() {
        if(ROT_C_LAST_STATE) 
        {
            if(gpio_get(ROT_C) == 0) 
            {
                last_button_push_time = to_ms_since_boot(get_absolute_time());
                ROT_C_LAST_STATE = 0;
                printf("Button Pressed\n");
                return;
            }
        } 
    }

    void input::main_loop() {
        if(to_ms_since_boot(get_absolute_time()) - last_button_push_time > 200) ROT_C_LAST_STATE = 1;
    }
}