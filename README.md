## 🧬 Game of Life Challenger API (.NET 9)
A .NET 9 Web API implementation of Conway's Game of Life. This API allows to create a board, and manipulate genereating the next states


## 🚀 Features
- Create  board states
- Generete next stages
- Reset or delete board states



## 📦 Technologies
- .NET 9
- C#
- Clean architecture
- Rest Web API
- Mongo
- in-memory
- Docker
- Swagger (OpenAPI)
- xUnit (unit tests)
- CQRS
- K6

## 📦 Design patterns
- Mediator
- Chain of Responsibility
- Decorator

##  📖 Conway’s Game of Life Rules
- Any live cell with two or three live neighbours survives.
- Any dead cell with three live neighbours becomes a live cell.
- All other live cells die in the next generation. Similarly, all other dead cells stay dead.

# How to run the application
Open your terminal and run the following command to create the Mongo and API containers:
```bash
docker-compose up --build -d
```

open browser http://localhost:5000/swagger/index.html 


samples to create a board:

```json
{
	"name": "Sample test",
	"boardState": [
		[
			true,
			false,
			false
		],
		[
			false,
			true,
			false
		],
		[
			false,
			false,
			true
		]
	]
}
```

## k6 to test API performance

Install K6 using chocolatey
```bash
choco install k6
```

Use the command below to generate performance results in the performance-tests folder.
```bash
k6 run GET-FinalState-.js
```