using System;
using System.ComponentModel;
using DbApiBuilderEntityGenerator.Core.Options;

namespace DbApiBuilderEntityGenerator.Core.Serialization;

/// <summary>
/// Generator Options
/// </summary>
public class GeneratorModel
{

  public GeneratorModel()
  {
    RelationshipNaming = RelationshipNaming.Plural;
    EntityNaming = EntityNaming.Singular;
  }

  /// <summary>
  /// Gets or sets the project directory.
  /// </summary>
  /// <value>
  /// The project directory.
  /// </value>
  public string? OutputDirectory { get; set; }

  /// <summary>
  /// Gets or sets the database to generate code for.
  /// </summary>
  /// <value>
  /// The database to generate code for.
  /// </value>
  [DefaultValue(DatabaseProviders.SqlServer)]
  public DatabaseProviders Provider { get; set; } = DatabaseProviders.SqlServer;

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
  public List<string>? Tables { get; set; }

  /// <summary>
  /// Gets or sets the schema to include in the model, or an empty enumerable to include all.
  /// </summary>
  /// <value>
  /// The schema to include in the model, or an empty enumerable to include all.
  /// </value>
  public List<string>? Schemas { get; set; }

  /// <summary>
  /// Gets or sets the model that specifies which elements should be ignored during processing.
  /// </summary>
  public DatabaseMatchModel? Exclude { get; set; }

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
  public SelectionModel? Renaming { get; set; }


  /// <summary>
  /// Gets or sets the renaming expressions.
  /// </summary>
  /// <value>
  /// The renaming expressions.
  /// </value>
  public string? OutputFileName { get; set; }

  public string? TemplateFilePath { get; set; }
}

