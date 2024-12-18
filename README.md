# Cryptotracker
 
## Prerequisites
- git
- docker
- dotnet 8

## Getting Started

1. Clone this repository
2. Open a terminal and move the root directory
3. Start the containers `docker-compose -f .\docker-compose.development.yaml up --build`
4. If not already installed, install Entity Framework tooling `dotnet tool install --global dotnet-ef`
5. Move into rest-api/API and apply the database migrations `dotnet ef database update`
6. Go to localhost:5173 for frontend or to localhost:8080/swagger for the api

