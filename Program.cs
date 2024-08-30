using GitSeeker;
using Newtonsoft.Json;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

DefaultActions.AddDefaultActions();

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var config = deserializer.Deserialize<Configuration>(File.ReadAllText("config.yml"));

new Thread(()=>Seeker.Loop(config)).Start();

Console.ReadLine();