# YamInjection
Light dependency injection engine

Basic principle: Everything should have an interface, dependencies of classes should be on interfaces, those dependencies should be automatically provided to the class. By consitantly following this principle, you get a chain-of-dependency that your dependency injection engine will fill in for you.

## Creating a dependency
Let's suppose we have a program that needs another class to do work for it, a service let's say, that has a method called DoSomething (for this purpose, we'll say it prints out text).

```c#
public class SomeService
{
  public void DoSomething()
  {
     Console.WriteLine("It Worked");
     Console.ReadLine();
  }
}
```

The first thing we should do, is extract an interface from this class, then make the class implement it.

```c#
public interface ISomeService
{
  void DoSomething();
}
```

```c#
public class SomeService : ISomeService
{
...
}
```

Now we need to tell the program how get the interface when we need an instance of it. We'll need to create a class that implements YamInjection's InjectionMap class and override the Register method. Here's what that would look like (don't worry about the finer details, we'll cover that later).

```c#
public class MyInjectionMap : InjectionMap
{
    public override void Register()
    {
        Map<SomeService>()
            .To<ISomeService>()
            .ResolveOnlyOnce();
    }
}
```

Finally, we can code our main method with to resolve that dependency for us (again, don't worry about the stuff you don't understand, we'll cover it later).

```c#
  public class Program
  {
    public static void Main()
    {
        using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
        {
            var myInjectionMap = new MyInjectionMap();
            
            scope.UseMap(myInjectionMap);
            
            var service = scope.Resolve<ISomeService>();
            
            service.DoSomething();
        }
    }
  }
```

That's it. Feel free to test this example for yourself. Now, for the nitty-gritty.

## Mapping
Mapping interfaces to implementations is the way you tell YamInjection (or any DI/IOC) how to resolve dependencies in your classes. We do this by implementing the InjectionMap class included in the package and overriding its Register method.

```c#
public class MyInjectionMap : InjectionMap
{
    public override void Register()
    {
      ...
    }
}
```

Calling the Map method and providing the type to resolve as a generic, will give you options to map the type to an interface, itself, or allow it to be constructed using a factory.

Type to interface:
```c#
Map<SomeService>()
    .To<ISomeService>()
    .ResolveOnlyOnce();
```

Type to self:
```c#
Map<SomeService>()
    .AsSelf()
    .ResolveOnlyOnce();
```

Type using factory:
```c#
Map<SomeDbContext>()
    .Using(() => new SomeDbContext("someConnectionString"))
    .ResolveOncePerScope();
```

After selection to what we resolve this type to, we can define when a new instance is instantiated for the dependency. This can happen only once, once per scope (see Scopes), or every time a dependency exists.

Only once:
```c#
Map<SomeService>()
    .To<ISomeService>()
    .ResolveOnlyOnce();
```

Once per scope:
```c#
Map<SomeService>()
    .To<ISomeService>()
    .ResolveOncePerScope();
```

Every request:
```c#
Map<SomeService>()
    .To<ISomeService>()
    .ResolveEveryRequest();
```
