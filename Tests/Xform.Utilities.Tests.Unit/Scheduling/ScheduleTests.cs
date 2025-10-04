using global::Xform.Utilities.Scheduling;

namespace Xform.Utilities.Tests.Unit.Scheduling;

public class ScheduleTests
{
	[Fact]
	public void BuildSchedule_ShouldReturnOneShotSchedule_WhenTypeIsOneShot()
	{
		var config = new ScheduleConfiguration
		{
			Type = ScheduleType.OneShot,
			OneShot = new OneShotSchedule()
		};
		var result = global::Schedule.Build(config);
		Assert.IsType<OneShotSchedule>(result);
	}

	[Fact]
	public void BuildSchedule_ShouldThrow_WhenConfigHasNoMatchingSchedule()
	{
		var config = new ScheduleConfiguration
		{
			Type = ScheduleType.Daily
		};
		Assert.Throws<ArgumentNullException>(() => global::Schedule.Build(config));
	}

	[Fact]
	public void CalculateOffsetToNextInterval_OneShot_ShouldReturnZeroOffset()
	{
		var schedule = new OneShotSchedule
		{
			ScheduleStart = DateTime.Today,
			TimeStart = TimeSpan.Zero
		};
		var now = DateTime.Today.AddMinutes(1);
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		Assert.True(isActive);
		Assert.Equal(TimeSpan.Zero, offset);
	}

	[Fact]
	public void CalculateOffsetToNextInterval_IntervalSchedule_ShouldReturnDuration()
	{
		var schedule = new IntervalSchedule
		{
			ScheduleStart = DateTime.Today,
			TimeStart = TimeSpan.Zero,
			Duration = TimeSpan.FromMinutes(30)
		};
		var now = DateTime.Today.AddMinutes(10);
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		Assert.True(isActive);
		Assert.Equal(TimeSpan.FromMinutes(30), offset);
	}

	[Fact]
	public void CalculateOffsetToNextInterval_DailySchedule_BeforeStart_ShouldReturnPositiveOffset()
	{
		var schedule = new DailySchedule
		{
			ScheduleStart = DateTime.Today,
			TimeStart = new TimeSpan(8, 0, 0),
			Interval = 1
		};
		var now = DateTime.Today.AddHours(6);
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		Assert.False(isActive);
		Assert.True(offset > TimeSpan.Zero);
	}

	[Fact]
	public void CalculateOffsetToNextInterval_WeeklySchedule_ShouldRespectIntervalAndDay()
	{
		var schedule = new WeeklySchedule
		{
			ScheduleStart = DateTime.Today,
			TimeStart = new TimeSpan(9, 0, 0),
			Interval = 1,
			DaysOfTheWeek = DaysOfWeek.Monday
		};
		var now = new DateTime(2025, 10, 3, 8, 0, 0); // Friday
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		Assert.True(offset > TimeSpan.Zero);
	}

	[Fact]
	public void CalculateOffsetToNextInterval_MonthlySchedule_ShouldCalculateCorrectDay()
	{
		var schedule = new MonthlySchedule
		{
			ScheduleStart = new DateTime(2025, 1, 1),
			TimeStart = new TimeSpan(10, 0, 0),
			DayNumber = 15
		};
		var now = new DateTime(2025, 1, 10, 9, 0, 0);
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		var expected = new DateTime(2025, 1, 15, 10, 0, 0) - now + TimeSpan.FromMilliseconds(1);
		Assert.Equal(expected, offset);
	}

	[Fact]
	public void CalculateOffsetToNextInterval_ShouldReturnNegative_WhenScheduleEnded()
	{
		var schedule = new OneShotSchedule
		{
			ScheduleStart = DateTime.Today.AddDays(-2),
			ScheduleStop = DateTime.Today.AddDays(-1)
		};
		var now = DateTime.Today;
		var (isActive, offset) = schedule.CalculateOffsetToNextInterval(now);
		Assert.False(isActive); Assert.True(offset < TimeSpan.Zero);
	}
}