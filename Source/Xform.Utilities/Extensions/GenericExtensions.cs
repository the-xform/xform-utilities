// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

namespace XForm.Utilities.Extensions;

public static class GenericExtensions
{
	/// <summary>
	/// Converts the given value to specified type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static T? ConvertTo<T>(this object? value)
	{
		if (value == null)
		{
			return default;
		}

		Type target_type = typeof(T);

		if (target_type.IsEnum)
		{
			if (value is not string)
			{
				throw new ArgumentException($"Cannot convert '{value}' to enum '{target_type.Name}'. The value must be a string.");
			}

			if (Enum.IsDefined(target_type, value))
			{
				return (T)Enum.Parse(target_type, (string)value, ignoreCase: true);
			}
			else
			{
				throw new ArgumentException($"The value '{value}' is not defined in the enum '{target_type.Name}'.");
			}
		}
		else if (target_type == typeof(bool))
		{
			if (value is string string_value)
			{
				return (T)(object)Boolean.Parse(string_value);
			}
			else if (value is bool)
			{
				return (T)value;
			}
			else
			{
				throw new ArgumentException($"Cannot convert '{value}' to bool.");
			}
		}
		else if (target_type == typeof(TimeSpan))
		{
			if (value is string string_value)
			{
				return (T)(object)TimeSpan.Parse(string_value);
			}
			else if (value is TimeSpan)
			{
				return (T)value;
			}
			else
			{
				throw new ArgumentException($"Cannot convert '{value}' to TimeSpan.");
			}
		}
		else if (target_type == typeof(Guid))
		{
			if (value is string)
			{
				Guid guid;
				if (Guid.TryParse(value as string, out guid) == true)
				{
					return (T)(object)guid;
				}
				else
				{
					throw new ArgumentException($"The value '{value}' cannot be converted to Guid.");
				}
			}
		}

		return (T)Convert.ChangeType(value, target_type);
	}
}
