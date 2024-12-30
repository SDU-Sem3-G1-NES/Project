#ifndef SHARED_H
#define SHARED_H

#include "api_handler.h"
#include "ssd1306.h"

extern ssd1306_t display;
extern int HEIGHT;
extern int COMPARE_HEIGHT;
extern bool BUTTON_PRESSED;
extern char *API_URL;
#endif