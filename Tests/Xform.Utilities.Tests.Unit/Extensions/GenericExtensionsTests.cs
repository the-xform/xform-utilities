using XForm.Utilities.Extensions;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class GenericExtensionsTests
{
	private enum TestEnum
	{
		First,
		Second
	}

	#region Null Tests
	[Fact]
	public void ConvertTo_ReturnsDefault_WhenValueIsNull()
	{
		object? input = null;
		var result = input.ConvertTo<int>();
		Assert.Equal(0, result); // default(int)
	}
	#endregion

	#region Primitive Type Conversion
	[Fact]
	public void ConvertTo_ConvertsStringToInt()
	{
		object input = "42";
		var result = input.ConvertTo<int>();
		Assert.Equal(42, result);
	}

	[Fact]
	public void ConvertTo_ConvertsIntToDouble()
	{
		object input = 10;
		var result = input.ConvertTo<double>();
		Assert.Equal(10.0, result);
	}
	#endregion

	#region Enum Conversion
	[Fact]
	public void ConvertTo_Enum_Success_WhenValidString()
	{
		object input = "First";
		var result = input.ConvertTo<TestEnum>();
		Assert.Equal(TestEnum.First, result);
	}

	[Fact]
	public void ConvertTo_Enum_Throws_WhenNotString()
	{
		object input = 1;
		Assert.Throws<ArgumentException>(() => input.ConvertTo<TestEnum>());
	}

	[Fact]
	public void ConvertTo_Enum_Throws_WhenInvalidValue()
	{
		object input = "Invalid";
		Assert.Throws<ArgumentException>(() => input.ConvertTo<TestEnum>());
	}
	#endregion

	#region Boolean Conversion
	[Fact]
	public void ConvertTo_Bool_FromString()
	{
		object input = "true";
		var result = input.ConvertTo<bool>();
		Assert.True(result);
	}

	[Fact]
	public void ConvertTo_Bool_FromBool()
	{
		object input = false;
		var result = input.ConvertTo<bool>();
		Assert.False(result);
	}

	[Fact]
	public void ConvertTo_Bool_Throws_InvalidType()
	{
		object input = 123;
		Assert.Throws<ArgumentException>(() => input.ConvertTo<bool>());
	}
	#endregion

	#region TimeSpan Conversion
	[Fact]
	public void ConvertTo_TimeSpan_FromString()
	{
		object input = "01:30:00";
		var result = input.ConvertTo<TimeSpan>();
		Assert.Equal(TimeSpan.FromHours(1.5), result);
	}

	[Fact]
	public void ConvertTo_TimeSpan_FromTimeSpan()
	{
		object input = TimeSpan.FromMinutes(45);
		var result = input.ConvertTo<TimeSpan>();
		Assert.Equal(TimeSpan.FromMinutes(45), result);
	}

	[Fact]
	public void ConvertTo_TimeSpan_Throws_InvalidType()
	{
		object input = 123;
		Assert.Throws<ArgumentException>(() => input.ConvertTo<TimeSpan>());
	}
	#endregion

	#region Guid Conversion
	[Fact]
	public void ConvertTo_Guid_FromValidString()
	{
		var guid = Guid.NewGuid();
		object input = guid.ToString();
		var result = input.ConvertTo<Guid>();
		Assert.Equal(guid, result);
	}

	[Fact]
	public void ConvertTo_Guid_Throws_InvalidString()
	{
		object input = "NotAGuid";
		Assert.Throws<ArgumentException>(() => input.ConvertTo<Guid>());
	}

	[Fact]
	public void ConvertTo_Guid_FromGuidInstance()
	{
		var guid = Guid.NewGuid();
		object input = guid;
		var result = input.ConvertTo<Guid>();
		Assert.Equal(guid, result);
	}
	#endregion

	#region Other Type Conversion
	[Fact]
	public void ConvertTo_String_FromInt()
	{
		object input = 100;
		var result = input.ConvertTo<string>();
		Assert.Equal("100", result);
	}

	[Fact]
	public void ConvertTo_Double_FromString()
	{
		object input = "3.14";
		var result = input.ConvertTo<double>();
		Assert.Equal(3.14, result);
	}
	#endregion
}
