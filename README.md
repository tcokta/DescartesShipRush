# DescartesShipRush Binary Data Diff API

This is a simple C# .NET Web API for comparing binary data provided through HTTP endpoints. It exposes endpoints for submitting binary data to the left and right sides and retrieving the comparison results.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) or any other C# IDE
- [Postman](https://www.postman.com/) or any other API testing tool like built in swagger

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/binary-data-diff-api.git

2. Open the solution in Visual Studio.

3. Build the solution to restore dependencies.

4. Run the application.

## Usage

### Endpoints
1. Left Endpoint

   -**HTTP METHOD**: POST

   -URL: `/v1/diff/{id}/left`

   -**Request body**
   
      ```
      {
        "Data": "base64_encoded_binary_data"
      }
      ```

2. Right Endpoint

   -**HTTP METHOD**: POST

   -URL: `/v1/diff/{id}/right`

   -**Request body**
   
      ```
      {
        "Data": "base64_encoded_binary_data"
      }
      ```

3. Diff Endpoint

   -**HTTP METHOD**: GET

   -URL: `/v1/diff/{id}`

   -**Response**:
     - If data is equal: `{}`
     - If data size is not equal: `{}`
     - If content does not match:
   
        ```
        `{
          "DiffResultType": "ContentDoNotMatch",
          "Diffs": [
            {
              "Offset": 0,
              "Length": 1
            },
            {
              "Offset": 2,
              "Length": 2
            }
          ]
        }`
        ```
## Example

  1. Send data to the left endpoint
  
    `curl -X POST -H "Content-Type: application/json" -d '{"Data": "aGVsbG8="}' http://localhost:YourPortNumber/v1/diff/1/left`
    
  2. Send data to the right endpoint
  
    `curl -X POST -H "Content-Type: application/json" -d '{"Data": "aGVsXGxv"}' http://localhost:YourPortNumber/v1/diff/1/right`
     
  3. Get the diff result

    `curl http://localhost:YourPortNumber/v1/diff/1`

## Testing

### Integration Tests

Integration tests are included in the `BinaryDataDiffIntegrationTests` class. These tests cover scenarios where data is equal, not equal in size, and content does not match.

### Unit Tests

Unit tests can be added for individual methods in the `BinaryDataDiffController` class. These tests ensure that the controller's logic is working as expected.
