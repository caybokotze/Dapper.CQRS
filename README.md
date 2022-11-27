# Description

> Dapper.CQRS is a CQRS pattern wrapper for Dapper (micro-orm). It provides you with a basic set of classes and interfaces to write complex queries structured according to the CQRS pattern. This helps you to maintain a clean code architecture for small and very large applications alike.

# WTF is Dapper?

Dapper is a lightweight micro-ORM developed by Stack-Overflow as an alternative to entity framework. A developer at stack overflow built it to solve the issue they were having with Entity Frameworks bulky slow queries. Dapper solves that by being almost as fast as [ADO.NET](http://ado.net/) but easier to use and map objects against each other. Dapper gives the developer more control by offering the ability to map objects against native SQL queries.

# WTF is CQRS?

CQRS stands for Command Query Responsibility Segregation. The idea behind it is to be able to separate your commands (DML) and your queries (DQL). CQRS is a great pattern to follow for small or large systems and offers the flexibility to keep all your database interactions very structured and orderly as the app scales. It is quite similar to the repository pattern, however instead of using interfaces as abstractions we are abstracting every database transaction as a class instead of the context-based model that is used in EF.

# Getting Started

Get Dapper.CQRS nuget lives here: https://www.nuget.org/packages/Dapper.CQRS

You can also skip that and just install directly into your project of choice.

```shell
dotnet add package Dapper.CQRS --version 2.0.2
```


## Register in your service container
Some basic setup of the Query and Command Executor is all that is required.

```csharp
container.AddTransient<ICommandExecutor, CommandExecutor>();
container.AddTransient<IQueryExecutor, QueryExecutor>();
// Note: You can use the database provider of your own choosing...
container.AddTransient<IDbConnection, MySqlConnection>(_ => new MySqlDatabaseConnection(GetConnectionString()));
```

## Examples

### Our basic models
```csharp
public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public virtual UserDetails UserDetails { get; set; }
}

public class UserDetails
{
    [Key]
    public int Id { get; set; }
    public string? IdNumber { get; set; }
    
    public int UserId { get; set; }
}
```

### Queries
Define your queries as such. As you see in this example a query is being executed within a query. If the use case fits that problem you are free to do so.

Most queries you write will probably not be nested within each other. However, this is just to illustrate is can be done easily even though it might not always be the right solution.

Note how clear this is and how easy it is to figure out what is being done without a lot of additional context required.

```csharp
public class FetchUserById : Query<User?>
{
    private readonly int _userId;
    
    public FetchUserById(int userId) {
        _userId = userId;
    }
    
    public override User? Execute()
    {
        var userDetails = QueryExecutor.Execute(new FetchUserDetailsByUserId(_userId));
        var user = QueryFirst<User>("select * from users where id = @Id", new {Id = _userId});
        user.UserDetails = userDetails;
        return user;
    }
}
```

If we need to bind these two queries within a transaction scope we can easily do that as well.

```csharp
public class FetchUserById : Query<User?>
{
    private readonly int _userId;
    
    public FetchUserById(int userId) {
        _userId = userId;
    }
    
    public override User? Execute()
    {
        using var scope = new TransactionScope();
        var userDetails = QueryExecutor.Execute(new FetchUserDetailsByUserId(_userId));
        var user = QueryFirst<User>("select * from users where id = @Id", new {Id = _userId});
        user.UserDetails = userDetails;
        scope.Complete();
        return user;
    }
}
```

### Commands

Now you are ready to get started using the CQRS setup within the project. All you have to do now is to start creating your command and query classes and plug them into your controllers.

## Dependency resolution example
Making use of your queries and commands is very straight forward. All you have to do is inject them into your desired service of choice and get on with it.

```csharp
public class HomeController : ControllerBase
{
	public ICommandExecutor CommandExecutor { get; }
	public IQueryExecutor QueryExecutor { get; }
	
	public HomeController(
	    ICommandExecutor commandExecutor, 
	    IQueryExecutor queryExecutor)
	{
	    CommandExecutor = commandExecutor;
	    QueryExecutor = queryExecutor;
	}
	
	publid User Foo(int id) {
	    return QueryExecutor.Execute(new FetchUserById(id));
	}
}
```


## Typical project structure
This is typically what a CQRS based project would be structured like. This is very beautiful and clear. The responsibility of every query and command is very clear and this scales very well as the project grows larger. No messy interfaces and that gooby mess that happens with the UnitOfWork/Repository pattern. All reponsibilities should be clearly defined.

```bash
├───Data
│   └───Queries
│   │   └───User
│   │       ├───FetchUserById.cs
│   │       ├───FetchUserByEmail.cs
│   │       └───FetchUserByGuid.cs   
│   │
│   └───Commands
│       └───User
│           └───FetchUserByGuid.cs   
```


# Package dependencies

The Dapper.CQRS Core library is built with .NET Standard 2.1. There are some other dependencies on .NET 6 which include...

- Dapper (2.0.123)
- Microsoft.Extensions.DependencyInjection.Abstractions (6.0.0)
- Microsoft.Extensions.Logging.Abstractions (6.0.3)

# Database Support

> This is really up to you. All database connections should inherit from IDbConnection. As long as this is the case it will be supported. That is the advantage of Dapper and Dapper.CQRS isn't any different.

# Feature List

- [x] CommandExecutor execution interface
- [X] QueryExecutor execution interface
- [X] BaseSqlExecutor with virtual members for easy unit testing
- [X] Commands which inherit from BaseSqlExecutor
- [X] Queries which inherit from BaseSqlExecutor

# Pull Requests

If you would like to contribute to this package you are welcome to do so.

## Testing Coverage

Testing coverage currently covers:

- QueryExecutor with return type
- CommandExecutor with return type
- CommandExecutor without return type
- Queries
- Commands with return types
- Commands without return types