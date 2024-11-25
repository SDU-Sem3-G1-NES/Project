FROM python:3.12

WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
RUN chmod 644 config/cert.pem config/key.pem && chown root:root config/cert.pem config/key.pem
CMD ["python", "simulator/main.py", "--desks", "400", "--port", "8433", "--https", "--certfile", "config/cert.pem", "--keyfile", "config/key.pem", "--disablesim"]
EXPOSE 8433