using System.ComponentModel;
using YamlDotNet.Serialization;

namespace DbApiBuilderEntityGenerator.Core.Options;

/// <summary>
/// Top level generator configuration options
/// </summary>
public class GeneratorOptions : OptionsBase
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GeneratorOptions"/> class.
  /// </summary>
  public GeneratorOptions()
  {
    Variables = new VariableDictionary();
    Provider = DatabaseProviders.SqlServer;
    Directory = @".\";
    Tables = [];
    Schemas = [];
    Exclude = new DatabaseMatchOptions();
    EntityNaming = EntityNaming.Singular;
    RelationshipNaming = RelationshipNaming.Plural;
    Renaming = new SelectionOptions();
  }

  [YamlIgnore]
  public VariableDictionary Variables { get; }

  /// <summary>
  /// Gets or sets the project directory.
  /// </summary>
  /// <value>
  /// The project directory.
  /// </value>
  public string? Directory
  {
    get => GetProperty();
    set => SetProperty(value);
  }

  /// <summary>
  /// Gets or sets the database to generate code for.
  /// </summary>
  /// <value>
  /// The database to generate code for.
  /// </value>
  [DefaultValue(DatabaseProviders.SqlServer)]
  public DatabaseProviders Provider { get; set; }


  /// <summary>
  /// Gets or sets the connection string for reverse engineering the database
  /// </summary>
  /// <value>
  /// The connection string for reverse engineering the database
  /// </value>
  public string? ConnectionString { get; set; }

  /// <summary>
  /// Gets or sets the name of the connection in the user secret file.
  /// </summary>
  /// <value>
  /// The name of the connection.
  /// </value>
  public string? ConnectionName { get; set; }

  /// <summary>
  /// Gets or sets the user secrets identifier. A user secrets ID is unique value used to store and identify a collection of secret configuration values.
  /// </summary>
  /// <value>
  /// The user secrets identifier.
  /// </value>
  public string? UserSecretsId { get; set; }

  /// <summary>
  /// Gets or sets the tables to include in the model, or an empty enumerable to include all
  /// </summary>
  /// <value>
  /// The tables to include in the model, or an empty enumerable to include all
  /// </value>
  public List<string> Tables { get; }

  /// <summary>
  /// Gets or sets the schema to include in the model, or an empty enumerable to include all.
  /// </summary>
  /// <value>
  /// The schema to include in the model, or an empty enumerable to include all.
  /// </value>
  public List<string> Schemas { get; }

  /// <summary>
  /// Gets or sets the exclude table options.
  /// </summary>
  /// <value>
  /// The exclude table options.
  /// </value>
  public DatabaseMatchOptions Exclude { get; }

  /// <summary>
  /// Gets or sets the entity class naming strategy.
  /// </summary>
  /// <value>
  /// The entity class naming strategy.
  /// </value>
  [DefaultValue(EntityNaming.Singular)]
  public EntityNaming EntityNaming { get; set; }

  /// <summary>
  /// Gets or sets the relationship property naming strategy.
  /// </summary>
  /// <value>
  /// The relationship property naming strategy.
  /// </value>
  [DefaultValue(RelationshipNaming.Plural)]
  public RelationshipNaming RelationshipNaming { get; set; }

  /// <summary>
  /// Gets or sets the renaming expressions.
  /// </summary>
  /// <value>
  /// The renaming expressions.
  /// </value>
  public SelectionOptions Renaming { get; }

  /// <summary>
  /// Gets or sets the renaming expressions.
  /// </summary>
  /// <value>
  /// The renaming expressions.
  /// </value>
  public string? OutputFileName { get; set; }

  public string? TemplateFilePath { get; set; }
}
