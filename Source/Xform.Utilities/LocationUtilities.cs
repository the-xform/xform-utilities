// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

namespace XForm.Utilities;

public static class LocationUtilities
{
	/// <summary>
	/// Gets the path to the directory where the entry assembly is located.
	/// </summary>
	/// <returns></returns>
	public static string GetEntryAssemblyDirectory()
	{
		var entry_asembly = System.Reflection.Assembly.GetEntryAssembly();

		if (entry_asembly != null)
		{
			var code_base = entry_asembly.Location;
			var uri = new UriBuilder(code_base);
			var path = Uri.UnescapeDataString(uri.Path);
			var dir_path = Path.GetDirectoryName(path);
			if (dir_path != null)
			{
				return dir_path;
			}
			return "";
		}
		return "";
	}
}
