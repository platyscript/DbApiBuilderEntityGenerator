using System;
using DbApiBuilderEntityGenerator.Core.Options;

namespace DbApiBuilderEntityGenerator.Core;

public interface ICodeGenerator
{
  bool Generate(GeneratorOptions options);
}
