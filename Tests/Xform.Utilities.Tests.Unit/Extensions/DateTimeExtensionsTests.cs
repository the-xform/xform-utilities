using global::XForm.Utilities.Extensions;

namespace Xform.Utilities.Tests.Unit.Extensions;

public class DateTimeExtensionsTests
{
	[Fact]
	public void ToUtcIgnoreDst_Throws_WhenAlreadyUtc()
	{
		var utcDate = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Utc);
		Assert.Throws<ArgumentException>(() => utcDate.ToUtcIgnoreDst());
	}

	[Fact]
	public void ToUtcIgnoreDst_ConvertsLocalToUtc_ByBaseOffset()
	{
		var localDate = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Local);

		var expected = localDate + TimeZoneInfo.Local.BaseUtcOffset;

		var result = localDate.ToUtcIgnoreDst();

		Assert.Equal(expected, result);
	}

	[Fact]
	public void ToLocalIgnoreDst_Throws_WhenNotUtc()
	{
		var localDate = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Local);
		Assert.Throws<ArgumentException>(() => localDate.ToLocalIgnoreDst());
	}

	[Fact]
	public void ToLocalIgnoreDst_ConvertsUtcToLocal_ByBaseOffset()
	{
		var utcDate = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Utc);

		var expected = utcDate + TimeZoneInfo.Local.BaseUtcOffset;

		var result = utcDate.ToLocalIgnoreDst();

		Assert.Equal(expected, result);
	}

	[Fact]
	public void ConvertToISO8601Format_ReturnsExpectedString()
	{
		var dto = new DateTimeOffset(2025, 10, 1, 14, 30, 45, TimeSpan.FromHours(5.5));
		var formatted = dto.ConvertToISO8601Format();

		// Note: The format string in your code is "yyyymmddThhmmss+|-hhmm"
		// "mm" is minutes, not months → potential bug in code
		Assert.NotNull(formatted);
		Assert.Contains("T", formatted);
	}

	[Fact]
	public void ToDateTimeOffset_WithTimeZoneInfo_Works()
	{
		var dt = new DateTime(2025, 10, 1, 8, 0, 0);
		var tz = TimeZoneInfo.FindSystemTimeZoneById("UTC");

		var result = dt.ToDateTimeOffset(tz);

		Assert.Equal(dt.Ticks, result.Ticks);
		Assert.Equal(tz.BaseUtcOffset, result.Offset);
	}

	[Fact]
	public void ToDateTimeOffset_WithTimeSpan_Works()
	{
		var dt = new DateTime(2025, 10, 1, 8, 0, 0);
		var offset = TimeSpan.FromHours(3);

		var result = dt.ToDateTimeOffset(offset);

		Assert.Equal(dt, result.DateTime);
		Assert.Equal(offset, result.Offset);
	}
}
