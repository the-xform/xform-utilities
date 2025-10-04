// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

namespace XForm.Utilities.Extensions;

public static class DateTimeExtensions
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="dateTime"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static DateTime ToUtcIgnoreDst(this DateTime dateTime)
	{
		if (dateTime.Kind == DateTimeKind.Utc)
		{
			throw new ArgumentException("DateTime object already in UTC");
		}
		var dt = dateTime + TimeZoneInfo.Local.BaseUtcOffset;
		DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		return dt;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="dateTime"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static DateTime ToLocalIgnoreDst(this DateTime dateTime)
	{
		if (dateTime.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("DateTime object is not in UTC");
		}
		var dt = dateTime + TimeZoneInfo.Local.BaseUtcOffset;
		DateTime.SpecifyKind(dt, DateTimeKind.Local);
		return dt;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="dateTime"></param>
	/// <returns></returns>
	public static string ConvertToISO8601Format(this DateTimeOffset dateTime)
	{
		return dateTime.ToString("yyyymmddThhmmss+|-hhmm");
	}

	/// <summary>
	/// Takes the given date and adds the given offset to create equivalent DateTimeOffset value.
	/// </summary>
	/// <param name="dateTime"></param>
	/// <param name="timeZoneInfo"></param>
	/// <returns></returns>
	public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
	{
		return new DateTimeOffset(dateTime.Ticks, timeZoneInfo.BaseUtcOffset);
	}

	/// <summary>
	/// Takes the given date and adds the given offset to create equivalent DateTimeOffset value.
	/// </summary>
	/// <param name="dateTime"></param>
	/// <param name="timeSpan"></param>
	/// <returns></returns>
	public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeSpan timeSpan)
	{
		return new DateTimeOffset(dateTime, timeSpan);
	}
}
