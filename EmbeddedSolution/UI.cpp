#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <vector>
#include <algorithm>
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

#include "bitmaps/TurnLeft.h"
#include "bitmaps/TurnRight.h"
#include "bitmaps/normal_numbers.h"
#include "bitmaps/italic_numbers.h"

std::vector<int> split_into_digits(int number) {
    std::vector<int> digits;
    while (number > 0) {
        digits.push_back(number % 10);
        number /= 10;
    }
    std::reverse(digits.begin(), digits.end());
    return digits;
}

void display_height() 
{
    const unsigned char (*numbers_bitmap)[226] = (HEIGHT == COMPARE_HEIGHT) ? normal_numbers : italic_numbers;
    int x_offset = (HEIGHT > 99) ? 38 : 48;

    ssd1306_clear_square(&display, 38, 24, 50, 24);
    std::vector<int> digits = split_into_digits(HEIGHT);

    for (int digit : digits) {
        ssd1306_bmp_show_image_with_offset(&display, numbers_bitmap[digit], 226, x_offset, 24);
        x_offset += 17;
        printf("%d", digit);
    }
    ssd1306_show(&display);
}

void ui_init()
{
    ssd1306_clear(&display);
    ssd1306_bmp_show_image_with_offset(&display, TurnLeft_bmp, TurnLeft_bmp_size, 4, 20);
    ssd1306_bmp_show_image_with_offset(&display, TurnRight_bmp, TurnRight_bmp_size, 92, 20);
    ssd1306_draw_string_with_font(&display, 28, 0, 1, font_8x5, "Nick's Table");
    ssd1306_show(&display);
    display_height();
}