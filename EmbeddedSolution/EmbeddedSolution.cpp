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

// Bitmaps
#include "bitmaps/LinakLogo.h"

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
    }
    rotary_encoder_input.read_rotary_encoder();
}

void init() {
    tc_init(&rotary_encoder_input, ROT_A, ROT_B, ROT_C);

    gpio_set_irq_enabled_with_callback(ROT_A, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_B, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);
    gpio_set_irq_enabled_with_callback(ROT_C, GPIO_IRQ_EDGE_RISE | GPIO_IRQ_EDGE_FALL, true, &on_rotary_encoder_change);

    char buffer[20];
    snprintf(buffer, sizeof(buffer), "Height: %dcm", height);
    ssd1306_clear(&display);
    ssd1306_draw_string_with_font(&display, 0, 0, 1, font_8x5, buffer);
    ssd1306_show(&display);
    printf("Height: %dcm\n", height);
}

int main()
{
    init();

    while (true) {
        rotary_encoder_input.main_loop();

        if(button_pressed)
        {
            ssd1306_clear(&display);
            ssd1306_draw_string_with_font(&display, 0, 0, 1, font_8x5, "Button pressed");
            ssd1306_show(&display);
            printf("Button pressed\n");
            button_pressed = false;
        }
        else
        {
            switch (rotary_encoder_input.get_state()) {
                case rotary_encoder::STATES::RIGHT_TURN:
                    if(height < 132) height++;
                    rotary_encoder_input.reset_state();
                    break;
                case rotary_encoder::STATES::LEFT_TURN:
                    if(height > 68) height--;
                    rotary_encoder_input.reset_state();
                    break;
                default:
                    break;
            }

            if(compare_height != height)
            {
                char buffer[20];
                ssd1306_clear(&display);
                snprintf(buffer, sizeof(buffer), "Height: %dcm", height);
                ssd1306_draw_string_with_font(&display, 0, 0, 1, font_8x5, buffer);
                ssd1306_show(&display);
                printf("Height: %dcm\n", height);
                compare_height = height;
            }
        }
    }
}
