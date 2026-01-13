using System;
using System.Reflection;
using DbApiBuilderEntityGenerator.Core.Extensions;
using DbApiBuilderEntityGenerator.Core.Options;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;
using ScriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions;
namespace DbApiBuilderEntityGenerator.Core.Scripts;

public abstract class ScriptTemplateBase<TVariable> where TVariable : ScriptVariablesBase
{

  private Script<string>? _scriptTemplate;

  protected ScriptTemplateBase(ILoggerFactory loggerFactory, GeneratorOptions generatorOptions, TemplateOptions templateOptions)
  {
    Logger = loggerFactory.CreateLogger(this.GetType());
    TemplateOptions = templateOptions;
    GeneratorOptions = generatorOptions;
  }

  protected ILogger Logger { get; }

  public TemplateOptions TemplateOptions { get; }

  public GeneratorOptions GeneratorOptions { get; }
  protected abstract TVariable CreateVariables();
  protected virtual void WriteCode()
  {
    var templatePath = TemplateOptions.TemplatePath;

    if (!templatePath.IsNullOrWhiteSpace() && !File.Exists(templatePath))
    {
      Logger.LogWarning("Template '{template}' could not be found.", templatePath);
      return;
    }

    // save file
    var directory = TemplateOptions.Directory;
    var fileName = TemplateOptions.FileName;

    if (directory.IsNullOrEmpty() || fileName.IsNullOrEmpty())
    {
      Logger.LogWarning("Template could not resolve output file.");
      return;
    }

    var path = Path.Combine(directory, fileName);
    var exists = File.Exists(path);

    if (File.Exists(path))
      Logger.LogInformation("Updating template script file: {fileName}", fileName);
    else
      Logger.LogInformation("Creating template script file: {fileName}", fileName);

    // get content
    var content = ExecuteScript();

    if (content.IsNullOrWhiteSpace())
    {
      Logger.LogDebug("Skipping template because it didn't return any text.");
      return;
    }

    File.WriteAllText(path, content);
  }
  protected virtual string ExecuteScript()
  {
    var templatePath = TemplateOptions.TemplatePath;
    if (!templatePath.IsNullOrWhiteSpace() && !File.Exists(templatePath))
    {
      Logger.LogWarning("Template '{template}' could not be found.", templatePath);
      return string.Empty;
    }

    var script = LoadScript(templatePath);
    var variables = CreateVariables();

    var scriptTask = script.RunAsync(variables);
    var scriptState = scriptTask.Result;

    return scriptState.ReturnValue;
  }
  protected Script<string> LoadScript(string scriptPath)
  {
    if (_scriptTemplate != null)
      return _scriptTemplate;

    Logger.LogDebug("Loading template script: {script}", scriptPath);

    string scriptContent = String.Empty;

    if (scriptPath.IsNullOrEmpty())
    {
      scriptContent = Assembly.GetExecutingAssembly()
                 .ReadResourceAsync("DbApiBuilderEntityGenerator.Core.template.dab-config1.csx")
                 .Result;

    }
    else
    {
      scriptContent = File.ReadAllText(scriptPath);
    }

    var scriptOptions = ScriptOptions.Default
        .WithReferences(
            typeof(ScriptVariablesBase).Assembly
        )
        .WithImports(
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "DbApiBuilderEntityGenerator.Core.Extensions",
            "DbApiBuilderEntityGenerator.Core.Metadata.Generation",
            "DbApiBuilderEntityGenerator.Core.Options",
            "Microsoft.EntityFrameworkCore.Internal"
        );

    _scriptTemplate = CSharpScript.Create<string>(scriptContent, scriptOptions, typeof(TVariable));
    var diagnostics = _scriptTemplate.Compile();

    if (diagnostics.Length == 0)
      return _scriptTemplate;

    Logger.LogInformation("Template Compile Diagnostics: ");
    foreach (var diagnostic in diagnostics)
    {
      var message = diagnostic.GetMessage();
      switch (diagnostic.Severity)
      {
        case DiagnosticSeverity.Info:
          Logger.LogDebug(message);
          break;
        case DiagnosticSeverity.Warning:
          Logger.LogWarning(message);
          break;
        case DiagnosticSeverity.Error:
          Logger.LogError(message);
          break;
        default:
          Logger.LogDebug(message);
          break;
      }
    }

    return _scriptTemplate;
  }
}


