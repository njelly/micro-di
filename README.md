## Micro-DI
A micro dependency injection framework for .net C#.

### License
MIT License

### Bind and Resolve

```csharp
var container = new Container();
container.Bind<ISomeType, SomeType>();

var instance = container.Resolve<ISomeType>();
```

### Bind and Resolve With Arguments
You can pass additional arguments to the constructor via ResolveWith.

```csharp
public class SomeType : ISomeType
{
	public string Text { get; set; }
	
    SomeType(string text) { Text = text; }
}

var container = new Container();
container.Bind<ISomeType, SomeType>();

// no tag supplied `tag`...
var instance = container.ResolveWith<ISomeType>(null, "Hello World");

Assert.AreEqual(instance.Text, "Hello World");
```
### Tags
Binding multiple variations of the same key type with tags.

```csharp
var container = new  Container();
container.Bind<ISomeType, SomeTypeA>().WithTag("a");
container.Bind<ISomeType, SomeTypeB>().WithTag("b");

var instanceA = container.Resolve<ISomeType>("a");
var instanceB = container.ResolveWith<ISomeType>("b", "Hello World");
```

### Singletons 
You can bind a type as a 'singleton' so that it will persist.

```csharp
var container = new Container();
container.Bind<ISomeType, SomeType>().AsSingleton();

var instance1 = container.Resolve<ISomeType>();
var instance2 = container.Resolve<ISomeType>();

Assert.AreEqual(instance1, instance2);
```

### Inject Attributes
If you can't inject via a constructor, use [Inject] attributes to inject into Fields or Properties.

```csharp
class SomeType : ISomeType
{
    [Inject] private IOtherType _otherType;
}
```

### WithFactory for CustomConstructor
Use WithFactory to pass a factory delegate to be used when creating a new instance.

```csharp
var container = new Container();
container.Bind<ISomeType, SomeType>().WithFactory(delegate(IContainer container, IBinding binding, object[] args)
{
    return new SomeType();
});

var instanceA = container.Resolve<ISomeType>();
var instanceB = container.ResolveWith<ISomeType>(null, "Hello World");
```
