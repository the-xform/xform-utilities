// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using Xform.Utilities.Scheduling;
using XForm.Utilities.Validations;

public static class Schedule
{
	#region - Public Static Methods -

	/// <summary>
	/// Builds the appropriate schedule object based on the configuration provided.
	/// </summary>
	/// <param name="configuration"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static ISchedule Build(ScheduleConfiguration configuration)
	{
		switch (configuration.Type)
		{
			case ScheduleType.OneShot:
				{
					Xssert.IsNotNull(configuration.OneShot);
					return configuration.OneShot;
				}
			case ScheduleType.Interval:
				{
					Xssert.IsNotNull(configuration.Interval);
					return configuration.Interval;
				}
			case ScheduleType.Daily:
				{
					Xssert.IsNotNull(configuration.Daily);
					return configuration.Daily;
				}
			case ScheduleType.Weekly:
				{
					Xssert.IsNotNull(configuration.Weekly);
					return configuration.Weekly;
				}
			case ScheduleType.Monthly:
				{
					Xssert.IsNotNull(configuration.Monthly);
					return configuration.Monthly;
				}
			case ScheduleType.Unknown:
			default:
				throw new ArgumentException("Unknown Schedule Type.");
		}
	}

	/// <summary>
	/// Gets the time span (day, hours, minutes, seconds) to the next interval given a specific date/time to calculate from.
	/// </summary>
	/// <param name="schedule"></param>
	/// <param name="startingFrom">Date and Time to calculate offset from to next interval.</param>
	/// <returns>
	/// True: if schedule is active and offset to next interval was calculated.
	/// False: with positive offset indicates time to start of schedule.
	/// False: with negative offset indicates time past the schedule stop.
	/// </returns>
	public static (bool IsActive, TimeSpan OffsetToNext) CalculateOffsetToNextInterval(this ISchedule schedule, DateTime startingFrom)
	{
		var is_active = false;
		var offset_to_next = TimeSpan.Zero;

		// See if the scedule has started yet or has already stopped.
		if (startingFrom >= schedule.ScheduleStart + schedule.TimeStart)
		{
			if (schedule.ScheduleStop == DateTime.MaxValue)
			{
				// Schedule started and no schedule end.
				is_active = true;
			}
			else
			{
				if (startingFrom < schedule.ScheduleStop)
				{
					// We haven't yet reached the end of schedule.
					is_active = true;
				}
				else
				{
					// Schedule already stopped ... let caller know how long ago it stopped.
					offset_to_next = schedule.ScheduleStop - startingFrom;
				}
			}
		}
		else
		{
			// Schedule hasn't started yet ... let caller know how long until start.
			offset_to_next = (schedule.ScheduleStart + schedule.TimeStart) - startingFrom;
		}

		// Active schedule, calculate how long until next execution.
		if (is_active)
		{
			switch (schedule.Type)
			{
				case ScheduleType.OneShot:
					{
						offset_to_next = DoCalculateOffsetOneShot();
						break;
					}
				case ScheduleType.Interval:
					{
						offset_to_next = DoCalculateOffset((IntervalSchedule)schedule);
						break;
					}
				case ScheduleType.Daily:
					{
						offset_to_next = DoCalculateOffset((DailySchedule)schedule, startingFrom);
						break;
					}
				case ScheduleType.Weekly:
					{
						offset_to_next = DoCalculateOffset((WeeklySchedule)schedule, startingFrom);
						break;
					}
				case ScheduleType.Monthly:
					{
						offset_to_next = DoCalculateOffset((MonthlySchedule)schedule, startingFrom);
						break;
					}
				case ScheduleType.Unknown:
				default:
					throw new ArgumentException("Unknown Schedule Type.");
			}
		}

		// One last check to see if calculated offset to next interval would put us past the scheduled stop.
		if (is_active && schedule.ScheduleStop != DateTime.MaxValue)
		{
			bool is_past;
			try
			{
				is_past = startingFrom.Add(offset_to_next) > schedule.ScheduleStop;
			}
			catch (ArgumentOutOfRangeException)
			{
				is_past = true;
			}

			if (is_past)
			{
				// Next interval would be past schedule stop
				offset_to_next = schedule.ScheduleStop - startingFrom;
				is_active = false;
			}
		}

		return (is_active, offset_to_next);
	}

	#endregion - Public Static Methods -

	#region - Private Methods -

	private static TimeSpan DoCalculateOffsetOneShot() => TimeSpan.Zero; // Right "Now", no delay after schedule start.

	private static TimeSpan DoCalculateOffset(IntervalSchedule schedule) => schedule.Duration; // A duration or "Delay" between executions.

	private static TimeSpan DoCalculateOffset(DailySchedule daily, DateTime startingFrom)
	{
		// Get the number of interval days from schedule start to requested start time.
		var days = (startingFrom - daily.ScheduleStart).Days;
		var extra_days = 0;
		var extra_time = TimeSpan.Zero;
		var start_time = startingFrom.TimeOfDay;

		// Bump days until we're on the configured daily interval.
		while ((days % daily.Interval) != 0)
		{
			days++;
			extra_days++;
			start_time = daily.TimesOfTheDay[0];
		}

		// If past last time of the day then bump days again until start of next valid day.
		if (start_time > daily.TimesOfTheDay[^1])
		{
			start_time = daily.TimesOfTheDay[0];

			days++;
			extra_days++;

			while ((days % daily.Interval) != 0)
			{
				days++;
				extra_days++;
			}
		}

		if (extra_days > 0)
		{
			// Calculate how long until midnight so we get to the next day.
			extra_time = startingFrom.AddDays(1).Date - startingFrom;
			// Then add in time to first interval.
			extra_time += start_time;
			// We don't need one of those whole days since we calculated the portion of a day that gets us to tomorrow.
			extra_days--;
		}
		else
		{
			// Find the correct time offset in list of times for next interval.
			foreach (var ts in daily.TimesOfTheDay)
			{
				if (ts >= start_time)
				{
					extra_time = ts - start_time;
					break;
				}
			}
		}

		return new TimeSpan(extra_time.Days + extra_days, extra_time.Hours, extra_time.Minutes, extra_time.Seconds, extra_time.Milliseconds + 1);
	}

	private static TimeSpan DoCalculateOffset(WeeklySchedule weekly, DateTime startingFrom)
	{
		// Calculate the first day of the week for the schedule start.
		var start_of_week_date = weekly.ScheduleStart.AddDays(-(int)weekly.ScheduleStart.DayOfWeek);
		var days = (startingFrom - start_of_week_date).Days;
		var weeks = days / 7;
		var extra_days = 0;
		TimeSpan extra_time;

		// If past the time of the day to start then bump to the next day.
		if (startingFrom.TimeOfDay > weekly.TimeStart)
		{
			days++;
			weeks = days / 7;
			extra_days++;
		}

		// Bump days until we're on the configured weekly interval and on a valid day of the week.
		var is_valid_day = (((int)weekly.DaysOfTheWeek) & (1 << ((int)startingFrom.AddDays(extra_days).DayOfWeek))) != 0;
		while ((weeks % weekly.Interval) != 0 || is_valid_day == false)
		{
			days++;
			weeks = days / 7;
			extra_days++;
			is_valid_day = (((int)weekly.DaysOfTheWeek) & (1 << ((int)startingFrom.AddDays(extra_days).DayOfWeek))) != 0;
		}

		if (extra_days > 0)
		{
			// Calculate how long until midnight so we get to the next day.
			extra_time = startingFrom.AddDays(1).Date - startingFrom;
			// Then add in time to first interval.
			extra_time += weekly.TimeStart;
			// We don't need one of those whole days since we calculated the portion of a day that gets us to tomorrow.
			extra_days--;
		}
		else
		{
			extra_time = weekly.TimeStart - startingFrom.TimeOfDay;
		}

		return new TimeSpan(extra_time.Days + extra_days, extra_time.Hours, extra_time.Minutes, extra_time.Seconds, extra_time.Milliseconds + 1);
	}

	private static TimeSpan DoCalculateOffset(MonthlySchedule monthly, DateTime startingFrom)
	{
		var offset_to_next = TimeSpan.Zero;

		// Find the right month
		var extra_months = 0;

		var target_date = new DateTime(startingFrom.Year, startingFrom.Month, 1);
		var is_valid_month = (((int)monthly.MonthsOfTheYear) & (1 << (target_date.AddMonths(extra_months).Month - 1))) != 0;

		// Bump the months until we get the first valid month.
		while (!is_valid_month)
		{
			extra_months++;
			is_valid_month = (((int)monthly.MonthsOfTheYear) & (1 << (target_date.AddMonths(extra_months).Month - 1))) != 0;
		}

		// Calculate the target day number now that we have a possible valid target month.
		var day_number_in_month = monthly.CalculateDayNumberInMonth(target_date.AddMonths(extra_months).Year, target_date.AddMonths(extra_months).Month);
		var is_valid_day = false;

		// Special check to see if we're exactly on the right month and not later on the day and time.
		if (extra_months == 0)
		{
			if (startingFrom.Day == day_number_in_month)
			{
				if (startingFrom.TimeOfDay <= monthly.TimeStart)
				{
					offset_to_next = monthly.TimeStart - startingFrom.TimeOfDay + TimeSpan.FromMilliseconds(1);
					is_valid_day = true;
				}
			}
			else if (startingFrom.Day < day_number_in_month)
			{
				offset_to_next = (new DateTime(startingFrom.Year, startingFrom.Month, day_number_in_month) + monthly.TimeStart) - startingFrom + TimeSpan.FromMilliseconds(1);
				is_valid_day = true;
			}
			else
			{
				// Not a valid starting day ... keep going.
			}

			// Bump to the next month since this one is not valid.
			if (is_valid_day == false)
			{
				extra_months++;
			}
		}

		// If we didn't start on the correct month or starting time was already over then keep searching.
		if (is_valid_day == false)
		{
			is_valid_month = (((int)monthly.MonthsOfTheYear) & (1 << (target_date.AddMonths(extra_months).Month - 1))) != 0;

			// Bump the months until we get the first valid month.
			while (!is_valid_month)
			{
				extra_months++;
				is_valid_month = (((int)monthly.MonthsOfTheYear) & (1 << (target_date.AddMonths(extra_months).Month - 1))) != 0;
			}

			// We should be on the final valid target month ... recalculate day number and final offset.
			target_date = target_date.AddMonths(extra_months);
			day_number_in_month = monthly.CalculateDayNumberInMonth(target_date.Year, target_date.Month);
			offset_to_next = ((new DateTime(target_date.Year, target_date.Month, day_number_in_month) + monthly.TimeStart) - startingFrom) + TimeSpan.FromMilliseconds(1);
		}

		return offset_to_next;
	}

	#endregion - Private Methods -
}
