# Leave Management API

The Leave Management API is designed to facilitate the management of leave requests within an organization. This API provides a comprehensive set of endpoints that allow employees, managers, and HR personnel to efficiently handle various aspects of leave management, including `requesting`, `approving`, and `tracking` leave requests in real time.

## Endpoints

- [Submit new leave request](./Docs/Submit-Leave-Request.md)
- [Validate leave request](./Docs/Validate-Leave-Request.md)
- [Track the status of my leave requests.](./Docs/Track-Leave-Request-Status.md)

## Architecture

DDD
clean archi
EF core
unit and integration tests
=> code coverage: 80% with screenshot

real time implemetation

## Implementation

To simplify the test case, the Leave Management API is build with `inMemory` database for easy setup, data isolation, and flexibility.

## Local running

- Just start the API project and play with Swagger.

## Bonus Question

## Future improvements

- Add a leave balance verification feature to calculate the number of leave days an employee has taken according to the maximum allowable days for each leave type.
