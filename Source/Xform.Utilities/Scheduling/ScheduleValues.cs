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

namespace Xform.Utilities.Scheduling;

/// <summary>
/// Useful predefined schedule values that can be used to set up schedules quickly.
/// </summary>
public static class ScheduleValues
{
	/// <summary>
	/// All seven days of the week OR'ed together.
	/// </summary>
	public static readonly DaysOfWeek EveryDay = (DaysOfWeek.Sunday | DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday);

	/// <summary>
	/// Monday through Friday OR'ed together.
	/// </summary>
	public static readonly DaysOfWeek WeekDays = (DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday);

	/// <summary>
	/// Saturday and Sunday OR'ed together.
	/// </summary>
	public static readonly DaysOfWeek WeekEnds = (DaysOfWeek.Sunday | DaysOfWeek.Saturday);

	/// <summary>
	/// All the months of the year OR'ed together.
	/// </summary>
	public static readonly MonthsOfYear AllYearRound = MonthsOfYear.January | MonthsOfYear.February | MonthsOfYear.March | MonthsOfYear.April | MonthsOfYear.May | MonthsOfYear.June | MonthsOfYear.July | MonthsOfYear.August | MonthsOfYear.September | MonthsOfYear.October | MonthsOfYear.November | MonthsOfYear.December;
}