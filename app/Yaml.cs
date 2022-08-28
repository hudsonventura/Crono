using Crono;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Yaml
{
    private static dynamic Parse(string contents) {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<dynamic>(contents);
    }

    public static List<Container> loadFromFile(string filePath){
        string text = System.IO.File.ReadAllText(@$"docker-compose.yml");
        var yaml = Parse(text);

        List<Container> lista = new List<Container>();
        foreach (var servicoArray in yaml)
        {
            if (servicoArray.Key.ToString() == "services")
            {
                ///var servico = servicoArray.Key;
                var servicoObj = servicoArray.Value;
                foreach (var propsArray in servicoObj)
                {
                    var servico = propsArray.Key;
                    var props = propsArray.Value;

                    try
                    {
                        var validate = props["labels"];
                        var test = validate["crono"];
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    var scheduling = props["labels"];

                    lista.Add(new Container()
                    {
                        serviceName = servico,
                        scheduling = scheduling["crono"]
                    }); ;
                    //Console.WriteLine($"Serviço: {servico} programação - {programação}");

                }
            }
        }
        return lista;
    }
}