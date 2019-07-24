
### Boilerplate  (1.1.0)

This project is for programmers who want agility in building their API REST, 80% of the time we spend doing integrations and project structure, why not a boilerprate ready to use with implementations made, and with the time that you save you go to drink a beer or study another language or framework?

Wiki: https://srburton.github.io/dotnet.boilerplate/

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![License: MIT](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![License: MIT](https://img.shields.io/github/release/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](https://img.shields.io/github/tag-date/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](https://img.shields.io/github/languages/count/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](https://img.shields.io/github/last-commit/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](https://img.shields.io/github/languages/code-size/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](https://img.shields.io/github/issues-raw/srburton/dotnet.boilerplate.svg)]()
[![License: MIT](	https://img.shields.io/github/issues-closed/srburton/dotnet.boilerplate.svg)]()


##### Future migration from integrations to nuget packages

> Project: 
  dotnet.boilerplate.packs
    
##### dotnet --version
 - 2.1
 - 2.2
 - 3.0 (preview) 

##### Implementation
- Sendgrid
- RabbitMq 
- Redis
- Twilio 
- Sentry
- Aws (s3) 
- Docker
- Datadog ***(future)***
- Gateway Payment ***(future)***
- C.I ***(future)***

##### Architecture Features
- API (default)
- DDD (Domain Driven Design)
- IOC (Inversion of control).



#### Modeling approach

[![License: MIT](https://i.ibb.co/vwLp4X4/Untitled-Diagram-1.png)]()

#### IOC (Inversion of control)

###### Container register 

```cSharp

//IService alias (trasient)
public class Example1 : IService<Example1> { }

//IApplication alias (trasient)
public class Example2 : IApplication<Example2> { }

//IRepository alias (trasient)
public class Example3 : IRepository<Example3> { }

public class Example4 : IScoped<Example4> { }

public class Example5 : ISingleton<Example5> { }

public class Example6 : ITransient<Example6> { }

[Scoped]
public class Example7 { }

[Singleton]
public class Example8 { }

[Transient]
public class Example9 { }

```

###### Injection 

```cSharp

public class Example10
{
    readonly Example1 _example1;

    public Example10(IService<Example1> example1)
    {
        _example1 = (Example1)example1;
    }
}

```


