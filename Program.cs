using System.Text;
using GitSeeker;
using GitSeeker.GitObjects;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

IGitProvider git = new LocalGitProvider("C:\\Projects\\OxyCaptcha\\.git");

var c = git.GetCommit(git.GetBranches()[0].LastCommit);
Console.WriteLine(c.Message);
foreach (var obj in git.GetTree(c.Tree).Objects)
{
    var ch = obj.Directory ? "D" : "F";
    Console.WriteLine($"({ch}){obj.FileName} - {obj.Hash}");
}

return;
var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var yaml = serializer.Serialize(Configuration.Sample);
File.WriteAllText("config.yml", yaml);