// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.FileProviders;

namespace XForm.Utilities.Validations;

public static class Xssert
{
	#region - Generic -

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <param name="value"></param>
	/// <param name="target"></param>
	/// <param name="parameterName"></param>
	/// <param name="targetName"></param>
	/// <exception cref="ArgumentException"></exception>
	public static void Equal<TType>(
	  TType value,
	  TType target,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string targetName = "") where TType : notnull
	{
		if (value.Equals(target) == false)
		{
			throw new ArgumentException($"Value of '{parameterName}' must be equal to the value of '{targetName}'.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <param name="value"></param>
	/// <param name="target"></param>
	/// <param name="parameterName"></param>
	/// <param name="targetName"></param>
	/// <exception cref="ArgumentException"></exception>
	public static void NotEqual<TType>(
	  TType value,
	  TType target,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string targetName = "") where TType : notnull
	{
		if (value.Equals(target) == true)
		{
			throw new ArgumentException($"Value of '{parameterName}' must not be equal to the value of '{targetName}'.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <param name="value"></param>
	/// <param name="target"></param>
	/// <param name="parameterName"></param>
	/// <param name="targetName"></param>
	/// <exception cref="ArgumentException"></exception>
	public static void GreaterThan<TType>(
	  TType value,
	  TType target,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string targetName = "") where TType : notnull
	{
		if (Comparer<TType>.Default.Compare(value, target) <= 0)
		{
			throw new ArgumentException($"Value of '{parameterName}' must be greater than the value of '{targetName}'.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <param name="value"></param>
	/// <param name="target"></param>
	/// <param name="parameterName"></param>
	/// <param name="targetName"></param>
	/// <exception cref="Exception"></exception>
	public static void LessThan<TType>(
	  TType value,
	  TType target,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string targetName = "") where TType : notnull
	{
		if (Comparer<TType>.Default.Compare(value, target) >= 0)
		{
			throw new Exception($"Value of '{parameterName}' must be less than the value of '{targetName}'.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="parameterName"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static void IsNotNull<T>([NotNull] T value,
		[CallerMemberName] string parameterName = "")
	{
		if (value == null)
		{
			throw new ArgumentNullException($"Value of '{parameterName}' cannot be null.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="parameterName"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static void IsNotNullOrEmpty<T>([NotNull] IEnumerable<T> value, [CallerMemberName] string parameterName = "")
	{
		IsNotNull(value, parameterName);
		if (value?.Any() != true)
		{
			throw new ArgumentNullException($"Value of '{parameterName}' cannot be null or empty.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="value"></param>
	/// <param name="parameterName"></param>
	/// <param name="dictionaryName"></param>
	/// <exception cref="KeyNotFoundException"></exception>
	public static void ContainsKey<TKey, TValue>(
	  IDictionary<TKey, TValue> dictionary,
	  TKey value,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string dictionaryName = "")
	{
		if (dictionary.ContainsKey(value) == false)
		{
			throw new KeyNotFoundException($"No key with name '{parameterName}' found in dictionary '{dictionaryName}'.");
		}
	}

	public static void Contains<TValue>(
	  IEnumerable<TValue> enumerable,
	  TValue value,
	  [CallerMemberName] string parameterName = "",
	  [CallerMemberName] string enumerableName = "")
	{
		if (enumerable.Contains(value) == false)
		{
			throw new KeyNotFoundException($"Value '{value}' of parameter '{parameterName}' not contained within '{enumerableName}'.");
		}
	}

	#endregion - Generic -

	#region - IFileInfo -

	public static void FileExists(IFileInfo value,
		[CallerMemberName] string parameterName = "")
	{
		var fileInfo = value;
		if (fileInfo == null)
		{
			throw new ArgumentNullException($"'{parameterName}' is null.");
		}

		if (fileInfo.Exists == false)
		{
			throw new FileNotFoundException($"File '{parameterName}' does not exist.");
		}
	}

	#endregion - IFileInfo -
}
