#include <stdio.h>
#include "pico/stdlib.h"
#include "hardware/clocks.h"
#include "pico/cyw43_arch.h"
#include "hardware/gpio.h"
#include "hardware/irq.h"
#include "rotary_encoder.h"
#include "display.h"


#define ROT_A 13
#define ROT_B 14
#define ROT_C 15
rotary_encoder::input rotary_encoder_input(ROT_A, ROT_B, ROT_C);

int height = 90;
int compare_height = 90;
bool button_pressed = false;

void on_rotary_encoder_change(uint gpio, uint32_t events) {
    if(rotary_encoder_input.check_button()) {
        button_pressed = true;
        return;
    };
    rotary_encoder_input.read_rotary_encoder();
}

int main()
{
    stdio_init_all();
    // Initialise the Wi-Fi chip
    if (cyw43_arch_init()) {
        printf("Wi-Fi init failed\n");
        return -1;
    }

    // Initialise pins for the rotary encoder
    rotary_encoder_input.reset_state();
    gpio_init(ROT_A);
    gpio_set_dir(ROT_A, GPIO_IN);
    gpio_pull_up(ROT_A);

    gpio_init(ROT_B);
    gpio_set_dir(ROT_B, GPIO_IN);
    gpio_pull_up(ROT_B);

    gpio_init(ROT_C);
    gpio_set_dir(ROT_C, GPIO_IN);
    gpio_pull_up(ROT_C);

    gpio_set_irq_enabled_with_callback(ROT_A, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_B, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_C, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);


    display oled;
    oled.clear(true);
    oled._printf(0, true, "Height: %dcm", height);
    printf("Height: %dcm\n", height);

    while (true) {
        rotary_encoder_input.main_loop();

        if(button_pressed)
        {
            oled._printf(0, true, "Button pressed");
            printf("Button pressed\n");
            button_pressed = false;
        }
        else
        {
            switch (rotary_encoder_input.get_state()) {
                case rotary_encoder::STATES::RIGHT_TURN:
                    if(height < 132) height++;
                    //oled._printf(1, false, "");
                    rotary_encoder_input.reset_state();
                    break;
                case rotary_encoder::STATES::LEFT_TURN:
                    if(height > 68) height--;
                    //oled._printf(1, false, "");
                    rotary_encoder_input.reset_state();
                    break;
                default:
                    break;
            }

            if(compare_height != height)
            {
                oled._printf(0, true, "Height: %dcm", height);
                printf("Height: %dcm\n", height);
                compare_height = height;
            }
        }
    }
}
