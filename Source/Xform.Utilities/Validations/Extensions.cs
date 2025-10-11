// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace XForm.Utilities.Validations;

public static class Extensions
{
	#region - String -

	/// <summary>
	/// Determines if the given string is not null.
	/// </summary>
	/// <param name="guidValue"></param>
	/// <returns></returns>
	public static bool IsNotNull([NotNull] this string? guidValue)
	{
		if (guidValue == null)
		{
			guidValue ??= string.Empty; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return true;
	}

	/// <summary>
	///  Validates that the given string is not null or empty.
	/// </summary>
	/// <param name="stringValue"></param>
	/// <param name="considerEmptyIf"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this string? stringValue, string? considerEmptyIf = null)
	{
		stringValue ??= string.Empty; // To avoid warning CS8601: Possible null reference assignment.

		string trimmed = stringValue.Trim();

		if (string.IsNullOrEmpty(stringValue) == true
			|| string.IsNullOrWhiteSpace(trimmed) == true)
		{
			return false;
		}
		else if (considerEmptyIf != null
					&& stringValue.Trim().Equals(considerEmptyIf, StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given string is null and empty. For any other character to be treated as empty, use the second paraameter.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="considerNothingIf"></param>
	/// <returns></returns>
	public static bool HasNothing(this string? value, string? considerNothingIf = null)
	{
		value ??= string.Empty;

		string trimmed = value.Trim();

		if (string.IsNullOrEmpty(value)
			|| string.IsNullOrWhiteSpace(value))
		{
			return true;
		}
		else if (considerNothingIf != null
			&& value.Trim().Equals(considerNothingIf, StringComparison.InvariantCultureIgnoreCase))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Determines if the given string corresponds to a valid HTML color code.
	/// </summary>
	/// <param name="hexString"></param>
	/// <returns></returns>
	public static bool IsColorCode(this string hexStringOrColorName)
	{
		try
		{
			var color = System.Drawing.ColorTranslator.FromHtml(hexStringOrColorName);
			return true;
		}
		catch
		{
			return false;
		}
	}

	#endregion - String -

	#region - Guid -

	/// <summary>
	/// Determines if the given guid is not null.
	/// </summary>
	/// <param name="guidValue"></param>
	/// <returns></returns>
	public static bool IsNotNull([NotNull] this Guid? guidValue)
	{
		if (guidValue == null)
		{
			guidValue ??= Guid.Empty; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines whether the specified GUID is not null or default.
	/// </summary>
	/// <param name="guid"></param>
	/// <param name="considerEmptyIf"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this Guid? guid, Guid? considerEmptyIf = null)
	{
		if (guid == null
			|| guid == default(Guid)
			|| guid == considerEmptyIf)
		{
			guid ??= Guid.Empty;
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines whether the specified GUID is null or empty.
	/// </summary>
	/// <param name="guid"></param>
	/// <param name="considerEmptyIf"></param>
	/// <returns></returns>
	public static bool HasNothing(this Guid? guid, Guid? considerEmptyIf = null)
	{
		if (guid == null 
			|| guid == Guid.Empty)
		{
			return true;
		}

		if ((guid ?? Guid.Empty) == (considerEmptyIf ?? Guid.Empty))
		{
			return true;
		}

		return false;
	}

	#endregion - Guid -

	#region - Numbers - 

	/// <summary>
	/// Determines if the given integer is not null.
	/// </summary>
	/// <param name="intValue"></param>
	/// <param name="nameOfParam"></param>
	/// <returns></returns>
	public static bool IsNotNull([NotNull] this int? intValue)
	{
		if (intValue == null)
		{
			intValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given integer is not null or default.
	/// </summary>
	/// <param name="intValue"></param>
	/// <param name="nameOfParam"></param>
	/// <param name="considerDefaultIf"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this int? intValue, int? considerDefaultIf = 0)
	{
		if (intValue == null
			|| intValue == default(int))
		{
			intValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		if (intValue == considerDefaultIf)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given decimal value is not null.
	/// </summary>
	/// <param name="decimalValue"></param>
	/// <param name="nameOfParam"></param>
	/// <returns></returns>
	public static bool IsNotNull(this decimal? decimalValue)
	{
		if (decimalValue == null)
		{
			decimalValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given decimal value is not null or default.
	/// </summary>
	/// <param name="decimalValue"></param>
	/// <param name="nameOfParam"></param>
	/// <param name="considerDefaultIf"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this decimal? decimalValue, decimal? considerDefaultIf = 0)
	{
		if (decimalValue == null
			|| decimalValue == default)
		{
			decimalValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		if (decimalValue == considerDefaultIf)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given double value is not null.
	/// </summary>
	/// <param name="doubleValue"></param>
	/// <param name="nameOfParam"></param>
	/// <returns></returns>
	public static bool IsNotNull([NotNull] this double? doubleValue)
	{
		if (doubleValue == null)
		{
			doubleValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return true;
	}

	/// <summary>
	/// Determines if the given decimal value is not null or default.
	/// </summary>
	/// <param name="doubleValue"></param>
	/// <param name="nameOfParam"></param>
	/// <param name="considerDefaultIf"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this double? doubleValue, double? considerDefaultIf = 0)
	{
		if (doubleValue == null
			|| doubleValue == default)
		{
			doubleValue ??= 0; // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		if (doubleValue == considerDefaultIf)
		{
			return false;
		}

		return true;
	}

	#endregion - Numbers - 

	#region - Dictionary -

	/// <summary>
	/// Determines if the given dictionary has no values (is null or empty).
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <returns></returns>
	public static bool HasNothing<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary) where TKey : notnull 
	{
		return dictionary == null || dictionary.Count == 0;
	}

	/// <summary>
	/// Determines if the given dictionary has some values (is not null and not empty).
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <returns></returns>
	public static bool HasSomething<TKey, TValue>([NotNull] this Dictionary<TKey, TValue>? dictionary) where TKey : notnull 
	{
		if (dictionary == null)
		{
			dictionary = new Dictionary<TKey, TValue>(); // To avoid warning CS8601: Possible null reference assignment.

			return false;
		}

		return dictionary.Count > 0;
	}

	#endregion - Dictionary -

	#region - Boolean -

	/// <summary>
	/// Determines if the boolean value has something (is not null).
	/// </summary>
	/// <param name="boolean"></param>
	/// <returns></returns>
	public static bool HasSomething([NotNull] this bool? boolean)
	{
		if (boolean == null)
		{
			boolean = false; // To avoid warning CS8601: Possible null reference assignment.

			return false;
		}

		return boolean.HasValue;
	}

	#endregion - Boolean -

	#region - Generic -

	/// <summary>
	/// Validates that the given value is not null or default.
	/// </summary>
	/// <typeparam name="T">The type of the value being validated.</typeparam>
	/// <param name="val">The value being validated.</param>
	/// <param name="nameOfParam">The name of the parameter being validated.</param>
	/// <returns>True if the value is valid.</returns>
	public static bool HasSomething<T>([NotNull] this T? val, T? considerDefaultIf = null) where T : struct
	{
		if (val == null
			|| EqualityComparer<T>.Default.Equals(val.Value, default))
		{
			val = default(T); // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		if (val.Equals(considerDefaultIf) == true)
		{
			return false;
		}

		return true;
	}

	#endregion - Generic -

	#region - Enumberable -

	/// <summary>
	/// Checks if the list is null or empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static bool HasNothing<T>(this IEnumerable<T>? list)
	{
		return list == null || list.Any() == false;
	}

	/// <summary>
	/// Checks if the list is not null and not empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static bool HasSomething<T>([NotNull] this IEnumerable<T>? list)
	{
		if (list == null)
		{
			list = new List<T>(); // To avoid warning CS8601: Possible null reference assignment.
			return false;
		}

		return list.Any() == true;
	}

	#endregion - Enumberable -

}
