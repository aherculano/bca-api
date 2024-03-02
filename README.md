# bca-api

This API allows users to create and query vehicles. 
Allows for creating, starting, bidding and closing auctions.

## API Documentation

Although after launching the application, Swagger UI documentation can be found at 'http://localhost:8080/swagger', the complete OpenApi spec can be found [HERE](Specs/api.yaml)

## Decisions and Assumptions

### Domain Model
- This domain model has 2 entities - Vehicle and Auction  
- Each entity has its own repository
- I opted for implementing an anemic domain model with a domain service to implement business logic on the auction side of things 

#### Vehicle

The vheicle class is simple, it is defined as an abstract class with the common properties and Suv, Sedan and Truck classes implement the specific properties, inheriting all from vehicle.

#### Auction

- The Auction can only be on 2 Status -> Open and Closed  
- When an Auction is Closed and has bids, it means the car is sold.

#### Design Decisions 

* Separation Between Vehicle and Auctions
    * Easier for future seperation in two micro-services: One for managing Vehicles, one for managing Auctions
    * Although a vehicle can only have one open Auction, separating those two entities allow for, if needed, to have a auction that had no bids be closed, change the Starting Bid of the Vehicle and start a new Auction (Feature not implemented, but is easy to implement it with current structure)

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
    1. `make build-docker`  
    2. `make run-docker`

1. Build Image:  
`docker build -t bca-api:latest .`
2. Run Image:  
`docker run -p 5000:8080 bca-api:latest`
## Built With
- .NET 8
- MediatR, FluentResults and FluentValidation
- xUnit, FluentAssertions, NSubstitute and AutoFixture for testing

