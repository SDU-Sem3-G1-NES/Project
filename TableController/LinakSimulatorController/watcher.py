import sys
import time
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
import subprocess
from colorama import init, Fore, Style
import threading

# Initialize colorama
init(autoreset=True)

class Watcher:
    DIRECTORY_TO_WATCH = "/app"

    def __init__(self):
        self.observer = Observer()
        self.process = None

    def run(self):
        event_handler = Handler(self)
        self.observer.schedule(event_handler, self.DIRECTORY_TO_WATCH, recursive=True)
        self.observer.start()
        try:
            while True:
                time.sleep(5)
        except KeyboardInterrupt:
            self.observer.stop()
        self.observer.join()

    def start_process(self):
        if self.process:
            print(Fore.YELLOW + "Warning: Restarting the application due to file change.")
            self.process.terminate()
            self.process.wait()
        try:
            self.process = subprocess.Popen(
                [sys.executable, "api.py"],
                stdout=subprocess.PIPE,
                stderr=subprocess.PIPE,
                text=True
            )
            print(Fore.GREEN + "Application started successfully.")
            threading.Thread(target=self._print_output, args=(self.process.stdout,)).start()
            threading.Thread(target=self._print_output, args=(self.process.stderr,)).start()
        except Exception as e:
            print(Fore.RED + f"Error: Failed to start the application. {e}")

    def _print_output(self, stream):
        for line in iter(stream.readline, ''):
            print(line, end='')

class Handler(FileSystemEventHandler):
    def __init__(self, watcher):
        self.watcher = watcher

    def on_any_event(self, event):
        if event.is_directory:
            return None
        elif event.event_type == 'modified':
            # Restart the application
            self.watcher.start_process()

if __name__ == '__main__':
    w = Watcher()
    try:
        w.start_process()  # Start the initial process
        w.run()
    except Exception as e:
        print(Fore.RED + f"Error: {e}")