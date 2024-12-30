#ifndef API_HANDLER_H
#define API_HANDLER_H
#include <cstdint>

namespace api_handler {

    class status_message;
    
    bool init();

    void main_loop();
    uint8_t get_height();
    char *get_name();
    status_message get_latest_status();
    void set_height(uint8_t height);

    void watch_table_as_it_moves();
}

#endif