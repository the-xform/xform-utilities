// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Diagnostics.CodeAnalysis;

namespace XForm.Utilities.Validations;

public static class Extensions
{
	#region - String -

	/// <summary>
	///  Validates that the given value is not null or empty.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="parameterName"></param>
	/// <param name="considerEmptyIf"></param>
	/// <returns></returns>
	public static bool HasSomething(this string? value, string parameterName = "", string? considerEmptyIf = null)
	{
		value ??= string.Empty;

		string trimmed = value.Trim();

		if (string.IsNullOrEmpty(value) == true
			|| string.IsNullOrWhiteSpace(trimmed) == true)
		{
			return false;
		}
		else if (considerEmptyIf != null
					&& value.Trim().Equals(considerEmptyIf, StringComparison.InvariantCultureIgnoreCase))
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
	public static bool IsColorCode(this string hexString)
	{
		try
		{
			var color = System.Drawing.ColorTranslator.FromHtml(hexString);
			return true;
		}
		catch
		{
			return false;
		}
	}

	#endregion - String -

	#region - Generic -

	/// <summary>
	/// Validates that the given value is not null or default.
	/// </summary>
	/// <typeparam name="T">The type of the value being validated.</typeparam>
	/// <param name="val">The value being validated.</param>
	/// <param name="nameOfParam">The name of the parameter being validated.</param>
	/// <returns>True if the value is valid.</returns>
	public static bool IsNotNullOrDefault<T>(this T? val, string nameOfParam, T? considerDefaultIf = null) where T : struct
	{
		if (val == null
			|| EqualityComparer<T>.Default.Equals(val.Value, default))
		{
			return false;
		}

		if (val.Equals(considerDefaultIf) == true)
		{
			return false;
		}

		return true;
	}

	#endregion - Generic -

	#region - Guid -

	/// <summary>
	/// Determines whether the specified GUID is not null or default.
	/// </summary>
	/// <param name="guid"></param>
	/// <param name="nameOfParam"></param>
	/// <returns></returns>
	public static bool IsNotNullOrDefault(this Guid? guid, string nameOfParam, bool considerDefaultIfEmpty = false)
	{
		if (guid == null || guid == default(Guid))
		{
			return false;
		}

		if (considerDefaultIfEmpty && guid == Guid.Empty)
		{
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
		if (guid == null || guid == Guid.Empty)
		{
			return true;
		}
		if (considerEmptyIf.HasValue && guid == considerEmptyIf.Value)
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
	public static bool IsNotNull(this int? intValue, string nameOfParam)
	{
		if (intValue == null)
		{
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
	public static bool IsNotNullOrDefault(this int? intValue, string nameOfParam, int? considerDefaultIf = 0)
	{
		if (intValue == null
			|| EqualityComparer<int>.Default.Equals(intValue))
		{
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
	public static bool IsNotNull(this decimal? decimalValue, string nameOfParam)
	{
		if (decimalValue == null)
		{
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
	public static bool IsNotNullOrDefault(this decimal? decimalValue, string nameOfParam, decimal? considerDefaultIf = 0)
	{
		if (decimalValue == null
			|| decimalValue == default)
		{
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
	public static bool IsNotNull(this double? doubleValue, string nameOfParam)
	{
		if (doubleValue == null)
		{
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
	public static bool IsNotNullOrDefault(this double? doubleValue, string nameOfParam, double? considerDefaultIf = 0)
	{
		if (doubleValue == null
			|| doubleValue == default)
		{
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
	public static bool HasNothing<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary) where TKey : notnull // Add 'notnull' constraint to TKey to satisfy Dictionary<TKey, TValue> requirements
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
	public static bool HasSomething<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary) where TKey : notnull // Add 'notnull' constraint to TKey to satisfy Dictionary<TKey, TValue> requirements
	{
		if (dictionary == null)
		{
			// To avoid warning CS8601: Possible null reference assignment.
			dictionary = new Dictionary<TKey, TValue>();

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
			// To avoid warning CS8601: Possible null reference assignment.
			boolean = false;

			return false;
		}

		return boolean.HasValue;
	}

	#endregion - Boolean -

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
			// To avoid warning CS8601: Possible null reference assignment.
			list = new List<T>();

			return true;
		}

		return list.Any() == true;
	}

	#endregion - Enumberable -

}
