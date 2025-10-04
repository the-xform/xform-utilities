using global::XForm.Utilities.Extensions;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class EnumerableExtensionsTests
{
	private class Person
	{
		public string Name { get; set; } = string.Empty;
		public int Age { get; set; }
	}

	private string GetTempFilePath(string fileName)
	{
		var dir = Path.Combine(Path.GetTempPath(), "CsvTests_" + Guid.NewGuid());
		Directory.CreateDirectory(dir);
		return Path.Combine(dir, fileName);
	}

	[Fact]
	public void CreateCSVFromGenericList_ReturnsFalse_WhenListIsNull()
	{
		List<Person?>? list = null; // Forced suppression of warining for negative test.

		var result = list.CreateCSVFromGenericList("test.csv");

		Assert.False(result);
	}

	[Fact]
	public void CreateCSVFromGenericList_ReturnsFalse_WhenListContainsOnlyNulls()
	{
		var list = new List<Person?> { null, null };

		var result = list.CreateCSVFromGenericList("test.csv");

		Assert.False(result);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	[InlineData("InvalidFileNameOnly.csv")] // no directory
	public void CreateCSVFromGenericList_Throws_WhenPathInvalid(string path)
	{
		var list = new List<Person?> { new Person { Name = "Alice", Age = 30 } };

		Assert.Throws<ArgumentException>(() => list.CreateCSVFromGenericList(path));
	}

	[Fact]
	public void CreateCSVFromGenericList_CreatesCsvFile_WhenValidData()
	{
		var list = new List<Person?>
			{
				new Person { Name = "Alice", Age = 30 },
				new Person { Name = "Bob", Age = 40 }
			};

		var tempPath = GetTempFilePath($"people.csv");

		var result = list.CreateCSVFromGenericList(tempPath);

		Assert.True(result);
		Assert.True(File.Exists(tempPath));

		var lines = File.ReadAllLines(tempPath);

		// Header
		Assert.Contains("Name,Age", lines[0]);

		// First row
		Assert.Contains("Alice,30", lines[1]);

		// Second row
		Assert.Contains("Bob,40", lines[2]);

		if(File.Exists(tempPath))
		{
			File.Delete(tempPath);
		}
	}

	[Fact]
	public void CreateCSVFromGenericList_CreatesDirectory_IfNotExists()
	{
		var list = new List<Person?>
			{
				new Person { Name = "Charlie", Age = 25 }
			};

		var dir = Path.Combine(Path.GetTempPath(), "NonExistent_" + Guid.NewGuid());
		var path = Path.Combine(dir, "people.csv");

		var result = list.CreateCSVFromGenericList(path);

		Assert.True(result);
		Assert.True(Directory.Exists(dir));
		Assert.True(File.Exists(path));
	}
}

