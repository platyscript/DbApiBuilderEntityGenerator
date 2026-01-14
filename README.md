# Data API Builder Entity generator

This is a dotnet tool generates the [data-api-builder](https://learn.microsoft.com/en-us/azure/data-api-builder/) data-api-builder configuration file with the entites from an existing database.

** This is currently alpha release **

## Installation
`dotnet tool install --global DbApiBuilderEntityGenerator --version 1.0.3-alpha`
## Usage

### Command

`dab-entity-gen -p SqlServer -c Data Source=localhost;Initial Catalog=pubs;User ID=sa;Password=Passw0rd1;Encrypt=False`

### Configuration file

Sample Configuration file:

```yaml
# the connection string to the database
connectionString: "Data Source=(local);Initial Catalog=Tracker;Integrated Security=True"

# the database provider name. Default:SqlServer
provider: SqlServer

# config name to read the connection string from the user secrets file
connectionName: "ConnectionStrings:Generator"

# the user secret identifier, can be shared with .net core project
userSecretsId: "984ef0cf-2b22-4fd1-876d-e01499da4c1f"

# the directory to output the configuration. Default is current.
outputDirectory: .\

# tables to include or empty to include all
tables:
  - Priority
  - Status
  - Task
  - User

# schemas to include or empty to include all
schemas:
  - dbo

# exclude tables or columns
exclude:
  # list of expressions for tables to exclude, source is Schema.TableName
  tables:
    - exact: dbo.SchemaVersions
    - regex: dbo\.SchemaVersions$
  # list of expressions for columns to exclude, source is Schema.TableName.ColumnName
  columns:
    - exact: dbo.SchemaVersions\.Version
    - regex: dbo\.SchemaVersions\.Version$

# how to generate entity class names from the table name. Preserve|Plural|Singular. Default: Singular
entityNaming: Singular

# how to generate relationship collections names for the entity.  Default: Plural
relationshipNaming: Plural

# Rename entities and properties with regular expressions.  Matched expressions will be removed.
renaming:
  entities:
    - ^(sp|tbl|udf|vw)_
  properties:
    - ^{Table.Name}(?=Id|Name)

#templateFilePath: ./template/dab-config1.csx 

```

## Sample Configuration

Sample configuration file:- [Sample.yaml](https://github.com/platyscript/DbApiBuilderEntityGenerator/blob/update-readme/sample.yaml)

## Sample template file

Sample template file:- [db-config.csx](https://github.com/platyscript/DbApiBuilderEntityGenerator/blob/update-readme/src/DbApiBuilderEntityGenerator.Core/template/dab-config.csx)

## Work in Progress

- Permissions object is currently hardcoded. Need to get it from the database
- Change template from csx to liquid.
- Add an `init` command to generate an initial configuration yaml file.
- Develop a UI to update configuratio json file once generated.