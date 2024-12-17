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
int last_rot_output = 0;

void on_rotary_encoder_change(uint gpio, uint32_t events) {
    last_rot_output = rotary_encoder_input.read_rotary_encoder();
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

        switch (last_rot_output) {
            case 1:
                if(height < 132) height++;
                oled._printf(1, false, "");
                last_rot_output = 0;
                break;
            case 2:
                if(height > 68) height--;
                oled._printf(1, false, "");
                last_rot_output = 0;
                break;
            case 3:
                oled._printf(1, true, "Button pressed");
                last_rot_output = 0;
                break;
            default:
                break;
        }
        oled._printf(0, true, "Height: %dcm", height);
    }
}
