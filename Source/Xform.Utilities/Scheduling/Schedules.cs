// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Text.Json.Serialization;
using Xform.Utilities.Scheduling;

/// <summary>
/// Schedule interface to be implemented by any scheduling class.
/// </summary>
public interface ISchedule
{
	/// <summary>
	/// The type of schedule.
	/// </summary>
	ScheduleType Type { get; }

	/// <summary>
	/// When the schedule starts.
	/// </summary>
	DateTime ScheduleStart { get; }

	/// <summary>
	/// When the schedule stops.
	/// </summary>
	DateTime ScheduleStop { get; }

	/// <summary>
	/// Start time of day for the schedule.
	/// </summary>
	TimeSpan TimeStart { get; }
}

/// <summary>
/// Base class for all types of schedules.
/// </summary>
public abstract class ScheduleBase : ISchedule
{
	/// <summary>
	/// The type of schedule.
	/// </summary>
	public ScheduleType Type { get; init; }

	/// <summary>
	/// Date when the schedule starts.
	/// </summary>
	public DateTime ScheduleStart { get; set; } = DateTime.MinValue;

	/// <summary>
	/// Date when the schedule ends.
	/// </summary>
	public DateTime ScheduleStop { get; set; } = DateTime.MaxValue;

	/// <summary>
	/// Time of day when the schedule starts.
	/// </summary>
	public TimeSpan TimeStart { get; set; } = TimeSpan.Zero;
}

/// <summary>
/// Use this schedule when the job needs to fire only once and never again.
/// </summary>
public class OneShotSchedule : ScheduleBase
{
	public OneShotSchedule() => Type = ScheduleType.OneShot;
}

/// <summary>
/// Use this schedule when the job needs to fire periodically at specified TimeSpan intervals.
/// </summary>
public class IntervalSchedule : ScheduleBase
{
	public IntervalSchedule() => Type = ScheduleType.Interval;

	public TimeSpan Duration { get; set; }
}

/// <summary>
/// Use this schedule when the job needs to fire daily at specified time(s).
/// </summary>
public class DailySchedule : ScheduleBase
{
	public DailySchedule() => Type = ScheduleType.Daily;

	public int Interval { get; set; } // 1 is every day, 2 is every other day, etc.

	/// <summary>
	    /// All the (extra) times of the day, ensuring TimeStart is merged into the list and ensuring any duplicates are purged.
	    /// </summary>
	public IList<TimeSpan> TimesOfTheDay
	{
		get
		{
			var times_of_the_day = new List<TimeSpan> { TimeStart };
			return times_of_the_day.Union(_timesOfTheDay).OrderBy(t => t).ToList();
		}
		set => _timesOfTheDay = value;
	}

	private IList<TimeSpan> _timesOfTheDay = new List<TimeSpan>();
}

/// <summary>
/// Use this schedule when the job needs to fire weekly on specified days of week, and at specified start time.
/// </summary>
public class WeeklySchedule : ScheduleBase
{
	public WeeklySchedule() => Type = ScheduleType.Weekly;

	public int Interval { get; set; } = 1; // 1 is every week, 2 is every other week, etc.

	public DaysOfWeek DaysOfTheWeek { get; set; } = ScheduleValues.EveryDay;
}

/// <summary>
/// use this schedule when the job needs to fire in specified months, on the specified day of month, and at specified start time.
/// </summary>
public class MonthlySchedule : ScheduleBase
{
	public MonthlySchedule() => Type = ScheduleType.Monthly;

	public MonthsOfYear MonthsOfTheYear { get; set; } = ScheduleValues.AllYearRound;
	public int DayNumber { get; set; } // 1 - 31
	public DayOfMonth OnThe { get; set; } = DayOfMonth.Unknown; // First | Second | Third | Fourth | Last
	public DaysOfWeek DayOfTheWeek { get; set; } = DaysOfWeek.Unknown; // Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday

	[JsonIgnore]
	public bool UseDayNumber => DayNumber is >= 1 and <= 31;

	public int CalculateDayNumberInMonth(int year, int month)
	{
		var target_month = new DateTime(year, month, 1);
		var days_in_target_month = DateTime.DaysInMonth(year, month);

		// Explicitly pre-chosen to numeric value ... just ensure choice is clamped to last day if needed.
		if (UseDayNumber)
		{
			return DayNumber > days_in_target_month ? days_in_target_month : DayNumber;
		}

		var first_day = 0;
		var second_day = 0;
		var third_day = 0;
		var fourth_day = 0;
		var last_day = 0;

		for (var i = 0; i < days_in_target_month; i++)
		{
			var day_of_the_week = (int)target_month.AddDays(i).DayOfWeek;

			// TODO ... doesn't really work clearly if DayOfTheWeek or'ed together with multiple days.
			if ((((int)DayOfTheWeek) & (1 << day_of_the_week)) == 0) continue;

			if (i < 7) first_day = i + 1;
			else if (i < 14) second_day = i + 1;
			else if (i < 21) third_day = i + 1;
			else if (i < 28) last_day = fourth_day = i + 1;
			else if (i < 31) last_day = i + 1;
		}

		return OnThe switch
		{
			DayOfMonth.First => first_day,
			DayOfMonth.Second => second_day,
			DayOfMonth.Third => third_day,
			DayOfMonth.Fourth => fourth_day,
			DayOfMonth.Last => last_day,
			_ => throw new ArgumentException("Monthly schedule has unknown 'OnThe' DayOfMonth value.")
		};
	}
}
