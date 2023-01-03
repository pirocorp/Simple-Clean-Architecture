# Clean Architecture with CQRS

Simple Clean Architecture example with NO DDD, etc. The application layer is implemented like Service Layer from classical n-layered architecture. Highlights the difrences between n-layer and Clean/Hexagon/Onion architectures.

![image](https://user-images.githubusercontent.com/34960418/205628894-ed445a14-203a-4fe0-a603-93bcd1a2f9b4.png)


# Overview

## Domain Layer

This will contain all **Entities**, **ValueObjects**, **Enums**, **Exceptions**, **Interfaces**, types and logic **specific to the domain layer**.

### Entities

Entities encapsulate **Enterprise wide business rules**. An entity can be an object with methods, or it can be a set of data structures and functions. It doesn’t matter so long as the entities could be used by many different applications in the enterprise.

If you **don’t have an enterprise**, and are just writing a single application, then these entities are **the business objects of the application**. They encapsulate the most general and high-level rules. They are the least likely to change when something external changes. For example, you would not expect these objects to be affected by a change to page navigation, or security. No operational change to any particular application should affect the entity layer.


## Application Layer

This will contain all **Interfaces**, **Models**, **Logic**, **Commands/Queries**, **Validators**, **Exceptions**, **CQRS**, **MediatR**, types and logic specific to the application layer.

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

The software in this layer contains application specific business rules. It encapsulates and implements all of the use cases of the system. These use cases orchestrate the flow of data to and from the entities, and direct those entities to use their enterprise wide business rules to achieve the goals of the use case.

We do not expect changes in this layer to affect the entities. We also do not expect this layer to be affected by changes to externalities such as the database, the UI, or any of the common frameworks. This layer is isolated from such concerns.

We do, however, expect that changes to the operation of the application will affect the use-cases and therefore the software in this layer. If the details of a use-case change, then some code in this layer will certainly be affected.


## Infrastructure Layer

This will contain all **Persistence**, **Identity**, **File System**, **System Clock**, **Api Clients**, **Unit of Work**, **Repository Patterns**, types and logic specific to the infrastrucutre layer.

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.


## Presentation Layer

This layer depends on both the **Application** and **Infrastructure layers**, however, **the dependency on Infrastructure is only to support dependency injection**. Therefore only Startup.cs should reference Infrastructure.

Can be **SPA**, **Web API**, **Razor Pages**, **MVC**, **Web Forms**, etc.


## The outermost layer (Infrastructure + Presentation)

The outermost layer is generally composed of frameworks and tools such as the Database, the Web Framework, etc. Generally you don’t write much code in this layer other than glue code that communicates to the next circle inwards.

This layer is where all the details go. The Web is a detail. The database is a detail. We keep these things on the outside where they can do little harm.


# Using [MediatR](https://github.com/jbogard/MediatR) for [CQRS](https://github.com/pirocorp/CSharp-Masterclass/tree/main/09.%20CQRS)

**CQRS stands for Command and Query Responsibility Segregation**, a pattern that separates read and update operations for a data store. The flexibility created by migrating to CQRS allows a system to better evolve over time and prevents update commands from causing merge conflicts at the domain level.

![image](https://user-images.githubusercontent.com/34960418/210239652-92b18c94-865a-4f64-a156-94ba2f681394.png)


## MediatR Pipeline Behaviours

### What are Pipelines

**Requests**/**Responses** travel back and forth through Pipelines in ASP.net Core. When an Actor sends a request it passes the through a pipeline to the application, where an operation is performed using data contained in the request message. Once, the operation is completed a Response is sent back through the Pipeline containing the relevant data.

Pipelines are only aware of what the Request or Response are, and this is an important concept to understand when thinking about **ASP.net Core Middleware**.

Pipelines are also extremely handy when it comes to wanting implement common logic like Validation and Logging, primarily because we can write code that executes during the request enabling you to validate or perform logging duties etc.

### Unhandled Exception Behavior

**UnhandledExceptionBehavior** catches all unhandled exceptions at the application layer, logs them, and throws them for handling in the API layer. **ApiExceptionFilterAttribute** handles Exceptions at the API layer.

## Notifications 

![image](https://user-images.githubusercontent.com/34960418/210239088-bc4fa7f6-eda5-48bf-8aa4-f97311b8b0a1.png)

**MediatR** has concept Notification (event) with several interfaces, INotification, IAsyncNotification. The concept and how it works with MediatR are very similar to a request (Command or Query) and its appropriate handler. The difference with a Notification is it can have **many** handlers.

- Request: only one handler
- Notification: zero to many handlers


## Vertical Slices

Feature Slices or Vertical Slices are the terms used most for organizing by feature. Whatever our feature slice is, it surely makes sense to keep all that code together. 



# Technologies

- [ASP.NET Core 7](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0)
- [Entity Framework Core 7](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://automapper.org/)
- [NUnit](https://nunit.org/)
- [FluentAssertions](https://fluentassertions.com/)
- [Moq](https://github.com/moq)
- [MediatR](https://github.com/jbogard/MediatR)


