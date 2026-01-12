using System;
using System.Reflection;

namespace DbApiBuilderEntityGenerator.Core.Extensions;

public static class AssemblyExtensions
{
  public static async Task<string> ReadResourceAsync(this Assembly assembly, string name)
  {
    // Determine path
    string resourcePath; // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
    resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

    using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
    using StreamReader reader = new(stream);
    return await reader.ReadToEndAsync();
  }
}
