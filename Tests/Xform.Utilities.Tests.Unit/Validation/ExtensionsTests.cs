using global::XForm.Utilities.Validations;

namespace Xform.Utilities.Tests.Unit.Validation;

public class ExtensionsTests
{
	#region String

	[Theory]
	[InlineData("Hello", true)]
	[InlineData("", false)]
	[InlineData("   ", false)]
	[InlineData(null, false)]
	[InlineData("IGNORE", false)]
	public void HasSomething_String_Works(string? value, bool expected)
	{
		bool result = value.HasSomething(nameof(value), considerEmptyIf: "IGNORE");
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData("", true)]
	[InlineData("   ", true)]
	[InlineData("Nothing", true)]
	[InlineData("Something", false)]
	public void HasNothing_String_Works(string? value, bool expected)
	{
		bool result = value.HasNothing("Nothing");
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData("#FFFFFF", true)]
	[InlineData("#000", true)]
	[InlineData("red", true)]
	[InlineData("NotAColor", false)]
	public void IsColorCode_Works(string value, bool expected)
	{
		Assert.Equal(expected, value.IsColorCode());
	}

	#endregion

	#region Generic

	[Fact]
	public void IsNotNullOrDefault_Generic_ReturnsFalse_WhenNullOrDefault()
	{
		int? val1 = null;
		int? val2 = 0;

		Assert.False(val1.IsNotNullOrDefault("param"));
		Assert.False(val2.IsNotNullOrDefault("param"));
	}

	[Fact]
	public void IsNotNullOrDefault_Generic_ReturnsFalse_WhenConsiderDefaultIf()
	{
		int? val = 5;
		Assert.False(val.IsNotNullOrDefault("param", 5));
	}

	[Fact]
	public void IsNotNullOrDefault_Generic_ReturnsTrue_WhenValid()
	{
		int? val = 10;
		Assert.True(val.IsNotNullOrDefault("param"));
	}

	#endregion

	#region Guid

	[Fact]
	public void IsNotNullOrDefault_Guid_ReturnsFalse_WhenNullOrEmpty()
	{
		Guid? g1 = null;
		Guid? g2 = Guid.Empty;

		Assert.False(g1.IsNotNullOrDefault("param"));
		Assert.False(g2.IsNotNullOrDefault("param"));
	}

	[Fact]
	public void IsNotNullOrDefault_Guid_RespectsConsiderDefaultIfEmpty()
	{
		Guid? g = Guid.Empty;
		Assert.False(g.IsNotNullOrDefault("param", considerDefaultIfEmpty: true));
	}

	[Fact]
	public void IsNotNullOrDefault_Guid_ReturnsTrue_WhenValid()
	{
		Guid? g = Guid.NewGuid();
		Assert.True(g.IsNotNullOrDefault("param"));
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData("00000000-0000-0000-0000-000000000000", true)]
	public void HasNothing_Guid_Works(object? guidVal, bool expected)
	{
		Guid? g = guidVal == null ? null : Guid.Parse(guidVal.ToString()!);
		Assert.Equal(expected, g.HasNothing());
	}

	#endregion

	#region Numbers

	[Fact]
	public void IsNotNull_Int_Works()
	{
		int? n1 = null;
		int? n2 = 5;

		Assert.False(n1.IsNotNull("param"));
		Assert.True(n2.IsNotNull("param"));
	}

	[Fact]
	public void IsNotNullOrDefault_Int_Works()
	{
		int? n1 = null;
		int? n2 = 0;
		int? n3 = 5;

		Assert.False(n1.IsNotNullOrDefault("param"));
		Assert.False(n2.IsNotNullOrDefault("param"));
		Assert.True(n3.IsNotNullOrDefault("param"));
		Assert.False(n3.IsNotNullOrDefault("param", 5));
	}

	[Fact]
	public void IsNotNull_Decimal_Works()
	{
		decimal? d1 = null;
		decimal? d2 = 1.23m;

		Assert.False(d1.IsNotNull("param"));
		Assert.True(d2.IsNotNull("param"));
	}

	[Fact]
	public void IsNotNullOrDefault_Decimal_Works()
	{
		decimal? d1 = null;
		decimal? d2 = 0m;
		decimal? d3 = 5m;

		Assert.False(d1.IsNotNullOrDefault("param"));
		Assert.False(d2.IsNotNullOrDefault("param"));
		Assert.True(d3.IsNotNullOrDefault("param"));
		Assert.False(d3.IsNotNullOrDefault("param", 5m));
	}

	[Fact]
	public void IsNotNull_Double_Works()
	{
		double? d1 = null;
		double? d2 = 2.5;

		Assert.False(d1.IsNotNull("param"));
		Assert.True(d2.IsNotNull("param"));
	}

	[Fact]
	public void IsNotNullOrDefault_Double_Works()
	{
		double? d1 = null;
		double? d2 = 0d;
		double? d3 = 5d;

		Assert.False(d1.IsNotNullOrDefault("param"));
		Assert.False(d2.IsNotNullOrDefault("param"));
		Assert.True(d3.IsNotNullOrDefault("param"));
		Assert.False(d3.IsNotNullOrDefault("param", 5d));
	}

	#endregion

	#region Dictionary

	[Fact]
	public void HasNothing_Dictionary_Works()
	{
		Dictionary<string, int>? dict1 = null;
		var dict2 = new Dictionary<string, int>();
		var dict3 = new Dictionary<string, int> { ["a"] = 1 };

		Assert.True(dict1.HasNothing());
		Assert.True(dict2.HasNothing());
		Assert.False(dict3.HasNothing());
	}

	[Fact]
	public void HasSomething_Dictionary_Works()
	{
		Dictionary<string, int>? dict1 = null;
		var dict2 = new Dictionary<string, int>();
		var dict3 = new Dictionary<string, int> { ["a"] = 1 };

		Assert.False(dict1.HasSomething());
		Assert.False(dict2.HasSomething());
		Assert.True(dict3.HasSomething());
	}

	#endregion

	#region Boolean

	[Fact]
	public void HasSomething_Bool_Works()
	{
		bool? b1 = null;
		bool? b2 = true;
		bool? b3 = false;

		Assert.False(b1.HasSomething());
		Assert.True(b2.HasSomething());
		Assert.True(b3.HasSomething());
	}

	#endregion

	#region Enumerable

	[Fact]
	public void HasNothing_Enumerable_Works()
	{
		List<int>? list1 = null;
		var list2 = new List<int>();
		var list3 = new List<int> { 1, 2 };

		Assert.True(list1.HasNothing());
		Assert.True(list2.HasNothing());
		Assert.False(list3.HasNothing());
	}

	[Fact]
	public void HasSomething_Enumerable_Works()
	{
		List<int>? list1 = null;
		var list2 = new List<int>();
		var list3 = new List<int> { 1, 2 };

		Assert.True(list1.HasSomething());   // Null but returns true by design
		Assert.False(list2.HasSomething());
		Assert.True(list3.HasSomething());
	}

	#endregion
}