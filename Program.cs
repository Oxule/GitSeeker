using System.Text;
using GitSeeker;
using GitSeeker.GitObjects;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var config = deserializer.Deserialize<Configuration>(File.ReadAllText("config.yml"));

var loop = new Thread(()=>Seeker.Loop(config));
loop.Start();
Console.ReadLine();

return;

IGitProvider git = new LocalGitProvider("C:\\Projects\\GitSeeker\\.git");

var c = git.GetCommit(git.GetBranches()[0].LastCommit);
Console.WriteLine(c.Message);
foreach (var obj in git.GetTreeRecursive(c.Tree))
{
    var ch = obj.directory ? "D" : "F";
    Console.WriteLine($"({ch}){obj.path} - {obj.hash}");
}

return;
var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var yaml = serializer.Serialize(Configuration.Sample);
File.WriteAllText("config.yml", yaml);