#include "api_handler.h"
#include "shared.h"
#include "pico/async_context.h"
#include "lwip/apps/http_client.h"
#include "lwip/altcp.h"
#include "lwip/altcp_tcp.h"
#include "lwip/altcp_tls.h"
#include "lwip/err.h"
#include "lwip/sockets.h"
#include "lwip/inet.h"
#include "lwip/sys.h"
#include "lwip/netdb.h"
#include "lwip/dns.h"
#include <cstring>
#include <cstdio>
#include "example_http_client_util.h"
#include "pico/cyw43_arch.h"
#include "wifi.h"

namespace api_handler {

    class status_message {
        public:
            uint8_t error_code;
            char *error_message;

            status_message(uint8_t error_code, char *error_message) {
                this->error_code = error_code;
                this->error_message = error_message;
            }
    };

    err_t set_header_get(struct _httpc_state *connection, void *arg, struct pbuf *hdr, u16_t hdr_len, u32_t content_len) 
    {
        // Set the headers
        // Add header manually since httpc_add_header is not available
        printf("Setting header\n");
        char header[256];
        snprintf(header, sizeof(header), "ApiKey: %s\r\n", API_KEY); // Ensure proper header format with \r\n

        // Check if the header fits into the provided pbuf
        if (strlen(header) > hdr_len) {
            printf("Header length exceeds buffer length\n");
            return ERR_BUF;
        }

        // Add the header to the pbuf
        pbuf_take(hdr, header, strlen(header));

        return ERR_OK;
        }

    void table_info_received(void *arg, httpc_result_t httpc_result, u32_t rx_content_len, u32_t srv_res, err_t err)
    {
        if (err != ERR_OK) {
            printf("Error received: %d\n", err);
            return;
        }

        // Process the received data
        printf("Received data: %lu bytes\n", rx_content_len);

        // Handle the result
        if (httpc_result == HTTPC_RESULT_OK) {
            printf("HTTP request successful\n");
        } else {
            printf("HTTP request failed with result: %d\n", httpc_result);
        }
    }

    bool init()
    {
        lwip_init();
        char host[256];
        sprintf(host, "%s", ASP_URL);

        char request_url[256];
        sprintf(request_url, "/api/Table/%s", TABLE_GUID);

        printf("Creating request to %s%s\n", host, request_url);
        EXAMPLE_HTTP_REQUEST_T req1 = {0};
        req1.hostname = host;
        //req1.tls_config = altcp_tls_create_config_client(NULL, 0);
        req1.port = ASP_PORT;
        req1.url = request_url;
        req1.headers_fn = set_header_get;
        req1.result_fn = table_info_received;

        printf("Sending request\n");
        int result = 0;
        result += http_client_request_sync(cyw43_arch_async_context(), &req1);

        while(!req1.complete) {
            printf("Waiting for request to complete\n");
            async_context_poll(cyw43_arch_async_context());
            async_context_wait_for_work_ms(cyw43_arch_async_context(), 1000);
        }

        printf("Result: %d\n", result);
        bool success = (result == 200) ? true : false;
        return success;
    }

    void main_loop();
    uint8_t get_height();
    char *get_name();
    status_message get_latest_status();
    void set_height(uint8_t height);

    void watch_table_as_it_moves();
}