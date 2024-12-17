import argparse
import ssl
import random
import logging
from http.server import HTTPServer
from users import UserType
from desk_manager import DeskManager
from simple_rest_server import SimpleRESTServer
import signal
import sys

logging.basicConfig(level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s")

def generate_desk_id():
    return ":".join(f"{random.randint(0, 255):02x}" for _ in range(6))

def generate_desk_name():
    return f"DESK {random.randint(1000, 9999)}"

def run(server_class=HTTPServer, handler_class=SimpleRESTServer, port=8000, disable_simulation=False, use_https=False, cert_file=None, key_file=None, desks=2, speed=60):
    logging.info(f"Initializing DeskManager with simulation speed: {speed}")
    desk_manager = DeskManager(speed)
    
    logging.info("Adding default desks...")
    desk_manager.add_desk("cd:fb:1a:53:fb:e6", "HC Desk 1", "Linak A/S", UserType.DISABLED)
    desk_manager.add_desk("ee:62:5b:b8:73:1d", "HC Desk 2", "Linak A/S", UserType.DISABLED)
    
    if len(desk_manager.get_desk_ids()) < desks:
        logging.info(f"Adding {desks - len(desk_manager.get_desk_ids())} additional desks.")
        for i in range(desks - len(desk_manager.get_desk_ids())):
            desk_manager.add_desk(generate_desk_id(), generate_desk_name(), "Linak A/S", UserType.DISABLED if disable_simulation else UserType.ACTIVE)
    
    desk_manager.start_updates()

    def handler(*args, **kwargs):
        handler_class(desk_manager, *args, **kwargs)

    server_address = ("0.0.0.0", port)
    SimpleRESTServer.initialize_api_keys()
    httpd = server_class(server_address, handler)

    if use_https:
        if not cert_file or not key_file:
            logging.error("Both certificate and key files must be provided for HTTPS.")
            raise ValueError("Both certificate and key files must be provided for HTTPS.")
        context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
        context.load_cert_chain(certfile=cert_file, keyfile=key_file)
        
        httpd.socket = context.wrap_socket(httpd.socket, server_side=True)
        protocol = "HTTPS"
    else:
        protocol = "HTTP"

    logging.info(f"Starting {protocol} server on port {port}...")
    
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        logging.info("Shutting down server...")
    finally:
        desk_manager.stop_updates()  # Stop the desk updates
        logging.info("Server stopped.")
        
def signal_handler(sig, frame):
    logging.info("Received signal to stop the server.")
    raise KeyboardInterrupt

signal.signal(signal.SIGTERM, signal_handler)

"""
    To execute the script as HTTPS, use the following command:
        python main.py --port 8443 --https --certfile cert.pem --keyfile key.pem

    To execute the script as HTTP, use the following command:
        python main.py --port 8000
"""

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Start a simple REST API server.")
    parser.add_argument("--port", type=int, default=8000, help="Port to run the server on (default: 8000)")
    parser.add_argument("--https", action="store_true", help="Enable HTTPS")
    parser.add_argument("--certfile", type=str, help="Path to the SSL certificate file")
    parser.add_argument("--keyfile", type=str, help="Path to the SSL key file")
    parser.add_argument("--desks", type=int, default=2, help="Minimum number of desks to simulate (default: 2)")
    parser.add_argument("--speed", type=int, default=60, help="Simulation speed (default: 60)")
    parser.add_argument("--disablesim", action="store_true", default=False, help="Disable simulation (default: False)")

    args = parser.parse_args()

    logging.info("Starting server with the following configuration:")
    logging.info(f"Port: {args.port}")
    logging.info(f"HTTPS: {'Enabled' if args.https else 'Disabled'}")
    if args.https:
        logging.info(f"Certificate file: {args.certfile}")
        logging.info(f"Key file: {args.keyfile}")
    logging.info(f"Number of desks: {args.desks}")
    logging.info(f"Simulation speed: {args.speed}")

    run(
        port=args.port,
        use_https=args.https,
        cert_file=args.certfile,
        key_file=args.keyfile,
        desks=args.desks,
        speed=args.speed,
        disable_simulation=args.disablesim
    )
