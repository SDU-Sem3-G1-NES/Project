***Https doesnt work properly, because theres no SSL certificate.***
***Use --insecure flag in curl. For embedded it might be better to configure the API to use http***

# Authentication

Pass the API key as a header:

ApiKey: your-api-key-here

# Endpoints

## Get Full Table Info

**GET** `/api/table/{guid}`

### Example:

curl -X GET https://localhost:4488/api/Table/cd:fb:1a:53:fb:e6 -H "ApiKey: PUT_KEY_HERE" --insecure

### Response

- `200 OK`: Returns all information of the specified table.
- `404 Not Found`: Table not found.


## Set Table Height

**PUT** `/api/table/{guid}/height`

### Example:

curl -X PUT https://localhost:4488/api/Table/cd:fb:1a:53:fb:e6/height -H "Content-Type: application/json" -H "ApiKey: PUT_KEY_HERE" -d '999' --insecure

### Response

- `200 OK`: Table height set successfully.
- `404 Bad Request`: Table not found.
- `503 Service Unavailable`: Failed to set table height.

## Subscribe (for webhooks)

**PUT** `/api/Webhook/{guid}/subscribe`

### Example:

curl -X POST https://localhost:4488/api/Webhook/cd:fb:1a:53:fb:e6/subscribe -H "Content-Type: application/json" -H "ApiKey: PUT_KEY_HERE" -d "\\"http://localhost:5079/print\\"" --insecure

### Response

- `200 OK`: Subscription added.
- `404 Bad Request`: Subscribtion failed. This table might already have a subscription. Are you using the right format?
"http://www.example.com/example"
- `503 Service Unavailable`: Subscribtion failed. {exception message}

## Unsubscribe (for webhooks)

**PUT** `/api/Webhook/{guid}/subscribe`

### Example:

curl -X POST https://localhost:4488/api/Webhook/cd:fb:1a:53:fb:e6/unsubscribe -H "Content-Type: application/json" -H "ApiKey: PUT_KEY_HERE" --insecure

### Response

- `200 OK`: Unsubscribed successfully.
- `404 Bad Request`: Subscription not found
- `503 Service Unavailable`: Failed to unsubscribe. {exception message}