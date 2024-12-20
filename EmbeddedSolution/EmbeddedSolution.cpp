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
#include "wifi.h"
#include "font.h"

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

bool initialise_wifi() {
    cyw43_arch_enable_sta_mode();
    if (cyw43_arch_wifi_connect_timeout_ms(WIFI_SSID, WIFI_PASS, CYW43_AUTH_WPA2_AES_PSK, 10000)) {
        printf("Wi-Fi connect failed\n");
        return false;
    }
    return true;
}

int main()
{
    stdio_init_all();

    i2c_init(i2c_default, 400000);
    gpio_set_function(PICO_DEFAULT_I2C_SDA_PIN, GPIO_FUNC_I2C);
    gpio_set_function(PICO_DEFAULT_I2C_SCL_PIN, GPIO_FUNC_I2C);
    gpio_pull_up(PICO_DEFAULT_I2C_SDA_PIN);
    gpio_pull_up(PICO_DEFAULT_I2C_SCL_PIN);


    ssd1306_t display;
    display.external_vcc = false;
    ssd1306_init(&display, 128, 64, 0x3C, i2c_default);
    ssd1306_clear(&display);
    ssd1306_bmp_show_image(&display, LinakLogo_bmp, sizeof(LinakLogo_bmp));
    ssd1306_show(&display);

    if (cyw43_arch_init()) {
        printf("Wi-Fi init failed\n");
        return false;
    }

    // Initialise the Wi-Fi chip
    while(!initialise_wifi()) {
        sleep_ms(1000);
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


    //display oled;
    //oled.clear(true);
    //oled._printf(0, true, "Height: %dcm", height);
    char buffer[20];
    snprintf(buffer, sizeof(buffer), "Height: %dcm", height);
    ssd1306_clear(&display);
    ssd1306_draw_string_with_font(&display, 0, 0, 1, font_8x5, buffer);
    ssd1306_show(&display);
    printf("Height: %dcm\n", height);

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
