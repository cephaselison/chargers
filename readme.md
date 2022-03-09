# Zaptec Interview Challenge

In this challenge you'll work on a simplified charging system. The system consists of two parts, a backend and a frontend. The backend is written in .NET (C#) and is located in the `Backend` directory. The frontend is an Angular SPA and is located in the `Frontend` directory.

Please read through this document before starting your work.

## Getting started

Before you can start your implementation, you'll need to get the application running.

First install [.NET 6](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org/en/).

After installing these dependencies you can start the backend by running this command from the `Backend` directory:

    dotnet watch run

This will start the application and will rebuild automatically if you do any changes to the backend source. The backend application is running on localhost port 5000.

After installing frontend dependencies, by running the following command in the `Frontend` directory:

    npm install

You can start the frontend application from the `Frontend` directory:

    npm start

After starting both backend and frontend applications you should be able to access the application from the following URL: [http://localhost:5050](http://localhost:5050).

If you do not have a preferred code editor, we recommend you download and use [Visual Studio Code](https://code.visualstudio.com/).

## Your challenge

You will be working on improving this simple charging system.

The charging system main parts are installations and chargers. An installation maps to a physical electrical circuit with limited by a circuit breaker. Chargers are installed in groups in installations. The total charge current of all chargers in the installation cannot exceed the fuse current. Chargers are limited by minimum (10A) and maximum (32A) charge current, these limits cannot be changed as they represent physical limitations of the vehicles.

After opening the frontend in your preferred browser ([http://localhost:5050](http://localhost:5050)), you'll see a list of chargers. Each charger has a button to connect a vehicle. When a vehicle is connected, the charger will request current to be allocated, and will start charging with this current. After the battery is full, charging will stop. You can at any time disconnect a vehicle by clicking the disconnect button.

### Tasks

The tasks are not prioritized but are numbered for reference; you are free to tackle them in the order you would like. We would like you to spend at most 4 hours on the challenge and there is no requirement to complete everything. If you skip tasks or are unable to complete everything, please have a think about the questions so that we can have a discussion in the next interview.

The tasks are grouped in frontend and backend sections. If you are interviewing for a frontend position feel free to ignore backend tasks, and for a backend position the frontend tasks.

#### Frontend

Most of our frontend is currently implemented using AngularJS, and the provided frontend is implemented with this Angular version. As mentioned in the meet & greet we are starting migration to React in the near future.

1. Replicate the frontend using React.
2. The frontend does not update when backend state change. I.e. you cannot see the vehicle battery level or the actual charge rate without reloading the frontend. Extend the frontend to make it update when backend state changes.
3. Create a reusable component to display charger state.

##### Frontend bonus tasks

If you've completed the tasks above, congratulations! The following are optional tasks you can do if you're still having fun and would like some more challenges.

1. If you polled for backend state changes in task 2. above, refactor your solution to use websockets or similar to push state changes to frontend in real time.
2. Improve the app's look and feel.

#### Backend

1. The backend `ChargeController` class is responsible for allocating charge current to the active chargers. Currently this class is very naive; always allocating the chargers maximum allowed charge current. This means that if more than one charger is active, the installation fuse will trip.

    Refactor the `ChargeController.RequestCharge` method so that the installation circuit breaker is not tripped when more than one vehicle is connected. The available current should be shared as equally as possible between the chargers. If you have not done the second frontend task you can check whether the fuse is tripped by either inspecting the backend console output, or manually reloading the frontend in your browser. When fuse is tripped, you'll have to restart the backend application to reset fuse state.
    
    Remember that you should not allocate charge current below the charger minimum current, and you may need to queue chargers if too many are trying to charge simultaneously.
2. As a vehicle's battery is charged, its charge rate will decrease. When a vehicle is charging with less than its allocated current, the unused current should be allocated to other chargers. The same goes if a vehicle is disconnected, and all allocated current can be freed for other connected vehicles. Refactor the `ChargeController.OnChange` method to implement this change.

##### Backend bonus tasks

If you've completed the tasks above, congratulations! The following are optional tasks you can do if you're still having fun and would like some more challenges.

1. Persist backend state to file or database (e.g. SQLite) to persist state across application restart.
2. Outline how the application could be deployed to Azure services. E.g. what services to use? What deployment process would you recommend? Could/should parts of the application be replaced by Azure services?

## Implementation notes

- For simplicity, application state is kept in static variables. Restarting the application will reset state. Note that restart happens automatically when you save changes to the backend code and trigger a rebuild.
- It takes a few seconds for the backend to rebuild and restart when changes are detected. If you reload the frontend while backend is rebuilding, the API calls will fail. If API calls keep failing, inspect backend console for any compile or runtime errors.
- Chargers use the static `Queue` class to notify `Installation` of changes.
- The application uses Angular 1.6. This is a bit out of date, but it is what our real frontend is currently using. You'll find the Angular 1.6 documentation here: https://devdocs.io/angularjs~1.6
- Ideally you should only need to change the `ChargeController` class. If there are bugs or you need to change other classes, please feel free to do so, but provide an explanation why this was necessary.
- This is not a requirement, but feel free to add a unit test project if this helps you to implement your changes.

## Submission notes

- The challenge is distributed as a git repository. Please commit when appropriate using well-formed commit messages. Include the repository folder (`.git`) with your submission.
- If you copy code from any external resources (e.g. Stack Overflow), please provide a list of reference URLs.


# Good luck! :-)