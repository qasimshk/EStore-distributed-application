# EStore Distributed System

### Introduction - [Online Demo](http://gateway.runasp.net/swagger/index.html)

The EStore distributed system project is developed (using [MassTransit](https://masstransit.io/)) to explain the concept of orchestration microservice architecture & sagas patterns. As the name says this orchestrator service is responsible for the workflow of each asynchronous transaction. The payload sent by the gateway API is stored in the database and an entry is created. The created entry state is updated on each successful response sent by the consuming microservices.

![](https://github.com/qasimshk/EStore/blob/main/Database/EStore%20Design.drawio.png)

| APIs  | URLs |
| ------------- | ------------- |
| Gateway | [View Online](http://gateway.runasp.net/swagger/index.html) |
| Orchestrator | [View Online](http://orchestrator.runasp.net/swagger/index.html) |
| EStore | [View Online](http://estoresrv.runasp.net/swagger/index.html) |

### Orchestrator vs Choreography patters in microservice architecture 
Both the Orchestrator and Choreography patterns are used in Microservices architecture to coordinate the interactions between services. Here are some advantages of using each pattern:

**Advantages of Orchestrator Pattern:**

- Centralized Control: In this pattern, a central service (Orchestrator) is responsible for coordinating the interactions between services. This provides a single point of control for managing business logic, and helps to ensure that business processes are executed consistently and reliably.

- Flexibility: Orchestrator Pattern allows for a more flexible service design. Services can be loosely coupled and do not need to be aware of each other. This makes it easier to add or remove services, and modify business processes without impacting other services.

- Easier to monitor: Since the Orchestrator service is responsible for coordinating interactions between services, it can also provide detailed monitoring and logging of business processes. This can help to identify issues and improve the overall performance of the system.

**Advantages of Choreography Pattern:**

- Decentralized control: In this pattern, services communicate with each other directly, without the need for a central orchestrator. This makes the system more flexible and less reliant on a single point of failure.

- Scalability: Since each service is responsible for its own actions, it is easier to scale the system by adding more instances of individual services. This makes the system more resilient and able to handle high loads.

- Simplicity: Choreography Pattern is easier to implement and maintain as services are not dependent on a central orchestrator. This makes the system more straightforward to develop and test.

In summary, both patterns have their own advantages and disadvantages. Orchestrator Pattern is more suitable for complex business processes that require a centralized control mechanism. Whereas, Choreography Pattern is more suitable for simpler processes that require a decentralized, scalable and more flexible design.

### Project Setup:
- Install [Microsoft .Net 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Open solution in Visual Studio.
- Navigate to the project opened in visual studio and expand the database folder, execute both script files one by one to setup both the databases (NOTE: scripts files are generated using SQL 2022).
- Set database connection string in appsettings.josn for both estore api and orchestrator service.
- Set RabbitMQ configuration ( either URL or host name, user & password ) in both gateway api and orchestrator service.
- Set visual studio to start multiple projects and make sure orchestrator service, gateway api & estore api are set as start.

### Project user guide:
- Place an order using the request below in swagger post order.
- Use correlation Id to check the order state.
- Use correlation Id to check the completed order payment state.
- Complete order can be refunded using correclation Id.
- All orders including payment state records can be removed using correlation Id. 

#### Order Request
```http
{
  "customer": {
    "companyName": "Dell Public Ltd",
    "contactName": "Ben Thompson",
    "contactTitle": "Software Engineer",
    "address": "11 Albert street, Greenwich",
    "city": "London",
    "region": "Greater London",
    "postalCode": "SE2 0HF",
    "country": "United Kingdom",
    "phone": "1234567890",
    "fax": "03001234"
  },
  "order": {
    "employeeId": 5,
    "shipVia": 2,
    "freight": 3,
    "shipName": "Argos Pvt Ltd",
    "address": "11 Albert street, Greenwich",
    "city": "London",
    "region": "Greater London",
    "postalCode": "SE18 7RU",
    "country": "United Kingdom",
    "orderDetailsRequest": [
      {
        "productId": 13,
        "unitPrice": 15.22,
        "quantity": 3,
        "discount": 2
      },
      {
        "productId": 54,
        "unitPrice": 16.78,
        "quantity": 4,
        "discount": 5
      },
      {
        "productId": 40,
        "unitPrice": 10.11,
        "quantity": 1,
        "discount": 0
      }
    ]
  }
}
```

### Library & NuGet

- REST API
- RabbitMQ
- SQL Server
- MassTransit

<!-- 
qasim.kainos@gmail.com - gateway
vzimsim@gmail.com - orchestrator
savvysavingsapp@gmail.com - api 
https://www.monsterasp.net/
-->
