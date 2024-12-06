# LeaveManagement Api v1

The Leave Management API is designed to facilitate the management of leave requests within an organization.

This API provides a comprehensive set of endpoints that allow employees, managers, and HR personnel to efficiently handle various aspects of leave management,including requesting, approving, and tracking leave requests.

## Authentication

- HTTP Authentication, scheme: **Bearer**.
  - Add your **JWT token** as an authorization HTTP header.

## Endpoints

### `PATCH /api/v1/AdminLeaveRequests/{id}`

Update leave request status. Only users with `HR` admin roles can approve or reject a leave request.

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

```http
PATCH /api/v1/AdminLeaveRequests/0 HTTP/1.1
Authorization: bearer
Content-Type: application/json
Accept: application/json
Host: example.com

{
  "status":"Pending",
  "decisionReason":"string"
}
```

<h3>Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|integer(int32)|true|The leave request to update Id.|
|body|body|[UpdateLeaveRequest](#updateleaverequest)|true|None|

#### UpdateLeaveRequest
|Name|Type|Required|Description|
|---|---|---|---|
|status|string|true|Only `Approved` or `Rejected` are supported.|
|decisionReason|string¦null|false|For rejected requests a rejection reason is **mandatory**.|

<h3>Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[UpdatedLeaveRequestDto](#updatedleaverequestdto)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Validation errors occurred.|None|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized user.|None|
|403|[Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)|Forbidden access.|None|

#### UpdatedLeaveRequestDto
|Name|Type|Required|Description|
|---|---|---|---|
|id|integer(int32)|false|The updated leave request Id.|
|submittedBy|integer(int32)|false|none|The user Id who submitted the leave request.|
|leaveType|string|false|The leave type.|
|startDate|string(date-time)|false|The start date.|
|endDate|string(date-time)|false|The end date.|
|status|string|false|The new updated status.|
|decisionReason|string¦null|false|The decision reason.|

> Example responses

> 200 Response

```json
{
  "id": 1,
  "submittedBy": 1,
  "leaveType": "Off",
  "startDate": "2025-01-01T14:15:22Z",
  "endDate": "2025-01-14T14:15:22Z",
  "status": "Rejected",
  "decisionReason": "HR rejection"
}
```

> 401 Response
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.2",
  "title": "Unauthorized",
  "status": 401
}
```

<h1 id="leavemanagement-api-userleaverequests">User endpoints</h1>

### `POST /api/v1/me/UserLeaveRequests`

Submits my new leave request. Each user has the right to submit a new leave request.

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

```http
POST /api/v1/me/UserLeaveRequests HTTP/1.1
Authorization: bearer
Content-Type: application/json
Accept: application/json
Host: example.com

{
  "leaveType":"Off",
  "startDate":"2025-02-02T14:15:22Z",
  "endDate":"2025-02-10T14:15:22Z",
  "comment":"string"
}
```
<h3>Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[CreateLeaveRequest](#createleaverequest)|false|The leave request details.|

#### CreateLeaveRequest
|Name|Type|Required|Description|
|---|---|---|---|
|leaveType|[LeaveType](#leavetype)|false|The leave type.|
|startDate|string(date-time)|false|The start date which should **not** be in the past.|
|endDate|string(date-time)|false|The end date which should be after the start date.|
|comment|string¦null|false|Optional comment within the request.|

#### LeaveType
|Value|Description|
|---|---|
|Pending|By default each new leave request is pending.|
|Approved|Approved by an HR admin role.|
|Rejected|Rejected by an HR admin role.|

<h3>Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|integer|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Validation errors occurred.|None|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized user.|None|

> Example responses

> 200 Response
```json
1  //the new created leave request Id
```

### `GET /api/v1/me/UserLeaveRequests[?status=approved]`

Get my list of leave requests. The User have the possibility to filter by status.

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

```http
GET /api/v1/me/UserLeaveRequests HTTP/1.1
Authorization: bearer
Accept: application/json
Host: example.com

```

<h3>Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|status|query|string|false|The leave request status|

<h3>Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[LeaveRequestsCollectionDto](#leaverequestscollectiondto)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized user.|None|

#### LeaveRequestsCollectionDto
|Name|Type|Required|Description|
|---|---|---|---|
|count|integer(int32)|false|The total count of items in the list.|
|items|[[LeaveRequestDto](#schemadtos_leaverequestdto)]|false|none|

#### LeaveRequestDto

|Name|Type|Required|Description|
|---|---|---|---|
|id|integer(int32)|false|The leave request Id.|
|leaveType|string|false|none|
|startDate|string(date-time)|false|none|
|endDate|string(date-time)|false|none|
|status|string|false|none|
|comment|string¦null|false|none|
|decidedBy|integer(int32)¦null|false|none|
|decisionReason|string¦null|false|none|

> Example responses

> 200 Response

```json
{
  "count": 1,
  "items": [
    {
      "id": 1,
      "leaveType": "off",
      "startDate": "2025-08-24T14:15:22Z",
      "endDate": "2025-08-24T14:15:22Z",
      "status": "pending"
    }
  ]
}
```

### `POST /api/v1/Users/register`

Register new user.

```http
POST /api/v1/Users/register HTTP/1.1
Content-Type: application/json
Accept: application/json
Host: example.com

{
    "email":"string",
    "firstName":"string",
    "lastName":"string",
    "password":"string",
    "roles":["string"]}
```

<h3>Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[RegisterUserRequest](#schemacontracts_registeruserrequest)|false|New user information.|

> Body parameter

```json
{
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "password": "string",
  "roles": [
    "string"
  ]
}
```

<h3>Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|integer|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Validation errors occurred.|None|

> Example responses

> 200 Response

```json
1 // The registered user Id
```

### `POST /api/v1/Users/login`

Login to generate new user JWT token.

```http
POST /api/v1/Users/login HTTP/1.1
Content-Type: application/json
Accept: application/json
Host: example.com
Content-Length: 38

{
  "email":"string",
  "password":"string"
}
```


> Body parameter

```json
{
  "email": "string",
  "password": "string"
}
```

<h3>Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[LoginUserRequest](#schemacontracts_loginuserrequest)|false|The user email and password.|

<h3>Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|string|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Validation errors occurred.|None|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|User not found.|None|

> Example responses

> 200 Response

```json
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```