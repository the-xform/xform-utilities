// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

#if SAMPLE

"Schedule":
{
	"Type": "ENUM: Interval | OneShot | Daily | Weekly | Monthly",
   	"DateScheduleStart": "Optional date when schedule starts. Empty implies now.",
   	"DateScheduleStop": "Optional date if schedule has a hard stop in the future.",
   	"Interval":
   	{
		"TimeStart": "00:00:00.000", // Initial start delay
		"Duration": "0.00:00:00.00" // ... then every X.
	},
	"OneShot":
	{
		"TimeStart": "00:00:00.000" // Initial start delay, then go.

	}
	"Daily":
	{
		"TimeStart": "00:00:00.000",
   		"Interval": "1 is every day, 2 is every other day, etc.",
   		"TimesOfTheDay": // Optional: extra times.
   		[
			"TimeStart": "00:00:00.000",
			"TimeStart": "00:00:00.000",
			"TimeStart": "00:00:00.000"
   			// ...more times here
   		]
	}
	"Weekly":
	{
		"TimeStart": "00:00:00.000",
		"Interval": "1 is every week, 2 is every other week, etc.",
		"DaysOfTheWeek": "Every day if empty. Comma separated otherwise. Like: 'Monday, Friday'"
	}
	"Monthly":
	{
		"TimeStart": "00:00:00.000",
		"MonthsOfTheYear": "Every month if empty. Comma separated otherwise.",
		"DayOfTheMonth": // Use 'DayNumber' alone or set it to 0 and use 'OnThe' and 'DayOfTheWeek' together.
		{
			"DayNumber": "1 - 31",
			"OnThe": "First | Second | Third | Fourth | Last",
			"DayOfTheWeek": "Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday"
		}
	}
}

#endif

using Xform.Utilities.Scheduling;

public class ScheduleConfiguration
{
	/// <summary>
	/// The type of schedule.
	/// </summary>
	public ScheduleType Type { get; set; }

	/// <summary>
	/// When does the schedule start? If not specified, it is considered to be DateTime.MinValue.
	/// </summary>
	public DateTime ScheduleStart { get; set; } = DateTime.MinValue;

	/// <summary>
	/// When does the schedule stop? If not specified, it is considered to be DateTime.MaxValue.
	/// </summary>
	public DateTime ScheduleStop { get; set; } = DateTime.MaxValue;

	/// <summary>
	/// What time of the day should the schedule start? If not specified, it is considered to be 00:00:00 (midnight).
	/// </summary>
	public TimeSpan TimeStart { get; set; } = TimeSpan.Zero;

	/// <summary>
	/// One shot schedule configuration. Should be set only if Type is OneShot.
	/// </summary>
	public OneShotSchedule? OneShot { get; set; }

	/// <summary>
	/// Interval schedule configuration. Should be set only if Type is Interval.
	/// </summary>
	public IntervalSchedule? Interval { get; set; }

	/// <summary>
	/// Daily schedule configuration. Should be set only if Type is Daily.
	/// </summary>
	public DailySchedule? Daily { get; set; }

	/// <summary>
	/// Weekly schedule configuration. Should be set only if Type is Weekly.
	/// </summary>
	public WeeklySchedule? Weekly { get; set; }

	/// <summary>
	/// Monthly schedule configuration. Should be set only if Type is Monthly.
	/// </summary>
	public MonthlySchedule? Monthly { get; set; }
}
