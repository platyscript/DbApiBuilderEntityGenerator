using System;
using DbApiBuilderEntityGenerator.Core;
using DbApiBuilderEntityGenerator.Core.Extensions;
using DbApiBuilderEntityGenerator.Core.Options;
using DbApiBuilderEntityGenerator.Core.Serialization;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace DbApiBuilderEntityGenerator;

[Command("generate", "gen")]
public class GenerateCommand : OptionsCommandBase
{
  private readonly ICodeGenerator _codeGenerator;

  public GenerateCommand(ILoggerFactory logger, IConsole console, IConfigurationSerializer serializer, ICodeGenerator codeGenerator) : base(logger, console, serializer)
  {
    this._codeGenerator = codeGenerator;
  }

  [Option("-p <Provider>", Description = "Database provider to reverse engineer")]
  public DatabaseProviders? Provider { get; set; }

  [Option("-c <ConnectionString>", Description = "Database connection string to reverse engineer")]
  public string? ConnectionString { get; set; }


  protected override int OnExecute(CommandLineApplication application)
  {
    var workingDirectory = OutputDirectory ?? Environment.CurrentDirectory;
    var configurationFile = OptionsFile ?? ConfigurationSerializer.OptionsFileName;

    var configuration = Serializer.Load(workingDirectory, configurationFile);
    if (configuration == null)
    {
      Logger.LogInformation("Using default options");
      configuration = new GeneratorModel();
    }

    // override options
    if (ConnectionString.HasValue())
      configuration.ConnectionString = ConnectionString;

    if (Provider.HasValue)
      configuration.Provider = Provider.Value;


    // convert to options format to support variables
    var options = OptionMapper.Map(configuration);

    var result = _codeGenerator.Generate(options);

    return 0;

  }
}
