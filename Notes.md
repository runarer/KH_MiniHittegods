# Notater til MiniHittegods

## Spørsmål

### List med queries

Hvor skal queriene utføres. I service, da på en liste returnert fra repository.
Eller skal queriene, altså status, availablility og search sendes til repository for
å bli utført der før svar iform av List returneres til service.

**Svar fra Mr. Google Man:**
In summary, simple data retrieval queries belong in the repository, while the service
layer consumes the repository's results to fulfill business requirements. The placement
of highly complex, use-case-specific queries is an architectural decision, often solved with
dedicated query patterns or within the service layer itself.

In software architecture using the service and repository patterns,
queries for data are handled in different ways depending on their complexity and the
specific architectural style (e.g., standard layered architecture vs. CQRS).

Repository Layer
The repository layer is responsible for encapsulating data access logic and interacting
directly with the data store (e.g., a database).

    - Basic CRUD Operations: Repositories typically provide basic Create, Read, Update,
    and Delete (CRUD) methods for a specific entity or aggregate.
    - Simple Queries: Standard queries to find entities based on simple criteria (e.g.,
    "find all users with age > 30") reside in the repository.
    - Isolation: The repository keeps the data access logic isolated from the rest of the
    application, making it easier to perform unit tests in isolation and potentially
    switch data stores.
    - Abstraction: The goal is to make the data store seem like an in-memory collection
    of objects to the business logic layer.

Service Layer
The service layer implements the application's business logic,
orchestrates operations, and handles transactions.

    - Orchestration and Business Logic: Services use one or more repositories to
    retrieve raw data and then apply business rules, validation, or complex logic
    to transform that data into a result the application needs.
    - Complex Queries (Debated): There is debate on where complex queries that involve
    joining multiple tables or require a lot of application-specific filtering should go.
        - Argument for Service Layer: Some argue that if a query is specific to a particular
        business use case and involves logic beyond simple data retrieval (e.g., calculating
        employee productivity from multiple data points), it belongs in the service layer.
        - Argument for Repository Layer: Others maintain that all database operations,
        including complex joins and filtering, should be kept in the repository for
        reusability and to keep the service layer clean of data access specifics.

Best Practices & Patterns

    - Separation of Concerns: A key principle is separation of concerns: repositories
    handle data persistence, and services handle business operations.
    - Query Objects/Read Services: For very complex or analytical queries, a
    common approach, especially in architectures like CQRS, is to use dedicated "query objects"
    or "read services" that are separate from the main repositories. These services might
    directly query the database to get only the necessary data (often as a Data Transfer
    Object - DTO), bypassing the domain model if integrity checks are not needed for a
    read operation.
    - Avoid Passing IQueryable: Generally, services should not return IQueryable objects
    to the presentation or controller layer, as this pushes data access logic up the
    stack and makes testing difficult.
