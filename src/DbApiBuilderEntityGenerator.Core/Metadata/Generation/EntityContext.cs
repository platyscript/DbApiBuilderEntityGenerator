using System;

namespace DbApiBuilderEntityGenerator.Core.Metadata.Generation;

public class EntityContext : ModelBase
{
  public EntityContext()
  {
    Entities = [];
  }

  public string ContextNamespace { get; set; } = null!;

  public string ContextClass { get; set; } = null!;

  public string? ContextBaseClass { get; set; }

  public string? DatabaseName { get; set; }

  public EntityCollection Entities { get; set; }
}
