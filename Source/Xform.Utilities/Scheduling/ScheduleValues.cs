// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

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