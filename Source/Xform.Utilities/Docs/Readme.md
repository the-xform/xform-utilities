# xform-utilities

Day-to-day utilities that make handling **strings**, **integers**, **GUIDs**, and **objects** easier, resulting in increased efficiency for developers.


## Overview

`xform-utilities` is a lightweight .NET library that provides a set of reusable helpers and extension methods for common operations that developers perform frequently — trimming strings, parsing safely, working with GUIDs, validating inputs, and copying object properties.


## Getting Started

Add the project or NuGet reference to your .NET application and import the namespace:

```csharp
using Xform.Utilities;
```

Then you can call extension methods directly on native types like `string`, `int`, `Guid`, and `object`.


# String Extensions

**Namespace:** `XForm.Utilities.Extensions`  
**Assembly:** `XForm.Utilities.dll`  

The `StringExtensions` class provides commonly used **string utility methods** that extend the functionality of the .NET `string` type.  
These extensions simplify data cleansing, conversion, and searching tasks.

## Methods

### 1. `RemoveSpecialCharacters()`

```csharp
public static string RemoveSpecialCharacters(this string str, string replaceWith = "")
```

#### Description
Removes all special characters from a string except **alphanumeric characters**, underscores (`_`), and periods (`.`).  
Useful for sanitizing input data or generating valid filenames, identifiers, or keys.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `str` | `string` | Input string to clean. |
| `replaceWith` | `string` | Optional. Replacement for removed characters. Default is empty. |

#### Returns
A string with special characters removed or replaced.

#### Example

```csharp
using XForm.Utilities.Extensions;

string input = "User@#123.Name!";
string cleaned = input.RemoveSpecialCharacters();
// cleaned = "User123.Name"

string replaced = input.RemoveSpecialCharacters("_");
// replaced = "User__123.Name_"
```

---

### 2. `GetNonEmptyValuesAsList<T>()`

```csharp
public static IEnumerable<T> GetNonEmptyValuesAsList<T>(this string? commaSeparatedString, params char[] delimiters)
```

#### Description
Converts a **delimited string** into a strongly typed list of values.  
Automatically skips empty or whitespace-only entries.

If no delimiters are provided, it defaults to `{ ',', ' ', ';', '|' }`.

> ⚙️ This method uses `HasSomething()` and `ConvertTo<T>()` from `XForm.Utilities.Validations` for non-empty checks and type conversion.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `commaSeparatedString` | `string?` | Input delimited string (e.g., `"1,2,3"`). |
| `delimiters` | `params char[]` | Optional. Custom delimiters to split by. |

#### Returns
An `IEnumerable<T>` containing all non-empty, successfully converted values.

#### Example

```csharp
using XForm.Utilities.Extensions;

string input = "10, 20, 30,  , 40";

// Convert to list of integers
IEnumerable<int> numbers = input.GetNonEmptyValuesAsList<int>();
// numbers = [10, 20, 30, 40]

// Custom delimiter example
string values = "A|B|C||D";
var list = values.GetNonEmptyValuesAsList<string>('|');
// list = ["A", "B", "C", "D"]
```

#### Example with complex type conversion

```csharp
string input = "true; false; true";
IEnumerable<bool> boolList = input.GetNonEmptyValuesAsList<bool>(';');
// boolList = [true, false, true]
```

---

### 3. `ContainsString()`

```csharp
public static bool ContainsString(this string source, string toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
```

#### Description
Checks if the source string contains another string, with **case-insensitive comparison by default**.

This method simplifies substring searching without having to repeatedly call `IndexOf` or configure `StringComparison`.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `source` | `string` | The source string. |
| `toCheck` | `string` | The string to find. |
| `comp` | `StringComparison` | Optional. Comparison type (default: `InvariantCultureIgnoreCase`). |

#### Returns
`true` if `toCheck` exists within `source`, otherwise `false`.

#### Example

```csharp
using XForm.Utilities.Extensions;

string text = "The quick brown fox jumps over the lazy dog.";

bool contains1 = text.ContainsString("FOX"); 
// true (case-insensitive)

bool contains2 = text.ContainsString("cat");
// false

bool contains3 = text.ContainsString("Dog", StringComparison.Ordinal);
// false (case-sensitive)
```




# DateTime Extensions

**Namespace:** `XForm.Utilities.Extensions`  
**Assembly:** `XForm.Utilities.dll`  
**License:** MIT License  
**Author:** Rohit Ahuja  

The `DateTimeExtensions` class provides a set of **extension methods** for working with `DateTime` and `DateTimeOffset` objects.  
These methods handle timezone conversion, formatting, and conversion to offset-based values while ignoring Daylight Saving Time (DST) when necessary.


## Methods

### 1. `ToUtcIgnoreDst()`

```csharp
public static DateTime ToUtcIgnoreDst(this DateTime dateTime)
```

#### Description
Converts a local `DateTime` to **UTC**, ignoring Daylight Saving Time (DST).  
If the input is already in UTC, an `ArgumentException` is thrown.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `dateTime` | `DateTime` | The local date and time to convert. |

#### Returns
A `DateTime` object representing the UTC equivalent of the input time (without considering DST).

#### Exceptions
| Exception | Condition |
|------------|------------|
| `ArgumentException` | Thrown when the input `DateTime` is already in UTC. |

#### Example

```csharp
using XForm.Utilities.Extensions;

DateTime localTime = new DateTime(2025, 10, 13, 15, 0, 0, DateTimeKind.Local);
DateTime utcTime = localTime.ToUtcIgnoreDst();

Console.WriteLine(utcTime); // Output: Adjusted UTC time without DST consideration
```

---

### 2. `ToLocalIgnoreDst()`

```csharp
public static DateTime ToLocalIgnoreDst(this DateTime dateTime)
```

#### Description
Converts a UTC `DateTime` to **local time**, ignoring Daylight Saving Time (DST).  
If the input is not in UTC, an `ArgumentException` is thrown.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `dateTime` | `DateTime` | The UTC date and time to convert. |

#### Returns
A `DateTime` object representing the local equivalent of the input time (without considering DST).

#### Exceptions
| Exception | Condition |
|------------|------------|
| `ArgumentException` | Thrown when the input `DateTime` is not in UTC. |

#### Example

```csharp
using XForm.Utilities.Extensions;

DateTime utcTime = new DateTime(2025, 10, 13, 22, 0, 0, DateTimeKind.Utc);
DateTime localTime = utcTime.ToLocalIgnoreDst();

Console.WriteLine(localTime); // Output: Local time adjusted by BaseUtcOffset
```

---

### 3. `ConvertToISO8601Format()`

```csharp
public static string ConvertToISO8601Format(this DateTimeOffset dateTime)
```

#### Description
Converts a `DateTimeOffset` into a **custom ISO 8601 string** format (`yyyymmddThhmmss+|-hhmm`).  
This method provides a consistent format useful for logging and API serialization.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `dateTime` | `DateTimeOffset` | The date and time value to format. |

#### Returns
A string representing the date in ISO 8601 format.

#### Example

```csharp
using XForm.Utilities.Extensions;

DateTimeOffset date = DateTimeOffset.Now;
string iso = date.ConvertToISO8601Format();

Console.WriteLine(iso); // Output: "20251013T103045+0700" (example)
```

---

### 4. `ToDateTimeOffset(this DateTime dateTime, TimeZoneInfo timeZoneInfo)`

```csharp
public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
```

#### Description
Creates a `DateTimeOffset` by combining a `DateTime` with a given `TimeZoneInfo`.  
The `BaseUtcOffset` from the `timeZoneInfo` is applied.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `dateTime` | `DateTime` | The base date and time. |
| `timeZoneInfo` | `TimeZoneInfo` | The timezone to apply. |

#### Returns
A `DateTimeOffset` adjusted using the timezone’s base offset.

#### Example

```csharp
using XForm.Utilities.Extensions;

DateTime date = new DateTime(2025, 10, 13, 9, 0, 0);
TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

DateTimeOffset dto = date.ToDateTimeOffset(tz);
Console.WriteLine(dto); // Output: 2025-10-13 09:00:00 -07:00
```

---

### 5. `ToDateTimeOffset(this DateTime dateTime, TimeSpan timeSpan)`

```csharp
public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeSpan timeSpan)
```

#### Description
Creates a `DateTimeOffset` using a specified offset (`TimeSpan`).  
This allows explicit control over the UTC offset instead of using system time zones.

#### Parameters
| Parameter | Type | Description |
|------------|------|-------------|
| `dateTime` | `DateTime` | The base date and time. |
| `timeSpan` | `TimeSpan` | The UTC offset to apply. |

#### Returns
A `DateTimeOffset` constructed using the specified offset.

#### Example

```csharp
using XForm.Utilities.Extensions;

DateTime date = new DateTime(2025, 10, 13, 9, 0, 0);
TimeSpan offset = TimeSpan.FromHours(-5);

DateTimeOffset dto = date.ToDateTimeOffset(offset);
Console.WriteLine(dto); // Output: 2025-10-13 09:00:00 -05:00
```

---




# Dictionary Extensions

**Namespace:** `XForm.Utilities.Extensions`  
**Assembly:** `XForm.Utilities`

---

## Overview

The `DictionaryExtensions` class provides a set of extension methods that enhance dictionary operations in C#.  
It includes utility functions for safe addition, retrieval, key renaming, and index lookup.


## Methods

### 1. GetIndexOfDictionaryKey<T, K>

```csharp
public static int GetIndexOfDictionaryKey<T, K>(this IDictionary<T, K> dictionary, dynamic key)
```

**Description:**  
Returns the zero-based index of a specified key within a dictionary.  
If the key does not exist, the method returns `-1`.

**Example:**

```csharp
var dict = new Dictionary<string, int>
{
    { "A", 1 },
    { "B", 2 },
    { "C", 3 }
};

int index = dict.GetIndexOfDictionaryKey("B"); // Returns 1
```

---

### 2. SafeAdd<TKey, TValue>

```csharp
public static bool SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool overrideExistingValue = true)
```

**Description:**  
Adds an item to the dictionary safely without throwing a duplicate key exception.  
If the key already exists, the value is replaced when `overrideExistingValue` is set to `true`.

**Returns:**  
- `true` if the item was added as a new entry.  
- `false` if the key already existed and the value was updated.

**Example:**

```csharp
var dict = new Dictionary<string, int> { { "X", 1 } };

bool added = dict.SafeAdd("Y", 2);  // Returns true (new entry)
bool updated = dict.SafeAdd("X", 5); // Returns false (value replaced)
```

---

### 3. SafeGet<TKey, TValue>

```csharp
public static TValue? SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultReturnValue = default)
```

**Description:**  
Retrieves a value from the dictionary safely.  
If the key does not exist, it returns the provided default value.

**Example:**

```csharp
var dict = new Dictionary<string, string> { { "Name", "Rohit" } };

string name = dict.SafeGet("Name", "Unknown"); // Returns "Rohit"
string email = dict.SafeGet("Email", "N/A");   // Returns "N/A"
```

---

### 4. SafeGet<TReturnValue> (for non-generic IDictionary)

```csharp
public static TReturnValue? SafeGet<TReturnValue>(this IDictionary dictionary, object key, TReturnValue? defaultReturnValue = default)
```

**Description:**  
Retrieves and converts a value safely from a non-generic dictionary.  
If the key is not found, returns the specified default value.

**Example:**

```csharp
IDictionary dict = new Hashtable
{
    { "Age", 25 },
    { "Active", true }
};

int age = dict.SafeGet("Age", 0);       // Returns 25
bool active = dict.SafeGet("Active", false); // Returns true
```

---

### 5. RenameKey<TKey, TValue>

```csharp
public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
```

**Description:**  
Renames an existing key in the dictionary while preserving its associated value.

**Example:**

```csharp
var dict = new Dictionary<string, string>
{
    { "OldKey", "Value" }
};

dict.RenameKey("OldKey", "NewKey");
// Now contains { "NewKey": "Value" }
```

---

## Summary Table

| Method | Description |
|--------|-------------|
| `GetIndexOfDictionaryKey` | Gets the index of a specified key in a dictionary |
| `SafeAdd` | Adds an entry safely, replacing existing values if needed |
| `SafeGet<TKey, TValue>` | Retrieves a value safely from a generic dictionary |
| `SafeGet<TReturnValue>` | Safely retrieves and converts a value from a non-generic dictionary |
| `RenameKey` | Renames a key in the dictionary |




# Enumerable Extensions

**Namespace:** `XForm.Utilities.Extensions`  
**Assembly:** `XForm.Utilities.dll`  
**License:** MIT  
**Author:** [Rohit Ahuja]


## Overview

The `EnumerableExtensions` class provides an extension method for generic lists that allows easy creation of CSV files from list data.

This utility method helps export any list of objects into a structured CSV file, automatically creating headers and data rows based on object properties.


## Method Summary

### `bool CreateCSVFromGenericList<T>(this List<T?>? list, string csvCompletePath)`

Creates a CSV file from a generic list of objects.  
Each property of the object becomes a CSV column header, and each item in the list becomes a data row.

#### **Type Parameters**
- `T` – The type of elements in the list.

#### **Parameters**
| Name | Type | Description |
|------|------|-------------|
| `list` | `List<T?>?` | The list of objects to export as CSV. |
| `csvCompletePath` | `string` | The full path (including file name and extension) for the CSV file to be created. |

#### **Returns**
| Type | Description |
|------|-------------|
| `bool` | Returns `true` if the CSV was successfully created, `false` otherwise. |

#### **Exceptions**
| Exception | Condition |
|------------|------------|
| `ArgumentException` | Thrown if the file path is invalid or if a file already exists at that location. |
| `NullReferenceException` | Thrown if the list or an element is null. |


## Example Usage

```csharp
using XForm.Utilities.Extensions;

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string City { get; set; } = string.Empty;
}

class Program
{
    static void Main()
    {
        var people = new List<Person>
        {
            new Person { Name = "Alice", Age = 28, City = "New York" },
            new Person { Name = "Bob", Age = 35, City = "San Francisco" },
            new Person { Name = "Charlie", Age = 42, City = "Chicago" }
        };

        string csvPath = Path.Combine(Environment.CurrentDirectory, "People.csv");

        bool success = people.CreateCSVFromGenericList(csvPath);

        Console.WriteLine(success
            ? $"CSV successfully created at {csvPath}"
            : "Failed to create CSV file.");
    }
}
```


## Notes

- The method validates the list and CSV path before writing.  
- Automatically creates directories if missing.  
- Skips null list items.  
- Uses reflection to dynamically read property names and values.



## Example Output

For the example above, the generated `People.csv` file will contain:

```csv
Name,Age,City
Alice,28,New York
Bob,35,San Francisco
Charlie,42,Chicago
```


## Related Utilities

- `XForm.Utilities.Validations.Xssert` – Used for null validation.
- `System.Reflection` – Used to dynamically retrieve property information.

------------------------------------------------------------------------




# License

MIT License. See the LICENSE file in the project root for details.

------------------------------------------------------------------------




# Version History

## Next
- Added readme and license file to the project.

## 2.0.0
- Unified approach to IsNull and IsNotNull methods.
- Removed unnecessary paramterNames in validation methods.
- Added [CallerMemberName] attribute to make xssert error messages more meaningful.
- Fixed a typo in one of the error messages.

## 1.0.1
- Added unit tests.
- Added nuget configuration to publish package.

## 1.0.0
- Initial commit for the desired functionality in library.

------------------------------------------------------------------------