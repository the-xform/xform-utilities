using XForm.Utilities.Validations;

namespace Xform.Utilities.Tests.Unit.Validation;

public class XssertTests
{
	[Fact]
	public void Equal_ShouldNotThrow_WhenValuesAreEqual()
	{
		Xssert.Equal(5, 5, "param", "target");
	}

	[Fact]
	public void Equal_ShouldThrow_WhenValuesAreNotEqual()
	{
		Assert.Throws<ArgumentException>(() =>
			Xssert.Equal(5, 10, "param", "target"));
	}

	[Fact]
	public void NotEqual_ShouldNotThrow_WhenValuesAreNotEqual()
	{
		Xssert.NotEqual(5, 10, "param", "target");
	}

	[Fact]
	public void NotEqual_ShouldThrow_WhenValuesAreEqual()
	{
		Assert.Throws<ArgumentException>(() =>
			Xssert.NotEqual("abc", "abc", "param", "target"));
	}

	[Fact]
	public void GreaterThan_ShouldNotThrow_WhenValueIsGreater()
	{
		Xssert.GreaterThan(10, 5, "param", "target");
	}

	[Fact]
	public void GreaterThan_ShouldThrow_WhenValueIsLessOrEqual()
	{
		Assert.Throws<ArgumentException>(() =>
			Xssert.GreaterThan(5, 10, "param", "target"));

		Assert.Throws<ArgumentException>(() =>
			Xssert.GreaterThan(5, 5, "param", "target"));
	}

	[Fact]
	public void LessThan_ShouldNotThrow_WhenValueIsLess()
	{
		Xssert.LessThan(5, 10, "param", "target");
	}

	[Fact]
	public void LessThan_ShouldThrow_WhenValueIsGreaterOrEqual()
	{
		Assert.Throws<Exception>(() =>
			Xssert.LessThan(10, 5, "param", "target"));

		Assert.Throws<Exception>(() =>
			Xssert.LessThan(5, 5, "param", "target"));
	}

	[Fact]
	public void IsNotNull_ShouldNotThrow_WhenValueIsNotNull()
	{
		var obj = new object();
		Xssert.IsNotNull(obj, "param");
	}

	[Fact]
	public void IsNotNull_ShouldThrow_WhenValueIsNull()
	{
		object? obj = null;
		Assert.Throws<ArgumentNullException>(() =>
			Xssert.IsNotNull(obj, "param"));
	}

	[Fact]
	public void IsNotNullOrEmpty_ShouldNotThrow_WhenCollectionHasItems()
	{
		var list = new List<int> { 1, 2, 3 };
		Xssert.IsNotNullOrEmpty(list, "param");
	}

	[Fact]
	public void IsNotNullOrEmpty_ShouldThrow_WhenCollectionIsNull()
	{
		List<int>? list = null;
		Assert.Throws<ArgumentNullException>(() =>
			Xssert.IsNotNullOrEmpty(list!, "param"));
	}

	[Fact]
	public void IsNotNullOrEmpty_ShouldThrow_WhenCollectionIsEmpty()
	{
		var list = new List<int>();
		Assert.Throws<ArgumentNullException>(() =>
			Xssert.IsNotNullOrEmpty(list, "param"));
	}

	[Fact]
	public void ContainsKey_ShouldNotThrow_WhenKeyExists()
	{
		var dict = new Dictionary<string, int> { { "key", 1 } };
		Xssert.ContainsKey(dict, "key", "param", "dict");
	}

	[Fact]
	public void ContainsKey_ShouldThrow_WhenKeyDoesNotExist()
	{
		var dict = new Dictionary<string, int> { { "key", 1 } };
		Assert.Throws<KeyNotFoundException>(() =>
			Xssert.ContainsKey(dict, "missing", "param", "dict"));
	}

	[Fact]
	public void Contains_ShouldNotThrow_WhenValueExists()
	{
		var list = new List<int> { 1, 2, 3 };
		Xssert.Contains(list, 2, "param", "list");
	}

	[Fact]
	public void Contains_ShouldThrow_WhenValueDoesNotExist()
	{
		var list = new List<int> { 1, 2, 3 };
		Assert.Throws<KeyNotFoundException>(() =>
			Xssert.Contains(list, 99, "param", "list"));
	}
}

