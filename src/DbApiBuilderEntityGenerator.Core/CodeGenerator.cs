using System;
using DbApiBuilderEntityGenerator.Core.Extensions;
using DbApiBuilderEntityGenerator.Core.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbApiBuilderEntityGenerator.Core;

public class CodeGenerator : ICodeGenerator
{
  private readonly ILoggerFactory _loggerFactory;
  private readonly ILogger _logger;
  private readonly ModelGenerator _modelGenerator;

  public CodeGenerator(ILoggerFactory loggerFactory)
  {
    _loggerFactory = loggerFactory;
    _logger = loggerFactory.CreateLogger<CodeGenerator>();
    _modelGenerator = new ModelGenerator(loggerFactory);
    // _synchronizer = new SourceSynchronizer(loggerFactory);
  }

  public GeneratorOptions Options { get; set; } = null!;
  public bool Generate(GeneratorOptions options)
  {
    Options = options ?? throw new ArgumentNullException(nameof(options));

    var databaseProviders = GetDatabaseProviders();
    var databaseModel = GetDatabaseModel(databaseProviders.factory);
    if (databaseModel == null)
      throw new InvalidOperationException("Failed to create database model");

    _logger.LogInformation("Loaded database model for: {databaseName}", databaseModel.DatabaseName);

    var context = _modelGenerator.Generate(Options, databaseModel, databaseProviders.mapping);

    return true;
  }
  private DatabaseModel GetDatabaseModel(IDatabaseModelFactory factory)
  {
    _logger.LogInformation("Loading database model ...");


    var connectionString = ResolveConnectionString(Options.ConnectionString, Options.UserSecretsId, Options.ConnectionName);
    if (string.IsNullOrEmpty(connectionString))
      throw new InvalidOperationException("Could not find connection string.");

    var options = new DatabaseModelFactoryOptions(Options.Tables, Options.Schemas);

    return factory.Create(connectionString, options);
  }

  private static string? ResolveConnectionString(string? connectionString,
   string? userSecretsId,
   string? connectionName)
  {
    if (connectionString.HasValue())
      return connectionString;

    if (userSecretsId.HasValue() && connectionName.HasValue())
    {
      var secretsStore = new SecretsStore(userSecretsId);
      if (secretsStore.ContainsKey(connectionName))
        return secretsStore[connectionName];
    }

    throw new InvalidOperationException("Could not find connection string.");
  }

  private (IDatabaseModelFactory factory, IRelationalTypeMappingSource mapping) GetDatabaseProviders()
  {
    var provider = Options.Provider;

    _logger.LogDebug("Creating database model factory for: {provider}", provider);

    // start a new service container to create the database model factory
    var services = new ServiceCollection()
        .AddSingleton(_loggerFactory)
        .AddEntityFrameworkDesignTimeServices();

    switch (provider)
    {
      case DatabaseProviders.SqlServer:
        ConfigureSqlServerServices(services);
        break;
      case DatabaseProviders.PostgreSQL:
        ConfigurePostgresServices(services);
        break;
      case DatabaseProviders.MySQL:
        ConfigureMySqlServices(services);
        break;
      case DatabaseProviders.Sqlite:
        ConfigureSqliteServices(services);
        break;
      case DatabaseProviders.Oracle:
        ConfigureOracleServices(services);
        break;
      default:
        throw new NotSupportedException($"The specified provider '{provider}' is not supported.");
    }

    var serviceProvider = services
        .BuildServiceProvider();

    var databaseModelFactory = serviceProvider
        .GetRequiredService<IDatabaseModelFactory>();

    var typeMappingSource = serviceProvider
        .GetRequiredService<IRelationalTypeMappingSource>();

    return (databaseModelFactory, typeMappingSource);
  }
  private static void ConfigureMySqlServices(IServiceCollection services)
  {
    var designTimeServices = new Pomelo.EntityFrameworkCore.MySql.Design.Internal.MySqlDesignTimeServices();
    designTimeServices.ConfigureDesignTimeServices(services);
    services.AddEntityFrameworkMySqlNetTopologySuite();
  }

  private static void ConfigurePostgresServices(IServiceCollection services)
  {
    var designTimeServices = new Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal.NpgsqlDesignTimeServices();
    designTimeServices.ConfigureDesignTimeServices(services);
    services.AddEntityFrameworkNpgsqlNetTopologySuite();
  }

  private static void ConfigureSqlServerServices(IServiceCollection services)
  {
    var designTimeServices = new Microsoft.EntityFrameworkCore.SqlServer.Design.Internal.SqlServerDesignTimeServices();
    designTimeServices.ConfigureDesignTimeServices(services);
    services.AddEntityFrameworkSqlServerNetTopologySuite();
  }

  private static void ConfigureSqliteServices(IServiceCollection services)
  {
    var designTimeServices = new Microsoft.EntityFrameworkCore.Sqlite.Design.Internal.SqliteDesignTimeServices();
    designTimeServices.ConfigureDesignTimeServices(services);
    services.AddEntityFrameworkSqliteNetTopologySuite();
  }

  private static void ConfigureOracleServices(IServiceCollection services)
  {
    var designTimeServices = new Oracle.EntityFrameworkCore.Design.Internal.OracleDesignTimeServices();
    designTimeServices.ConfigureDesignTimeServices(services);
  }
}
