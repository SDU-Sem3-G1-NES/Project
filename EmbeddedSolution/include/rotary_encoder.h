#ifndef ROTARY_ENCODER_H
#define ROTARY_ENCODER_H

#include <stdint.h>

namespace rotary_encoder {

    class input {
        public:
            input(int a, int b, int c);
            void read_rotary_encoder();
            void main_loop();

        private:
            void check_button();
            volatile int ROT_A;
            volatile int ROT_B;
            volatile int ROT_C;

            volatile bool rotary_encoder_states[2];
            volatile int rotary_encoder_position;
            volatile bool ROT_C_LAST_STATE;

            uint32_t last_button_push_time;
    };
}

#endif