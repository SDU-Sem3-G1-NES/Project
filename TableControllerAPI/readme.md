## Authentication

Pass the API key as a header:

ApiKey: your-api-key-here


## Endpoints

# Get All Tables

**GET** `/api/tables`

- `200 OK`: Returns an array of table IDs.
- `404 Not Found`: No tables found.


# Get Table Height

**GET** `/api/tables/{guid}/height`

# Response

- `200 OK`: Returns the height of the table.
- `404 Not Found`: Table not found.


# Set Table Height

**PUT** `/api/tables/{guid}/height`

# Response

- `200 OK`: Table height set successfully.
- `400 Bad Request`: Failed to set table height.


# Get Table Speed

**GET** `/api/tables/{guid}/speed`

# Response

- `200 OK`: Returns the speed of the table.
- `404 Not Found`: Table not found.


# Get Table Status

**GET** `/api/tables/{guid}/status`

# Response

- `200 OK`: Returns the status of the table.
- `404 Not Found`: Table not found.


# Get Table Errors

**GET** `/api/tables/{guid}/error`

# Response

- `200 OK`: Returns a list of table errors. Empty if no errors.
