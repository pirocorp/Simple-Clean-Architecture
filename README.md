# Clean Architecture

Simple Clean Architecture example with ***NO*** DDD, CQRS, etc. Aplication layer is Service Layer.

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
