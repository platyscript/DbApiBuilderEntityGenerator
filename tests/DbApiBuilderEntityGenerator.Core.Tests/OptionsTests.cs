using System.Reflection;
using DbApiBuilderEntityGenerator.Core.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DbApiBuilderEntityGenerator.Core.Tests;

public class OptionsTests
{
    private readonly ITestOutputHelper _output;

    public OptionsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SaveDefault()
    {
        var generatorOptions = new GeneratorModel();
        // set user secret values
        generatorOptions.UserSecretsId = Guid.NewGuid().ToString();
        generatorOptions.ConnectionName = "ConnectionStrings:Generator";

        var serializer = new SerializerBuilder()
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

        var yaml = serializer.Serialize(generatorOptions);

        _output.WriteLine(yaml);
    }

    [Fact]
    public void Load()
    {
        var serializer = new ConfigurationSerializer(NullLogger<ConfigurationSerializer>.Instance);

        var resourcePath = "DbApiBuilderEntityGenerator.Core.Tests.Options.sample.yaml";
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var reader = new StreamReader(stream);

        var options = serializer.Load(reader);
        Assert.NotNull(options);
    }
}
