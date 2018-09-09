# Cave Tester

Cave Tester is a lightweight testing library that help you manage your database and generate mock data.

## Features

* Automatic database save and restore
* Determinist id generator
* Rules sets for object generation with [Bogus](https://github.com/bchavez/Bogus)

## Planned features

* Database pooling for easier parallel testing
* Some methods to test entity <=> dto mapping

## Quickstart

```C#
public class FooTests : Tester
{
	private readonly DbContext _dbContext;

	public FooTests()
	{
		// init your context here

		// Create a snapshot of the database before the test begin
		SaveHandler.Add(new SqlServerDbSnapshot(_dbContext.Database));
	}

	[Fact]
	public async Task ShouldFoo()
	{
		// Generate 5 Person with a random name
		await _dbContext.Generate<Person>(5, (faker, person) => person.Name = faker.Lorem.Word())
				 .SaveChangesAsync();

		// method to test
		// assertions

		// The database will automaticly be restored in a clean state from the snapeshot, wich will then be deleted
		// This happen even if the test fails
	}
}

public class Person
{
	public string Name { get; set; }
}
```

## Example

See a complete example [here](https://github.com/asagues/CaveTester/tree/master/CaveTester.Example)
