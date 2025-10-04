// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Reflection;
using XForm.Utilities.Validations;

namespace XForm.Utilities.Extensions;

public static class EnumerableExtensions
{
	/// <summary>
	/// Creates the CSV from a generic list.
	/// </summary>;
	/// <typeparam name="T"></typeparam>;
	/// <param name="list">The list.</param>;
	/// <param name="csvNameWithExt">Name of CSV (w/ path) w/ file ext.</param>;
	public static bool CreateCSVFromGenericList<T>(this List<T?>? list, string csvCompletePath)
	{
		if (list == null || list.FirstOrDefault() == null)
		{
			return false;
		}

		if (csvCompletePath.HasNothing()
			|| Path.GetDirectoryName(csvCompletePath) == null)
		{
			throw new ArgumentException($"The path '{csvCompletePath}' is not valid.");
		}
		else
		{
			// Get type from 0th member
			Type t;
			var first_item = list[0];
			Xssert.IsNotNull(first_item);

			t = first_item.GetType();

			var new_line = Environment.NewLine;

			var directory_path = Path.GetDirectoryName(csvCompletePath);
			Xssert.IsNotNull(directory_path);

			if (Directory.Exists(directory_path) == false)
			{
				Directory.CreateDirectory(directory_path);
			}

			if (File.Exists(csvCompletePath))
			{
				throw new ArgumentException($"The file '{csvCompletePath}' already exists.");
			}

			//using var sw = new StreamWriter(csvCompletePath);

			//make a new instance of the class name we figured out to get its props
			object? o = Activator.CreateInstance(t);
			if (o == null)
			{
				return false;
			}

			//gets all properties
			PropertyInfo[] props = o.GetType().GetProperties();

			//foreach of the properties in class above, write out properties
			//this is the header row
			File.AppendAllLines(csvCompletePath, new string[] { string.Join(",", props.Select(d => d.Name).ToArray()) });
			//sw.Write(string.Join(",", props.Select(d => d.Name).ToArray()) + new_line);

			//this acts as datarow
			foreach (T? item in list)
			{
				if (item != null)
				{
					//this acts as datacolumn
					var row = string.Join(",", props.Select(d => item.GetType()?
																	.GetProperty(d.Name)?
																	.GetValue(item, null)?
																	.ToString())
															.ToArray());
					//sw.Write(row + new_line);
					File.AppendAllLines(csvCompletePath, new string[] { string.Join(",", row) });
				}
			}
			return true;
		}
	}
}
