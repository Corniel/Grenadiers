# Grenadiers.NET
## Defending your pre-conditions

Grenadiers.NET is a lightweight static class that provides generic Guard methods
that allow to write robust code.

## Example
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

## Using guidelines
* Change the namespace to maximum shared namespace amongst the using projects
* Keep it internal and use [assembly: InternalsVisibleTo] to open up access
* Add specific Guard methods if you software needs them.
* Keep the checks cheep so that you also can run them in production code.
