using System;
using DbApiBuilderEntityGenerator.Core.Metadata.Generation;
using DbApiBuilderEntityGenerator.Core.Options;

namespace DbApiBuilderEntityGenerator.Core.Scripts;

public class ContextScriptVariables : ScriptVariablesBase
{
  public ContextScriptVariables(EntityContext entityContext, GeneratorOptions generatorOptions, TemplateOptions templateOptions)
    : base(generatorOptions, templateOptions)
  {
    EntityContext = entityContext ?? throw new ArgumentNullException(nameof(entityContext));
  }

  public EntityContext EntityContext { get; }
}
