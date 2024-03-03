# bca-api

This API allows users to create and query vehicles. 
Allows for creating, starting, bidding and closing auctions.

## API Documentation

Although after launching the application, Swagger UI documentation can be found at 'http://localhost:8080/swagger', the complete OpenApi spec can be found [HERE](Specs/api.yaml)

## Decisions and Assumptions

### Domain Model
- This domain model has 2 entities - Vehicle and Auction  
- Each entity has its own repository
- There is an auction service to handle the more complex business cases (such as opening auctions)

#### Vehicle

The vheicle class is simple, it is defined as an abstract class with the common properties and Suv, Sedan and Truck classes implement the specific properties, inheriting all from vehicle.
Forcefully, all the vheicles have a UniqueIdentifier, Manufacturer, Model, Year and StartingBid.

#### Auction

- The Auction can only be on 2 Status -> Open and Closed  
- When an Auction is Closed and has bids, it means the car is sold.

#### Design Decisions 

* Separation Between Vehicle and Auctions
    * Easier for future seperation in two micro-services: One for managing Vehicles, one for managing Auctions
    * Although a vehicle can only have one open Auction, separating those two entities allow for, if needed, to have a auction that had no bids be closed, change the Starting Bid of the Vehicle and start a new Auction (Feature not implemented, but is easy to implement it with current structure)
* Factory for creating vehicles
    * Since there are 3 types of vehicles available, each one with different properties, I decided to implement a Factory to ease the creation of the Vehicles depending on the concrete type
* Mediator and MediatR
    * The mediator pattern allows for low coupling between system components. Instead of talking to the services/handlers directly, when performing a command/query on the system, the system only needs to talk to the mediator which will know how to behave with the specific request.
    * MediatR is a popular library with tons of downloads and eases the implementation of the mediator pattern, thus making it a good option to have
* FluentResults
    * Propagating exceptions is costly. Exceptions should only be propagated for unexpected system behavior. I added FluentResults to the code so my system talks with Result objects instead of conctrete types directly, allowing me for checking if some failure occured along the runtime of the application. Only in case of unexpected failures (such as a repository failing: should only fail in case of a timeout) is where the application throws exceptions.

## How To Run The API Locally

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker](https://www.docker.com/products/docker-desktop)
- Optional: Make

### Using .NET CLI
* If make is installed:  
    1. `make build`  
    2. `make run`
1. Restore Packages:  
`dotnet restore`
2. Build Solution:  
`dotnet build`
3. Run Api:  
`dotnet run --project src/Api/Api.csproj`
### Using Docker
* If make is installed:  
    1. `make run-environment`  
    2. `make run-migrations`

1. Run the environment:  
`docker compose up -d`
2. Run Migrations:
`dotnet ef database update --project src/Migrations/Migrations.csproj`
## Built With
- .NET 8
- MediatR, FluentResults and FluentValidation
- xUnit, FluentAssertions, NSubstitute and AutoFixture for testing
- Docker

