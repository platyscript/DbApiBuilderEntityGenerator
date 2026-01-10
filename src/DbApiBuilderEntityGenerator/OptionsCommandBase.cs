using System;
using System.IdentityModel.Tokens.Jwt;
using DbApiBuilderEntityGenerator.Core;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace DbApiBuilderEntityGenerator;

public abstract class OptionsCommandBase : CommandBase
{
  protected OptionsCommandBase(ILoggerFactory logger, IConsole console, IConfigurationSerializer serializer) : base(logger, console)
  {
    this.Serializer = serializer;
  }

  protected IConfigurationSerializer Serializer { get; private set; }

  [Option("-d <outputDirectory>", Description = "The output directory")]
  public string OutputDirectory { get; set; } = Environment.CurrentDirectory;

  [Option("-f <file>", Description = "The options file name")]
  public string OptionsFile { get; set; } = ConfigurationSerializer.OptionsFileName;
}
