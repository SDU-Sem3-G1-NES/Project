## Authentication

Pass the API key as a header:

ApiKey: your-api-key-here


## Endpoints

# Get All Tables

**GET** `/api/table`

- `200 OK`: Returns an array of table IDs.
- `404 Not Found`: No tables found.


# Get Full Table Info

**GET** `/api/table/{guid}`

# Response

- `200 OK`: Returns all information of the specified table.
- `404 Not Found`: Table not found.


# Set Table Height

**PUT** `/api/table/{guid}/height`

# Response

- `200 OK`: Table height set successfully.
- `404 Bad Request`: Table not found.
- `503 Service Unavailable`: Failed to set table height.