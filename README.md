# Project Title

Device App


## List of projects

### DeviceAppAuthentication

* An ASP.Net Core server for authentication. Uses a crude implementation of OAuth 2.0 to protect API resources

### DevicesAPI

* An ASP.Net Core Web API project. A REST API for the Devices application 

### DevicesAPI.Test

* Unit and Integration tests for the DevicesAPI


## How to Run

Debug the projects in the following order. 
* DeviceAppAuthentication
* DevicesAPI

The app uses JWT tokens for verifing requests to the API. An access token is issued to a user when he/she logs into the application. 

To Log-In
* Debug both projects as described above.
* Visit "https://localhost:44344/" in a browser
* Click on the 'Sign-in' button at the top left of the page.
* Use the following default username [john@yahoo.com] and password [Qwert@1]
* Retrieve the token generated and use it in any request to the API

To Register
* Debug both projects as described above.
* Visit "https://localhost:44344/" in a browser
* Click on the 'Register' button at the top left of the page.
* Fill the form and submit. You will be redirected back to the home page
* On the home page, click the 'logout' button at the far left corner of the page
* Sign-in again with the username and password you used during registration. You should be presented with a token.
* Default expiration for the token is 7 days.


