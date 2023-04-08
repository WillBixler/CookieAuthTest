# CookieAuthTest

Testing .Net auth using cookies and no frontend code

## Usage

- Run the project
- GET https://localhost:7275/getmessage
    - It will return a 401 to prove auth is enforced
- POST https://localhost:7275/security/createtoken with JSON payload ```{ "username": "Test", "password": "User" }```
    - Response should be 200 with no body
    - A new cookie called "token" is created
- GET https://localhost:7275/getmessage again
    - Response should be 200 with body "Hello World"