using Crono;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

bool firstRun = true;
string timezone = "";

try
{
    int wait = Int16.Parse(Environment.GetEnvironmentVariable("WAIT"));
    System.Console.WriteLine($"Waiting {wait} seconds to all containers be up");
    Thread.Sleep(wait * 1000);
}
catch (Exception)
{

}

while (true)
{
    
    try
    {
        string localTZ = Environment.GetEnvironmentVariable("TIMEZONE");
        if (localTZ == null || localTZ == "")
        {
            throw new Exception();
        }

        if (localTZ != timezone) {
            timezone = localTZ;
            System.Console.WriteLine($"Setting timezone {timezone}");
            ExternalApp.run("ln", $"-sf /usr/share/zoneinfo/{timezone} /etc/localtime");
            Console.WriteLine($"Set timezone {timezone}");
        }
        
        
        
    }
    catch (Exception)
    {
        if (firstRun)
        {
            Console.WriteLine("You did not specify the timezone. If you want, put some thing like below in your docker-compose file.");
            Console.WriteLine("environment:");
            Console.WriteLine("\t- TIMEZONE=America/Sao_Paulo");
            firstRun = false;
        }
        
    }
    Console.WriteLine("Starting");
    Task.Run(() => start());
    Thread.Sleep(1000);
    GC.Collect();

    while (DateTime.Now.Second != 0)
    {
        Thread.Sleep(100);
    }

}

void start() {
    List<Container> containers = Yaml.loadFromFile(@$"docker-compose.yml");

    containers = LinuxCron.ajustString(containers);
    try
    {
        var runningContainers = Docker.getContainersCreated();
        containers = Docker.getContainersID(containers, runningContainers);
    }
    catch (Exception error)
    {
        Console.WriteLine($"Erro on get running container. Error: {error.Message}", Console.typeMessage.FAIL);
    }
    

    foreach (var container in containers)
    {
        var atuate = LinuxCron.Bind(container);
        if (atuate)
        {
            Console.WriteLine($"Restarting service: {container.serviceName}, {container.scheduling} -> {container.containerID}");
            try
            {
                Task.Run(() => Docker.operateContainer(container, "restart"));
            }
            catch (Exception error)
            {
                Console.WriteLine($"Fail to restart the container {container.serviceName}. Error: {error.Message}", Console.typeMessage.FAIL);
            }
        }
    }
}