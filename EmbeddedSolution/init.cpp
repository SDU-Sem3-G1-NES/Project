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
#include "bitmaps/NoWifi.h"

int attempt = 1;

ssd1306_t display;

void print_status_message(const char *message) 
{
    int x_offset = (128 - strlen(message) * 6) / 2;

    ssd1306_clear(&display);
    ssd1306_bmp_show_image(&display, LinakLogo_bmp, LinakLogo_bmp_size);
    ssd1306_draw_string_with_font(&display, x_offset, 52, 1, font_8x5, message);
    ssd1306_show(&display);
}

bool initialise_wifi() {
    while (attempt <= MAX_ATTEMPTS) {
        char status_message[50];
        snprintf(status_message, sizeof(status_message), "Connect WiFi(%d/%d)", attempt, MAX_ATTEMPTS);
        print_status_message(status_message);

        cyw43_arch_enable_sta_mode();
        if (cyw43_arch_wifi_connect_timeout_ms(WIFI_SSID, WIFI_PASS, CYW43_AUTH_WPA2_AES_PSK, 5000)) {
            printf("Wi-Fi connect failed\n");
            attempt++;
            sleep_ms(1000);
            continue;
        }
        return true;
    }
    return false;
}

void wifi_failed() 
{
    ssd1306_clear(&display);
    ssd1306_bmp_show_image_with_offset(&display, NoWifi_bmp, NoWifi_bmp_size, 48, 10);
    ssd1306_draw_string_with_font(&display, 16, 42, 1, font_8x5, "Wifi init failed");
    ssd1306_draw_string_with_font(&display, 28, 52, 1, font_8x5, "Please reset");
    ssd1306_show(&display);

    while (true) {
        sleep_ms(1000);
    }
}

bool tc_init
(
    rotary_encoder::input *rotary_encoder_input,
    int ROT_A,
    int ROT_B,
    int ROT_C
) {
    stdio_init_all();

    i2c_init(i2c_default, 800000);
    gpio_set_function(PICO_DEFAULT_I2C_SDA_PIN, GPIO_FUNC_I2C);
    gpio_set_function(PICO_DEFAULT_I2C_SCL_PIN, GPIO_FUNC_I2C);
    gpio_pull_up(PICO_DEFAULT_I2C_SDA_PIN);
    gpio_pull_up(PICO_DEFAULT_I2C_SCL_PIN);

    display.external_vcc = false;
    ssd1306_init(&display, 128, 64, 0x3C, i2c_default);
    print_status_message("");

    if (cyw43_arch_init()) {
        printf("Wi-Fi init failed\n");
        return false;
    }

    // Initialise the Wi-Fi chip
    if(!initialise_wifi()) 
    {
        wifi_failed();
    }

    // Initialise pins for the rotary encoder
    rotary_encoder_input->reset_state();
    gpio_init(ROT_A);
    gpio_set_dir(ROT_A, GPIO_IN);
    gpio_pull_up(ROT_A);

    gpio_init(ROT_B);
    gpio_set_dir(ROT_B, GPIO_IN);
    gpio_pull_up(ROT_B);

    gpio_init(ROT_C);
    gpio_set_dir(ROT_C, GPIO_IN);
    gpio_pull_up(ROT_C);

    return true;
}