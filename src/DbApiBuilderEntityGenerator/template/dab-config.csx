public string WriteCode()
{
    CodeBuilder.Clear();
    foreach (var entity in EntityContext.Entities)
    {
        CodeBuilder.Append("EntityClass: ").Append(entity.EntityClass.ToSafeName()).AppendLine();

        CodeBuilder.Append("ContextProperty: ").Append(entity.ContextProperty.ToSafeName()).AppendLine();

        CodeBuilder.Append("TableSchema: '").Append(entity.TableSchema.ToSafeName()).AppendLine("'");
        CodeBuilder.Append("TableName: '").Append(entity.TableName.ToSafeName()).AppendLine("'");


        CodeBuilder.Append("IsView: ").Append(entity.IsView.ToString()).AppendLine();

        CodeBuilder.Append("Properties:").AppendLine();
        using (CodeBuilder.Indent())
            GenerateProperties(entity);
    }

    return CodeBuilder.ToString();
}

private void GenerateProperties(Entity entity)
{
    foreach (var property in entity.Properties)
    {
        CodeBuilder.Append("- PropertyName: ").Append(property.PropertyName.ToSafeName()).AppendLine();
        CodeBuilder.Append("  ColumnName: '").Append(property.ColumnName.ToSafeName()).AppendLine("'");
        CodeBuilder.Append("  StoreType: ").Append(property.StoreType.ToSafeName()).AppendLine();
        CodeBuilder.Append("  NativeType: '").Append(property.NativeType.ToSafeName()).AppendLine("'");
        CodeBuilder.Append("  DataType: ").Append(property.DataType.ToString()).AppendLine();
        CodeBuilder.Append("  SystemType: ").Append(property.SystemType.Name).AppendLine();

        if (property.Size != null)
            CodeBuilder.Append("  Size: ").Append(property.Size.ToString()).AppendLine();

        if (property.Default != null)
            CodeBuilder.Append("  Default: '").Append(property.Default.ToSafeName()).AppendLine("'");

        if (property.ValueGenerated != null)
            CodeBuilder.Append("  ValueGenerated: ").Append(property.ValueGenerated.ToString()).AppendLine();

        if (property.IsNullable != null)
            CodeBuilder.Append("  IsNullable: ").Append(property.IsNullable.ToString()).AppendLine();

        if (property.IsPrimaryKey != null)
            CodeBuilder.Append("  IsPrimaryKey: ").Append(property.IsPrimaryKey.ToString()).AppendLine();

        if (property.IsForeignKey != null)
            CodeBuilder.Append("  IsForeignKey: ").Append(property.IsForeignKey.ToString()).AppendLine();

        if (property.IsReadOnly != null)
            CodeBuilder.Append("  IsReadOnly: ").Append(property.IsReadOnly.ToString()).AppendLine();

        if (property.IsRowVersion != null)
            CodeBuilder.Append("  IsRowVersion: ").Append(property.IsRowVersion.ToString()).AppendLine();

        if (property.IsUnique != null)
            CodeBuilder.Append("  IsUnique: ").Append(property.IsUnique.ToString()).AppendLine();
    }
}

// run script
WriteCode()
