### Unit and Integration Testing with Moq and TestContainer in xUnit

**Overview**

This <b>README</b> provides instructions and explanations for the unit and integration tests implemented in this project using the xUnit framework. We utilize two primary libraries to facilitate testing:

- <b>Moq:</b> A popular mocking framework for .NET, used to create and manage mock objects for unit testing.
- <b>TestContainer:</b> A library that allows running lightweight, disposable containers (e.g., Docker) for integration testing. This is especially useful for testing with external dependencies like databases.

**Prerequisites**

Before running the tests, ensure you have the following installed:

- .NET SDK
- Docker (for unit & integration tests using TestContainer)
- xUnit framework

**Project Structure**

The test project is organized into the following sections:

- <b>Unit Tests:</b> Focused on testing individual methods or classes in isolation using mock dependencies.
- <b>Integration Tests:</b> Validates the interaction between components and external systems (e.g., databases) in a controlled environment.
