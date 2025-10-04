// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Text.RegularExpressions;
using XForm.Utilities.Validations;

namespace XForm.Utilities.Extensions;

public static class StringExtensions
{
	/// <summary>
	/// Removes all special characters from the string except for alphanumeric characters, underscore, and period.
	/// </summary>
	/// <param name="str"></param>
	/// <param name="replaceWith"></param>
	/// <returns></returns>
	public static string RemoveSpecialCharacters(this string str, string replaceWith = "")
	{
		return Regex.Replace(str, "[^a-zA-Z0-9_.]+", replaceWith);
	}

	/// <summary>
	/// Converts a delimited string to a generic list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="commaSeparatedString"></param>
	/// <param name="delimiters">Defaults to { ',', ' ' } is no delimiters are specified.</param>
	/// <returns></returns>
	public static IEnumerable<T> GetNonEmptyValuesAsList<T>(this string? commaSeparatedString, params char[] delimiters)
	{
		if (string.IsNullOrWhiteSpace(commaSeparatedString))
		{
			return new List<T>();
		}

		if (delimiters == null || delimiters.Length == 0)
		{
			delimiters = new char[] { ',', ' ', ';', '|' };
		}

		var selected_values = commaSeparatedString.Split(delimiters).Where(s => s.HasSomething()).Select(i => i.ConvertTo<T>()).ToList();
		return (IEnumerable<T>)selected_values.Where(i => i != null);
	}

	/// <summary>
	/// Search for given string with case-insensitivity (by default).
	/// </summary>
	/// <param name="source"></param>
	/// <param name="toCheck"></param>
	/// <param name="comp"></param>
	/// <returns></returns>
	public static bool ContainsString(this string source,
										string toCheck,
										StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
	{
		return source?.IndexOf(toCheck, comp) >= 0;
	}

}
