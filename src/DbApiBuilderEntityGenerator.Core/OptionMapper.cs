using System;
using DbApiBuilderEntityGenerator.Core.Options;
using DbApiBuilderEntityGenerator.Core.Serialization;

namespace DbApiBuilderEntityGenerator.Core;

public static class OptionMapper
{
  public static GeneratorOptions Map(GeneratorModel generator)
  {
    var options = new GeneratorOptions();
    options.Variables.ShouldEvaluate = false;
    options.ConnectionName = generator.ConnectionName;
    options.ConnectionString = generator.ConnectionString;
    options.ConnectionName = generator.ConnectionName;
    options.UserSecretsId = generator.UserSecretsId;
    MapList(options.Tables, generator.Tables);
    MapList(options.Schemas, generator.Schemas);
    MapDatabaseMatch(options.Exclude, generator.Exclude);
    options.EntityNaming = generator.EntityNaming;
    options.RelationshipNaming = generator.RelationshipNaming;
    MapSelection(options.Renaming, generator.Renaming);

    options.Variables.ShouldEvaluate = true;

    return options;
  }
  private static void MapSelection(SelectionOptions option, SelectionModel? selection)
  {
    if (selection == null)
      return;

    MapList(option.Entities, selection.Entities, (match) =>
    {
      var prefix = OptionsBase.AppendPrefix(option.Prefix, $"Entity{option.Entities.Count:0000}");
      return MapMatch(option.Variables, match, prefix);
    });

    MapList(option.Properties, selection.Properties, (match) =>
    {
      var prefix = OptionsBase.AppendPrefix(option.Prefix, $"Property{option.Properties.Count:0000}");
      return MapMatch(option.Variables, match, prefix);
    });
  }
  private static void MapList<T>(IList<T> targetList, IList<T>? sourceList)
  {
    if (sourceList == null || sourceList.Count == 0)
      return;

    foreach (var source in sourceList)
      targetList.Add(source);
  }

  private static void MapList<TTarget, TSource>(IList<TTarget> targetList, IList<TSource>? sourceList, Func<TSource, TTarget> factory)
  {
    if (sourceList == null || sourceList.Count == 0)
      return;

    foreach (var source in sourceList)
    {
      var target = factory(source);
      targetList.Add(target);
    }
  }

  private static MatchOptions MapMatch(VariableDictionary variables, MatchModel match, string? prefix)
  {
    return new MatchOptions()
    {
      Exact = match.Exact,
      Expression = match.Expression
    };
  }

  private static void MapDatabaseMatch(DatabaseMatchOptions option, DatabaseMatchModel? match)
  {
    if (match == null)
      return;

    MapList(option.Tables, match.Tables, (match) =>
    {
      var prefix = OptionsBase.AppendPrefix(option.Prefix, $"Table{option.Tables?.Count:0000}");
      return MapMatch(option.Variables, match, prefix);
    });

    MapList(option.Columns, match.Columns, (match) =>
    {
      var prefix = OptionsBase.AppendPrefix(option.Prefix, $"Column{option.Columns?.Count:0000}");
      return MapMatch(option.Variables, match, prefix);
    });
  }
}
