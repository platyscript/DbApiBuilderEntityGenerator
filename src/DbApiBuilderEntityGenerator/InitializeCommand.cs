using System;
using DbApiBuilderEntityGenerator.Core.Serialization;
using DbApiBuilderEntityGenerator.Core.Options;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using DbApiBuilderEntityGenerator.Core;
using DbApiBuilderEntityGenerator.Core.Extensions;

namespace DbApiBuilderEntityGenerator;

[Command("initialize", "init")]
public class InitializeCommand : OptionsCommandBase
{
  public InitializeCommand(ILoggerFactory logger, IConsole console, IConfigurationSerializer serializer) : base(logger, console, serializer)
  {
  }
  [Option("-p <Provider>", Description = "Database provider to reverse engineer")]
  public DatabaseProviders? Provider { get; set; }

  [Option("-c <ConnectionString>", Description = "Database connection string to reverse engineer")]
  public string? ConnectionString { get; set; }

  [Option("--id <UserSecretsId>", Description = "The user secret ID to use")]
  public string? UserSecretsId { get; set; }

  [Option("--name <ConnectionName>", Description = "The user secret configuration name")]
  public string? ConnectionName { get; set; }
  protected override int OnExecute(CommandLineApplication application)
  {
    var outputDirectory = OutputDirectory ?? Environment.CurrentDirectory;
    if (!Directory.Exists(outputDirectory))
    {
      Logger.LogTrace($"Creating directory: {outputDirectory}");
      Directory.CreateDirectory(outputDirectory);
    }

    var optionsFile = OptionsFile ?? ConfigurationSerializer.OptionsFileName;

    GeneratorModel? options = null;
    if (Serializer.Exists(outputDirectory, optionsFile))
      options = Serializer.Load(outputDirectory, optionsFile);
    if (options == null)
      options = CreateOptionsFile(optionsFile);

    if (UserSecretsId.HasValue())
      options.UserSecretsId = UserSecretsId;

    if (ConnectionName.HasValue())
      options.ConnectionName = ConnectionName;

    if (Provider.HasValue)
      options.Provider = Provider.Value;

    if (ConnectionString.HasValue())
    {
      if (UserSecretsId.HasValue())
        options = CreateUserSecret(options, ConnectionString);
      else
        options.ConnectionString = ConnectionString;
    }

    Serializer.Save(options, outputDirectory, optionsFile);

    return 0;

  }

  private GeneratorModel CreateUserSecret(GeneratorModel options, string connectionString)
  {
    if (options.UserSecretsId.IsNullOrWhiteSpace())
      options.UserSecretsId = Guid.NewGuid().ToString();

    if (options.ConnectionName.IsNullOrWhiteSpace())
      options.ConnectionName = "ConnectionStrings:Generator";

    Logger.LogInformation("Adding Connection String to User Secrets file");

    // save connection string to user secrets file
    var secretsStore = new SecretsStore(options.UserSecretsId);
    secretsStore.Set(options.ConnectionName, connectionString);
    secretsStore.Save();

    return options;
  }

  private GeneratorModel CreateOptionsFile(string optionsFile)
  {
    var options = new GeneratorModel();

    options.OutputDirectory = ".\\";

    Logger.LogInformation($"Creating options file: {optionsFile}");

    return options;
  }
}