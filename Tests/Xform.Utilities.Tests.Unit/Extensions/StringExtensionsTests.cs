using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using global::XForm.Utilities.Extensions;
using Xunit;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class StringExtensionsTests
{
	#region RemoveSpecialCharacters Tests

	[Theory]
	[InlineData("abc123", "abc123")]
	[InlineData("abc@123!", "abc123")]
	[InlineData("hello_world.test", "hello_world.test")]
	[InlineData("remove$#special*chars!", "removespecialchars")]
	[InlineData("replace$#chars", "-", "replace-chars")]
	public void RemoveSpecialCharacters_RemovesOrReplacesCorrectly(string input, string replaceWith = "", string? expected = null)
	{
		expected ??= replaceWith == "" ? Regex.Replace(input, "[^a-zA-Z0-9_.]+", "") : Regex.Replace(input, "[^a-zA-Z0-9_.]+", replaceWith);

		var result = input.RemoveSpecialCharacters(replaceWith);

		Assert.Equal(expected, result);
	}

	#endregion

	#region GetNonEmptyValuesAsList<T> Tests

	[Fact]
	public void GetNonEmptyValuesAsList_ReturnsEmpty_WhenInputIsNullOrWhitespace()
	{
		string? input1 = null;
		string input2 = "   ";

		var result1 = input1.GetNonEmptyValuesAsList<int>();
		var result2 = input2.GetNonEmptyValuesAsList<int>();

		Assert.Empty(result1);
		Assert.Empty(result2);
	}

	[Fact]
	public void GetNonEmptyValuesAsList_UsesDefaultDelimiters_WhenNoneProvided()
	{
		string input = "1,2 3;4|5";

		var result = input.GetNonEmptyValuesAsList<int>().ToList();

		Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, result);
	}

	[Fact]
	public void GetNonEmptyValuesAsList_UsesCustomDelimiters()
	{
		string input = "1-2-3";
		char[] delimiters = new char[] { '-' };

		var result = input.GetNonEmptyValuesAsList<int>(delimiters).ToList();

		Assert.Equal(new List<int> { 1, 2, 3 }, result);
	}

	[Fact]
	public void GetNonEmptyValuesAsList_FiltersEmptyOrNullEntries()
	{
		string input = "1,, ,2,,3";

		var result = input.GetNonEmptyValuesAsList<int>().ToList();

		Assert.Equal(new List<int> { 1, 2, 3 }, result);
	}

	#endregion

	#region ContainsString Tests

	[Theory]
	[InlineData("Hello World", "hello", true)]
	[InlineData("Hello World", "WORLD", true)]
	[InlineData("Hello World", "test", false)]
	[InlineData("Hello World", "HELLO", true)]
	public void ContainsString_PerformsCaseInsensitiveSearch_ByDefault(string source, string toCheck, bool expected)
	{
		var result = source.ContainsString(toCheck);
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData("Hello World", "hello", StringComparison.Ordinal, false)]
	[InlineData("Hello World", "Hello", StringComparison.Ordinal, true)]
	public void ContainsString_RespectsStringComparison(string source, string toCheck, StringComparison comparison, bool expected)
	{
		var result = source.ContainsString(toCheck, comparison);
		Assert.Equal(expected, result);
	}

	[Fact]
	public void ContainsString_ReturnsFalse_WhenSourceIsNull()
	{
		string? source = null!; // Forced suppression of warining for negative test.
		bool result = source.ContainsString("test");
		Assert.False(result);
	}

	#endregion
}
