# Grenadiers.NET
Grenadiers.NET is a lightweight library to help fighting
[null dereferencing](https://en.wikipedia.org/wiki/Null_pointer#Null_dereferencing).

## Guard
The most straightforward way of tackling null dereferencing by defining a guard
method, that throws once the premise is not met, and otherwise returns.

Combined with static code analysis tools (such as [Sonar](https://nuget.org/packages/SonarAnalyzer.CSharp))
you will spot all potential null dereferences upfront, and get
`ArgumentNullException`'s runtime.

### Example
```csharp

public class Processor
{
    public Processor(ConfigService config, IIdentity identity)
    {
        this.Config = Guard.NotNull(config, nameof(config));
        this.Identity = Guard.NotNull(identity, nameof(identity));
    }

    public Result Process(Guid? id, ojbect inputData, int records)
    {
        Guard.NotEmpty(id, nameof(id));
        Guard.Positive(records, nameof(records));

        ProccessorData data = Guard.IsInstanceOf<ProccessorData>(inputData, nameof(inputData));

        // ...
    }
}
```

### Using guidelines
* Include the file in you project (an extra dependency is not worth it)
* Change the namespace to maximum shared namespace amongst the using projects
* Keep it internal and use [assembly: InternalsVisibleTo] to open up access
* Add specific Guard methods if you software needs them.
* Keep the checks cheep so that you also can run them in production code.

## Null object pattern
On way to tackle null dereferencing, is by using the [null object pattern](https://en.wikipedia.org/wiki/Null_object_pattern);
instead of returning null, return a null object that defined neutral behavior.
Simple examples are `string.Empty` and `Array.Empty<T>`.

To extend on this, and make it more consistent throughout your code base, it
might help to use this `Nil` helper class (the name is chosen to prevent
collision with null).

``` C#
var x = GetResult() ?? Nil.Object<MyType>();
```

Or, depending on you coding preferences:

``` C#
var x = GetResult().OrNilObject();
```

Null Objects are resolved based on the convention that static readonly
fields/properties/methods returning an assignable type with the name
None, Nil, Empty or Default, will do the trick. If such a factory is
not found, an `NilObjectUnavailable` exception is thrown; this
should obviously never occur runtime, but in case it does, the message
is clear. To prevent this, you can register custom NulObject Factories
for your type of choice.
