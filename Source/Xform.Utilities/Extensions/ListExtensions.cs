// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

namespace XForm.Utilities.Extensions;

public static class ListExtensions
{
	/// <summary>
	/// Adds an item to the given list if the value is not null or empty (in case of string).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="listValue"></param>
	/// <param name="value"></param>
	public static void AddIfNotNullOrEmpty<T>(this List<T>? listValue, T value)
	{
		if (typeof(T) == typeof(string))
		{
			if (string.IsNullOrEmpty(value?.ToString()) == false)
			{
				listValue?.Add(value);
			}
		}
		else if (value != null)
		{
			listValue?.Add(value);
		}
	}
}
