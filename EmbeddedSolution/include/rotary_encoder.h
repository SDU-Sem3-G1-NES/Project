#ifndef ROTARY_ENCODER_H
#define ROTARY_ENCODER_H

#include <stdint.h>

namespace rotary_encoder {

    enum class STATES {
        RIGHT_TURN,
        LEFT_TURN,
        NONE
    };

    class input {
        public:
            input(int a, int b, int c);
            void read_rotary_encoder();
            void main_loop();
            bool check_button();
            STATES get_state();
            void reset_state();

        private:
            volatile int ROT_A;
            volatile int ROT_B;
            volatile int ROT_C;
            STATES state;

            volatile bool rotary_encoder_states[2];
            volatile int rotary_encoder_position;
            volatile bool ROT_C_LAST_STATE;

            uint32_t last_button_push_time;
    };
}

#endif