#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include "pico/stdlib.h"
#include "hardware/clocks.h"
#include "pico/cyw43_arch.h"
#include "hardware/gpio.h"
#include "hardware/irq.h"
#include "rotary_encoder.h"
#include "ssd1306.h"
#include "font.h"
#include "init.h"
#include "UI.h"
#include "shared.h"

#define ROT_A 13
#define ROT_B 14
#define ROT_C 15
rotary_encoder::input rotary_encoder_input(ROT_A, ROT_B, ROT_C);

void on_rotary_encoder_change(uint gpio, uint32_t events) {
    if(rotary_encoder_input.check_button()) {
        BUTTON_PRESSED = true;
        return;
    }
    rotary_encoder_input.read_rotary_encoder();
}

void init() {
    tc_init(&rotary_encoder_input, ROT_A, ROT_B, ROT_C);

    gpio_set_irq_enabled_with_callback(ROT_A, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_B, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_C, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);

    char buffer[20];
    snprintf(buffer, sizeof(buffer), "Height: %dcm", HEIGHT);
    ssd1306_clear(&display);
    ssd1306_draw_string_with_font(&display, 0, 0, 1, font_8x5, buffer);
    ssd1306_show(&display);
    printf("Height: %dcm\n", HEIGHT);

    ui_init();
}

int main()
{
    init();

    while (true) {
        rotary_encoder_input.main_loop();

        if(BUTTON_PRESSED)
        {
            printf("Button pressed\n");
            COMPARE_HEIGHT = HEIGHT;
            display_height();
            BUTTON_PRESSED = false;
        }
        else
        {
            switch (rotary_encoder_input.get_state()) {
                case rotary_encoder::STATES::RIGHT_TURN:
                    if(HEIGHT < 132) HEIGHT++;
                    rotary_encoder_input.reset_state();
                    break;
                case rotary_encoder::STATES::LEFT_TURN:
                    if(HEIGHT > 68) HEIGHT--;
                    rotary_encoder_input.reset_state();
                    break;
                default:
                    break;
            }

            if(COMPARE_HEIGHT != HEIGHT)
            {
                display_height();
                printf("Height: %dcm\n", HEIGHT);
                COMPARE_HEIGHT = HEIGHT;
            }
        }
    }
}
