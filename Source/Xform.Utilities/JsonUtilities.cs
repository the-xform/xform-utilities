// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace XForm.Utilities;

/// <summary>
/// JSON serialization and deserialization utilities.
/// </summary>
public interface IJsonUtilities
{
	/// <summary>
	/// Default JSON serialization options with indentation enabled.
	/// </summary>
	JsonSerializerOptions DefaultOptions { get; }

	/// <summary>
	/// Default JSON serialization options with indentation disabled.
	/// </summary>
	JsonSerializerOptions DefaultOptionsWithNoIndentation { get; }

	/// <summary>
	/// Serialize an object to JSON string with indentation enabled.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="indented"></param>
	/// <returns></returns>
	string ToJson(object value, bool indented);

	/// <summary>
	/// Deserialize a JSON string to an object of type T.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	T? FromJson<T>(string value);
}

/// <summary>
/// JSON serialization and deserialization utilities.
/// </summary>
public class JsonUtilities : IJsonUtilities
{
	#region - Private Properties -

	/// <summary>
	/// Default settings for json serializer.
	/// </summary>
	private static JsonSerializerOptions _defaultOptions => new JsonSerializerOptions
	{
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never,
		Converters = { new JsonStringEnumConverter() },

		WriteIndented = true,
	};

	/// <summary>
	/// Default settings for json serializer.
	/// </summary>
	private static JsonSerializerOptions _defaultOptionsWithNoIndentation => new JsonSerializerOptions
	{
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never,
		Converters = { new JsonStringEnumConverter() },

		WriteIndented = false,
	};

	#endregion - Private Properties -

	/// <summary>
	/// Default settings for json serializer.
	/// </summary>
	JsonSerializerOptions IJsonUtilities.DefaultOptions => _defaultOptions;

	/// <summary>
	/// Default settings for json serializer with no indentation.
	/// </summary>
	JsonSerializerOptions IJsonUtilities.DefaultOptionsWithNoIndentation => _defaultOptionsWithNoIndentation;

	/// <summary>
	/// Serializes the object to json. Centralizes the use of serializer to only this method.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="indented"></param>
	/// <returns></returns>
	public string ToJson(object value, bool indented) => ConvertToJson(value, indented);

	/// <summary>
	/// Deserializes the json string to object. Centralizes the use of serializer to only this method.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public T? FromJson<T>(string value) => ConvertFromJson<T?>(value);

    /// <summary>
    /// Serializes the object to indented json. Centralizes the use of serializer to only this method.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indented"></param>
    /// <returns></returns>
    public static string ConvertToJson(object value, bool indented = true)
    {
        return JsonSerializer.Serialize(value, indented ? JsonUtilities._defaultOptions : JsonUtilities._defaultOptionsWithNoIndentation);
    }

    /// <summary>
    /// Deserializes the json string to object. Centralizes the use of serializer to only this method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? ConvertFromJson<T>(string value)
    {
		return JsonSerializer.Deserialize<T>(value, JsonUtilities._defaultOptions);
    }
}
