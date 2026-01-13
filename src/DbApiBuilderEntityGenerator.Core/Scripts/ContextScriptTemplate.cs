using System;
using DbApiBuilderEntityGenerator.Core.Metadata.Generation;
using DbApiBuilderEntityGenerator.Core.Options;
using Microsoft.Extensions.Logging;

namespace DbApiBuilderEntityGenerator.Core.Scripts;

public class ContextScriptTemplate : ScriptTemplateBase<ContextScriptVariables>
{
  private EntityContext _entityContext = null!;

  public ContextScriptTemplate(ILoggerFactory loggerFactory, GeneratorOptions generatorOptions, TemplateOptions templateOptions)
      : base(loggerFactory, generatorOptions, templateOptions)
  {
  }

  public void RunScript(EntityContext entityContext)
  {
    ArgumentNullException.ThrowIfNull(entityContext);

    _entityContext = entityContext;

    WriteCode();
  }

  protected override ContextScriptVariables CreateVariables()
  {
    return new ContextScriptVariables(_entityContext, GeneratorOptions, TemplateOptions);
  }
}
