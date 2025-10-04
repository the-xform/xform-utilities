using System.Text.Json;
using System.Text.Json.Serialization;
using XForm.Utilities;

namespace Xform.Utilities.Tests.Unit;

public class JsonUtilitiesTests
{
	private readonly IJsonUtilities _jsonUtilities;

	public JsonUtilitiesTests()
	{
		_jsonUtilities = new JsonUtilities();
	}

	private class Person
	{
		public string Name { get; set; } = "";
		public int Age { get; set; }
		public Role Role { get; set; }
	}

	private enum Role
	{
		User,
		Admin
	}

	[Fact]
	public void DefaultOptions_ShouldUseRelaxedEscaping_And_EnumConverter()
	{
		var options = _jsonUtilities.DefaultOptions;

		Assert.True(options.WriteIndented);
		Assert.Contains(options.Converters, c => c is JsonStringEnumConverter);
		Assert.Equal(JsonIgnoreCondition.Never, options.DefaultIgnoreCondition);
	}

	[Fact]
	public void DefaultOptionsWithNoIndentation_ShouldHaveWriteIndentedFalse()
	{
		var options = _jsonUtilities.DefaultOptionsWithNoIndentation;

		Assert.False(options.WriteIndented);
		Assert.Contains(options.Converters, c => c is JsonStringEnumConverter);
	}

	[Fact]
	public void ToJson_WithIndented_ShouldProducePrettyJson()
	{
		var person = new Person { Name = "Alice", Age = 30, Role = Role.Admin };

		var json = _jsonUtilities.ToJson(person, indented: true);

		Assert.Contains(Environment.NewLine, json); // indented JSON has newlines
		Assert.Contains("Alice", json);
		Assert.Contains("Admin", json); // enum should serialize as string
	}

	[Fact]
	public void ToJson_WithNoIndent_ShouldProduceCompactJson()
	{
		var person = new Person { Name = "Bob", Age = 25, Role = Role.User };

		var json = _jsonUtilities.ToJson(person, indented: false);

		Assert.DoesNotContain(Environment.NewLine, json);
		Assert.Contains("Bob", json);
		Assert.Contains("User", json);
	}

	[Fact]
	public void FromJson_ShouldDeserializeCorrectly()
	{
		string json = "{\"Name\":\"Charlie\",\"Age\":40,\"Role\":\"Admin\"}";

		var person = _jsonUtilities.FromJson<Person>(json);

		Assert.NotNull(person);
		Assert.Equal("Charlie", person!.Name);
		Assert.Equal(40, person.Age);
		Assert.Equal(Role.Admin, person.Role);
	}

	[Fact]
	public void ConvertToJson_And_ConvertFromJson_ShouldRoundTrip()
	{
		var original = new Person { Name = "Diana", Age = 28, Role = Role.User };

		string json = JsonUtilities.ConvertToJson(original);
		var deserialized = JsonUtilities.ConvertFromJson<Person>(json);

		Assert.NotNull(deserialized);
		Assert.Equal(original.Name, deserialized!.Name);
		Assert.Equal(original.Age, deserialized.Age);
		Assert.Equal(original.Role, deserialized.Role);
	}

	[Fact]
	public void ConvertFromJson_InvalidJson_ShouldThrowJsonException()
	{
		string invalidJson = "{invalid-json}";

		Assert.Throws<JsonException>(() => JsonUtilities.ConvertFromJson<Person>(invalidJson));
	}
}
