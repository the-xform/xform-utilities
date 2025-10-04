using System.Reflection;
using XForm.Utilities;

namespace Xform.Utilities.Tests.Unit;

public class LocationUtilitiesTests
{
	[Fact]
	public void GetEntryAssemblyDirectory_ShouldReturnNonEmptyPath()
	{
		// Act
		string dir = LocationUtilities.GetEntryAssemblyDirectory();

		// Assert
		Assert.False(string.IsNullOrEmpty(dir));
	}

	[Fact]
	public void GetEntryAssemblyDirectory_ShouldReturnExistingDirectory()
	{
		// Act
		string dir = LocationUtilities.GetEntryAssemblyDirectory();

		// Assert
		Assert.True(Directory.Exists(dir), $"Directory '{dir}' does not exist");
	}

	[Fact]
	public void GetEntryAssemblyDirectory_ShouldReturnSameAsAssemblyLocationDirectory()
	{
		// Arrange
		var assembly = Assembly.GetEntryAssembly();
		var expected_dir = Path.GetDirectoryName(assembly?.Location ?? "");

		// Act
		string actualDir = LocationUtilities.GetEntryAssemblyDirectory();

		// Assert
		Assert.Equal(expected_dir, actualDir);
	}
}
