***Https doesnt work properly, because theres no SSL certificate.***
***Use --insecure flag in curl. For embedded it might be better to configure the API to use http***

# Authentication

Pass the API key as a header:

ApiKey: your-api-key-here

# Endpoints

## Get Full Table Info

**GET** `/api/table/{guid}`

### Example:

curl -X GET https://localhost:4488/api/Table/ee:62:5b:b8:73:1d -H "ApiKey: PUT_KEY_HERE" --insecure

### Response

- `200 OK`: Returns all information of the specified table.
- `404 Not Found`: Table not found.


## Set Table Height

**PUT** `/api/table/{guid}/height`

### Example:

curl -X PUT https://localhost:4488/api/Table/ee:62:5b:b8:73:1d/height -H "Content-Type: application/json" -H "ApiKey: PUT_KEY_HERE" -d '999' --insecure

### Response

- `200 OK`: Table height set successfully.
- `404 Bad Request`: Table not found.
- `503 Service Unavailable`: Failed to set table height.