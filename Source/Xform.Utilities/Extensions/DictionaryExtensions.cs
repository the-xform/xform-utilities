// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Collections;

namespace XForm.Utilities.Extensions;

public static class DictionaryExtensions
{

	/// <summary>
	/// Gets the index of the item corresponding to the given key, from the dictionary.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="K"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <returns></returns>
	public static int GetIndexOfDictionaryKey<T, K>(this IDictionary<T, K> dictionary, dynamic key)
	{
		for (int index = 0; index < dictionary.Count; index++)
		{
			if (dictionary.Skip(index).First().Key == key)
				return index;
		}

		return -1;
	}

	/// <summary>
	/// Adds an item to the dictionary without throwing the duplicate key error; replaces the value if key already exists.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns>Returns true if the item was added as new and false if the item was found and value was replaced with new one.</returns>
	public static bool SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool overrideExistingValue = true)
	{
		if (dictionary.ContainsKey(key))
		{
			if (overrideExistingValue)
			{
				dictionary[key] = value;
			}
			return false;
		}
		else
		{
			dictionary.Add(key, value);
			return true;
		}
	}

	/// <summary>
	/// Safely gets a value from the generic dictionary. If not found, returns the default value of the value-type.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <param name="defaultReturnValue"></param>
	/// <returns></returns>
	public static TValue? SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultReturnValue = default)
	{
		if (dictionary != null && key != null && dictionary.ContainsKey(key))
		{
			return dictionary[key] = dictionary[key];
		}
		return defaultReturnValue;
	}

	/// <summary>
	/// Safely gets a value from a dictionary. If not found, returns the default value of the value-type.
	/// </summary>
	/// <typeparam name="TReturnValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <param name="defaultReturnValue"></param>
	/// <returns></returns>
	public static TReturnValue? SafeGet<TReturnValue>(this IDictionary dictionary, object key, TReturnValue? defaultReturnValue = default)
	{
		if (dictionary?.Contains(key) == true
			&& dictionary[key] != null)
		{
			return dictionary[key].ConvertTo<TReturnValue>();
		}
		return defaultReturnValue;
	}

	/// <summary>
	/// Renames a key in the dictionary.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dic"></param>
	/// <param name="fromKey"></param>
	/// <param name="toKey"></param>
	public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
								  TKey fromKey, TKey toKey)
	{
		TValue value = dic[fromKey];
		dic.Remove(fromKey);
		dic[toKey] = value;
	}
}
