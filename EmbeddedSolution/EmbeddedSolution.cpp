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
uint32_t last_input = 0;
bool input_was_reset = false;
bool display_on = true;


// Method Definitions
void check_input();
void reset_display_after_inactivity();
void enable_display_on_activity();
void on_rotary_encoder_change(uint gpio, uint32_t events);
void register_input();


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
        // Run Main Loop logic for rotary encoder driver
        rotary_encoder_input.main_loop();

        // Input logic
        check_input();

        // Reset display after 5 seconds of inactivity
        reset_display_after_inactivity();
    }
}


// Hardware interrupt Method
void on_rotary_encoder_change(uint gpio, uint32_t events) {
    if(rotary_encoder_input.check_button()) {
        BUTTON_PRESSED = true;
        return;
    }
    rotary_encoder_input.read_rotary_encoder();
}

// Input Handling Logic
void check_input() 
{
    if(BUTTON_PRESSED)
    {
        printf("Button pressed\n");
        enable_display_on_activity();
        COMPARE_HEIGHT = HEIGHT;
        display_height();
        BUTTON_PRESSED = false;
        register_input();
    }
    else
    {
        switch (rotary_encoder_input.get_state()) {
            case rotary_encoder::STATES::RIGHT_TURN:
                enable_display_on_activity();
                if(HEIGHT < 132) HEIGHT++;
                rotary_encoder_input.reset_state();
                register_input();
                break;
            case rotary_encoder::STATES::LEFT_TURN:
                enable_display_on_activity();
                if(HEIGHT > 68) HEIGHT--;
                rotary_encoder_input.reset_state();
                register_input();
                break;
            default:
                break;
        }

        if(COMPARE_HEIGHT != HEIGHT)
        {
            display_height();
            printf("Height: %dcm\n", HEIGHT);
            last_input = to_ms_since_boot(get_absolute_time());
            input_was_reset = false;
            COMPARE_HEIGHT = HEIGHT;
        }
    }
}

// Display Reset Logicv after inactivity
void reset_display_after_inactivity()
{
    // After 5 seconds, reset height if changed but not committed
    if(!input_was_reset && to_ms_since_boot(get_absolute_time()) - last_input > 5000)
    {
        input_was_reset = true;
        HEIGHT = COMPARE_HEIGHT;
        display_height();
    }

    // After 10 seconds, turn off display
    if(to_ms_since_boot(get_absolute_time()) - last_input > 10000 && display_on)
    {
        ssd1306_poweroff(&display);
        display_on = false;
    }
}

void enable_display_on_activity()
{
    if(!display_on)
    {
        ssd1306_poweron(&display);
        display_on = true;
    }
}

// Register Input
void register_input()
{
    last_input = to_ms_since_boot(get_absolute_time());
    input_was_reset = false;
}
