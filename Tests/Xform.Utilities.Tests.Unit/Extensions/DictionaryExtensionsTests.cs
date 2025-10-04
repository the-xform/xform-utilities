using System.Collections;
using global::XForm.Utilities.Extensions;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class DictionaryExtensionsTests
{
	[Fact]
	public void GetIndexOfDictionaryKey_ReturnsCorrectIndex_WhenKeyExists()
	{
		var dict = new Dictionary<string, int>
			{
				{ "A", 1 },
				{ "B", 2 },
				{ "C", 3 }
			};

		var index = dict.GetIndexOfDictionaryKey("B");

		Assert.Equal(1, index);
	}

	[Fact]
	public void GetIndexOfDictionaryKey_ReturnsMinusOne_WhenKeyNotFound()
	{
		var dict = new Dictionary<int, string>
			{
				{ 10, "Ten" },
				{ 20, "Twenty" }
			};

		var index = dict.GetIndexOfDictionaryKey(30);

		Assert.Equal(-1, index);
	}

	// ---------- SafeAdd ----------
	[Fact]
	public void SafeAdd_AddsNewKey_ReturnsTrue()
	{
		var dict = new Dictionary<string, int>();

		var result = dict.SafeAdd("A", 100);

		Assert.True(result);
		Assert.Equal(100, dict["A"]);
	}

	[Fact]
	public void SafeAdd_ReplacesValue_WhenKeyExists_ReturnsFalse()
	{
		var dict = new Dictionary<string, int> { { "X", 1 } };

		var result = dict.SafeAdd("X", 999);

		Assert.False(result);
		Assert.Equal(999, dict["X"]);
	}

	[Fact]
	public void SafeAdd_DoesNotReplaceValue_WhenOverrideDisabled()
	{
		var dict = new Dictionary<string, int> { { "X", 1 } };

		var result = dict.SafeAdd("X", 999, overrideExistingValue: false);

		Assert.False(result);
		Assert.Equal(1, dict["X"]); // value unchanged
	}

	// ---------- SafeGet<TKey,TValue> ----------
	[Fact]
	public void SafeGet_Generic_ReturnsValue_WhenKeyExists()
	{
		var dict = new Dictionary<string, int>
			{
				{ "A", 42 }
			};

		var result = dict.SafeGet("A", -1);

		Assert.Equal(42, result);
	}

	[Fact]
	public void SafeGet_Generic_ReturnsDefault_WhenKeyDoesNotExist()
	{
		var dict = new Dictionary<string, int>();

		var result = dict.SafeGet("MissingKey", -1);

		Assert.Equal(-1, result);
	}

	[Fact]
	public void SafeGet_Generic_ReturnsDefault_WhenDictionaryIsNull()
	{
		Dictionary<string, int>? dict = null!; // Forced suppression of warining for negative test.

		var result = dict.SafeGet("X", -1);

		Assert.Equal(-1, result);
	}

	// ---------- SafeGet (non-generic IDictionary) ----------
	[Fact]
	public void SafeGet_NonGeneric_ReturnsConvertedValue_WhenKeyExists()
	{
		IDictionary dict = new Hashtable
			{
				{ "X", "123" }
			};

		var result = dict.SafeGet<int>("X");

		Assert.Equal(123, result);
	}

	[Fact]
	public void SafeGet_NonGeneric_ReturnsDefault_WhenKeyDoesNotExist()
	{
		IDictionary dict = new Hashtable();

		var result = dict.SafeGet<int>("Y", -99);

		Assert.Equal(-99, result);
	}

	[Fact]
	public void SafeGet_NonGeneric_ReturnsDefault_WhenValueIsNull()
	{
		IDictionary dict = new Hashtable
			{
				{ "NullKey", null }
			};

		var result = dict.SafeGet<int>("NullKey", -5);

		Assert.Equal(-5, result);
	}

	// ---------- RenameKey ----------
	[Fact]
	public void RenameKey_RenamesKeySuccessfully()
	{
		var dict = new Dictionary<string, string>
			{
				{ "OldKey", "Value" }
			};

		dict.RenameKey("OldKey", "NewKey");

		Assert.False(dict.ContainsKey("OldKey"));
		Assert.True(dict.ContainsKey("NewKey"));
		Assert.Equal("Value", dict["NewKey"]);
	}
}
