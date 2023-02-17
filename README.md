

# API Documentation for Polling Stations
This API allows you to manage polling stations, including creating, retrieving, updating, and deleting polling stations. 
The API provides the following endpoints:

## Polling stations endpoints 
### POST /api/polling-stations
This endpoint is used to create a new polling station.

**Request**

The request must include a JSON body with the following parameters:

Parameter|Type|Required|Description
|---|---|---|---|
|latitude|number|Yes|The latitude of the polling station location.|
|longitude|number|Yes|The longitude of the polling station location.|

**Response**

If the request is successful, the API will return a JSON object with the following parameters:

|Parameter|Type|Description|
|---|---|---|
|id|guid|The unique identifier of the new polling station.|
|latitude|number|The latitude of the polling station location.|
|longitude|number|The longitude of the polling station location.|

### GET /api/polling-stations
This endpoint is used to retrieve a list of all polling stations.

**Request**

|Parameter|Type|Required|Description
|---|---|---|---|
|page|number|Yes|The latitude of the polling station location.
|pagesize|number|Yes|The longitude of the polling station location.

**Response**
If the request is successful, the API will return a JSON object with a list of polling stations. Each polling station object will have the following parameters:

|Parameter|Type|Description|
|---|---|---|
|id|string|The unique identifier of the polling station.|
|latitude|number|The latitude of the polling station location.|
|longitude|number|The longitude of the polling station location.|

### PUT /api/polling-stations/{id}
This endpoint is used to update an existing polling station.

**Request**
The request must include a JSON body with the following parameters:

| Parameter|Type|Description
|---|---|---|
| id|string|The unique identifier of the polling station.|
| latitude|number|The latitude of the polling station location.|
| longitude|number|The longitude of the polling station location.|

**Response**
If the request is successful, the API will return a JSON object with the updated polling station.
The polling station object will have the same parameters as described in the POST /api/polling-stations endpoint.

### GET /api/polling-stations/{id}
This endpoint is used to retrieve a single polling station by its ID.

**Request**
The id parameter in the URL specifies the ID of the polling station to retrieve.

**Response**
If the request is successful, the API will return a JSON object with the specified polling station.
The polling station object will have the same parameters as described in the POST /api/polling-stations endpoint.

### DELETE /api/polling-stations/{id}
This endpoint is used to delete a single polling station by its ID.

**Request**
The id parameter in the URL specifies the ID of the polling station to delete.

**Response**
If the request is successful, the API will return a JSON object with a message indicating that the polling station was successfully deleted.

### Error Codes
The API may return the following error codes:

|Error|Code|Description
|---|---|---|
|200|Ok|Success status response with details of a polling station.|
|204|No Content |Success status response code indicates that a request has succeeded.|
|400|Bad Request |The request is invalid.|
|404|Not Found | The specified polling station does not exist.|
|500|Internal Server Error |The server encountered an error.|
