// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

namespace Xform.Utilities.Scheduling;

public enum ScheduleType
{
	Unknown = 0,

	/// <summary>
	/// Occurs on a specific date/time. Never repeats.
	/// </summary>
	OneShot = 1,

	/// <summary>
	/// Occurs periodically at specified TimeSpan.
	/// </summary>
	Interval = 2,

	/// <summary>
	/// Occurs daily at specified time(s).
	/// </summary>
	Daily = 3,

	/// <summary>
	/// Occurs weekly on specified days of week, and at specified start time.
	/// </summary>
	Weekly = 4,

	/// <summary>
	/// Occurs in specified months, on the specified day of month, and at specified start time.
	/// </summary>
	Monthly = 5
}

/// <summary>
/// Which day of the week should be used. Since it is a flag, it can be combined as well.
/// </summary>
[Flags]
public enum DaysOfWeek
{
	Unknown = 0,
	Sunday = 1 << 0,
	Monday = 1 << 1,
	Tuesday = 1 << 2,
	Wednesday = 1 << 3,
	Thursday = 1 << 4,
	Friday = 1 << 5,
	Saturday = 1 << 6,
}

/// <summary>
/// Which occurance of the day of the week in the month should be used. E.g. 'Third' Monday of the month.
/// </summary>
public enum DayOfMonth
{
	Unknown = 0,
	First = 1,
	Second = 2,
	Third = 3,
	Fourth = 4,
	Last = 5
}

/// <summary>
/// Flags for the months of the year. Can be combined for multiple months in a year.
/// </summary>
[Flags]
public enum MonthsOfYear
{
	Unknown = 0,
	January = 1 << 0,
	February = 1 << 1,
	March = 1 << 2,
	April = 1 << 3,
	May = 1 << 4,
	June = 1 << 5,
	July = 1 << 6,
	August = 1 << 7,
	September = 1 << 8,
	October = 1 << 9,
	November = 1 << 10,
	December = 1 << 11
}

/// <summary>
/// State of the process for background workers.
/// </summary>
public enum ProcessState
{
	None = 0,
	NoRetry = 1,
	Retry = 2
}
