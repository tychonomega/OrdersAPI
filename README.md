# OrdersAPI

## Overview	

This API was created with many considerations, but ultimately time-restricted by time available and the indicated desire to keep the time invested low.  I will do my best to be concise in my description, understanding that those reviewing this will likely have the knowledge to explore things in depth and find what they are looking for if not explained here.

## Prereq

- .Net 6 LTS
- Docker
- Editor of choice (I used VS 2022 Enterprise)
- Internet connection for Auth
- (Optional) Tool Like MongoDBCompass if you want to manually inspect the DB
- Hopefully I am not forgetting anything, was trying to keep dependencies low


## About the application
The main components of the application are as follows
- .NET Core Web API Project (OrdersAPI)
- .NET Core Testing Application (xUnit, OrdersAPI.UnitTests)
- Auth0 integration for Authentication/Authorization
- MongoDB for Persistence
- MongoExpress for viewing data if you dont have other options at hand.
  - It isnt the best, but is accessible


## Running the Aplication
- Navigate to the root of the solution and run:
  - docker compose up (if you want to have the console output)
  - docker compose up -d (for daemon mode, you can view logs in docker desktop)
- Can also be ran from your IDE, if desired and supported by it.
- Application ports must not be changed, or Authentication will fail due to callback url issues.
  - you can point at your own Auth0 if you like and tweak the config
- Test project can be ran from Visual Studio or command line
  - Navigate to test project directory and run
    - dotnet test

## Additional Info

### Application Links
  - Swagger Page for Orders API [http://localhost:6060/index.html](http://localhost:6060/index.html)
    - Click the green Authorize button in the top right of the screen to create an account and login.
    - The Messages Controller simply contains public and protected routes to verify auth
    - The Orders Controller contains the requested operations
  - MongoExpress [http://localhost:8081/](http://localhost:8081/)
    - Credentials should be admin:pass, but check the logs if needed, they will be there on initialization

### Considerations
The application is not perfect.  Expect to run into typos and some shortcuts.  There are places that I left hard-coded keys/secrets/configuration that would
normally exist in a secure location, especially not source control.  These were left to keep things as simple for you to run as I could.

If you have any issues with my implementation, maning conventions, coding styles, or design choices, I would be happy to discuss them with you.

Source code management is on a single main branch.  I did not go through the effort of trying to simulate reviews, pull requests, or branching/merging.

I tried to make the code obvious (not requiring alot of comments).  Comments were added to some of the areas that, without further requirements, 
are a basic implementation, or are a shortcut that could baloon into areas of debate or architecture requirements.

Auth0 is configured to require 2FA, either TOTP, or Push Notifications with Auth0 Guardian.  You will need to create an account to call the api.  a throwaway gmail is fine.

The tests are set up as integration tests.  Setting up the Mocking framework for proper unit tests would have taken much longer, and frankly without many requirements, the integration tests were more valuable in my TDD.  The unit tests would have been extremely thin for the time spent on basic CRUD operations.

I have gone through some effort to make sure that this will run as easily as indicated above, but there are environmental issues that could cause difficuly.  If you run into any issues, please reach out and we can get it sorted out.


## Known Issues
- Swagger Authentication
  - If you login, logout, and login again without closing the modal window, it throws an error.  Just close it and open it again.


I think most else is self-explanatory.  If not, or I have left something critical out, please reach out for discussion.