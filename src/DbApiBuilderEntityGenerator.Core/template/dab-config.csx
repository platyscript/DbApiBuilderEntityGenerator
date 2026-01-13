public string WriteCode()
{
    CodeBuilder.Clear();
    // start entities object
    CodeBuilder.AppendLine("\"entities\": {");
    foreach (var entity in EntityContext.Entities)
    {
        using (CodeBuilder.Indent())
        {
            // start entity object
            CodeBuilder.Append("\"").Append(entity.EntityClass.ToSafeName()).Append("\": {").AppendLine();

            using (CodeBuilder.Indent())
            {
                // start source object
                CodeBuilder.Append("\"source\": {").AppendLine();
                CodeBuilder.Append("\"object\": \"").Append(entity.TableSchema.ToSafeName()).Append(".").Append(entity.TableName.ToSafeName()).Append("\",").AppendLine();
                if (entity.IsView)
                {
                    CodeBuilder.Append("\"type\": \"view\"").AppendLine();
                }
                else
                {
                    CodeBuilder.Append("\"type\": \"table\"").AppendLine();
                }

                // end source object
                CodeBuilder.Append("},").AppendLine();
            }

            using (CodeBuilder.Indent())
            {
                // start mappings object 
                CodeBuilder.Append("\"mappings\": {").AppendLine();

                var propertyCount = entity.Properties.Count - 1;
                int count = 0;

                foreach (var property in entity.Properties)
                {
                    CodeBuilder.Append("\"").Append(property.ColumnName.ToSafeName()).Append("\": ");
                    if (count == propertyCount)
                    {
                        CodeBuilder.Append("\"").Append(property.PropertyName.ToSafeName()).Append("\"").AppendLine();
                    }
                    else
                    {
                        CodeBuilder.Append("\"").Append(property.PropertyName.ToSafeName()).Append("\",").AppendLine();
                    }
                    count++;
                }
                // end mappings object 
                CodeBuilder.Append("},").AppendLine();
            }

            using (CodeBuilder.Indent())
            {
                // start relationships object 
                CodeBuilder.Append("\"relationships\": {").AppendLine();
                var relCount = 0;
                var relationshipCount = entity.Relationships.Count - 1;
                using (CodeBuilder.Indent())
                {
                    foreach (var relationship in entity.Relationships)
                    {
                        CodeBuilder.Append("\"").Append(relationship.RelationshipName.ToSafeName()).Append("\": {").AppendLine();
                        using (CodeBuilder.Indent())
                        {
                            CodeBuilder.Append("\"cardinality\": \"").Append(relationship.Cardinality.ToString().ToSafeName()).Append("\",").AppendLine();
                            CodeBuilder.Append("\"target.entity\": \"").Append(relationship.PrimaryEntity.EntityClass.ToSafeName().ToSafeName()).Append("\"").AppendLine();
                        }
                        CodeBuilder.Append("}");
                        if (relCount != relationshipCount)
                        {
                            CodeBuilder.Append(",");
                        }
                        relCount++;
                        CodeBuilder.AppendLine();
                    }
                }
                // end relationships object 
                CodeBuilder.Append("},").AppendLine();
            }

            using (CodeBuilder.Indent())
            {
                // start permissions object
                CodeBuilder.Append("\"permissions\": [").AppendLine();
                CodeBuilder.Append("{").AppendLine();
                using (CodeBuilder.Indent())
                {
                    CodeBuilder.Append("\"role\": \"anonymous\",").AppendLine();
                    CodeBuilder.Append("\"actions\": [").AppendLine();
                    CodeBuilder.Append("{").AppendLine();
                    using (CodeBuilder.Indent())
                    {
                        CodeBuilder.Append("\"action\": \"*\"").AppendLine();
                    }
                    CodeBuilder.Append("}]").AppendLine();

                }
                CodeBuilder.Append("}]").AppendLine();
                // end permissions object
            }
            // end entity object
            CodeBuilder.Append("},").AppendLine();
        }
    }
    // end entities object
    CodeBuilder.Append("}").AppendLine();
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
