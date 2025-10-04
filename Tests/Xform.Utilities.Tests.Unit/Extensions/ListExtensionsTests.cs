using global::XForm.Utilities.Extensions;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class ListExtensionsTests
{
	// ---------- String tests ----------
	[Fact]
	public void AddIfNotNullOrEmpty_AddsNonEmptyString()
	{
		var list = new List<string>();

		list.AddIfNotNullOrEmpty("Hello");

		Assert.Single(list);
		Assert.Equal("Hello", list[0]);
	}

	[Fact]
	public void AddIfNotNullOrEmpty_DoesNotAddEmptyString()
	{
		var list = new List<string>();

		list.AddIfNotNullOrEmpty("");

		Assert.Empty(list);
	}

	[Fact]
	public void AddIfNotNullOrEmpty_DoesNotAddNullString()
	{
		var list = new List<string?>();

		list.AddIfNotNullOrEmpty(null);

		Assert.Empty(list);
	}

	// ---------- Non-string tests ----------
	[Fact]
	public void AddIfNotNullOrEmpty_AddsNonNullObject()
	{
		var list = new List<int?>();

		list.AddIfNotNullOrEmpty(123);

		Assert.Single(list);
		Assert.Equal(123, list[0]);
	}

	[Fact]
	public void AddIfNotNullOrEmpty_DoesNotAddNullObject()
	{
		var list = new List<object?>();

		list.AddIfNotNullOrEmpty(null);

		Assert.Empty(list);
	}

	// ---------- Null list handling ----------
	[Fact]
	public void AddIfNotNullOrEmpty_DoesNothing_WhenListIsNull()
	{
		List<string>? list = null;

		// Should not throw
		list.AddIfNotNullOrEmpty("Hello");

		Assert.Null(list);
	}
}

