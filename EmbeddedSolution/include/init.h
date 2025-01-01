#ifndef INIT_H
#define INIT_H

#include <stdbool.h>

bool tc_init
(
    rotary_encoder::input *rotary_encoder_input,
    int ROT_A,
    int ROT_B,
    int ROT_C
);

#endif 