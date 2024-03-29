﻿using Crono;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

string timezone = "";

try
{
    setTimezone();
}
catch (Exception)
{
    Console.WriteLine("You did not specify the timezone, so I get the timezone from the container. If you want, put some thing like below in your docker-compose file in crono.");
    Console.WriteLine("environment:");
    Console.WriteLine("\t- TIMEZONE=America/Sao_Paulo");
}

try
{

    int wait = Int16.Parse(Environment.GetEnvironmentVariable("WAIT"));
    System.Console.WriteLine($"Waiting {wait} seconds to all containers be up");
    Thread.Sleep(wait * 1000);
}
catch (Exception)
{

}

//RUN JUST ONE TIME
List<Container> containers = Yaml.loadFromFile(@$"docker-compose.yml");
if (containers.Count == 0)
{
    Console.WriteLine("I did not read any container with label crono. Please configure some below:");
    Console.WriteLine("labels:");
    Console.WriteLine("\tcrono: \"* * * * *\"");
    return;
}

Console.WriteLine("I read:");
foreach (Container container in containers)
{
    Console.WriteLine($"\t{container.serviceName}: {container.scheduling}");
}

Console.WriteLine("Starting the work");


//LOOP
while (true)
{
    if (DateTime.Now.Minute % 10 == 0 )
    {
        Console.WriteLine("Don't worry. I'm working here!");
    }
    
    Task.Run(() => start());
    Thread.Sleep(1000);
    GC.Collect();

    while (DateTime.Now.Second != 0)
    {
        Thread.Sleep(100);
    }
}

void setTimezone()
{
    string localTZ = Environment.GetEnvironmentVariable("TIMEZONE");
    if (localTZ == null || localTZ == "")
    {
        throw new Exception();
    }

    if (localTZ != timezone)
    {
        timezone = localTZ;
        System.Console.WriteLine($"Setting timezone {timezone}");
        ExternalApp.run("ln", $"-sf /usr/share/zoneinfo/{timezone} /etc/localtime");
        Console.WriteLine($"Set timezone {timezone}");
    }
}




void start() {
    List<Container> containers = Yaml.loadFromFile(@$"docker-compose.yml");

    containers = LinuxCron.ajustString(containers);
    try
    {
        var runningContainers = Docker.getContainersCreated();
        try
        {
            containers = Docker.getContainersID(containers, runningContainers);
        }
        catch (Exception error2)
        {
            Console.WriteLine($"Error on get ID of a specific running container. Error: {error2.Message}", Console.typeMessage.FAIL);
        }
       
    }
    catch (Exception error)
    {
        Console.WriteLine($"Error on get running containers. Error: {error.Message}", Console.typeMessage.FAIL);
    }
    

    foreach (var container in containers)
    {
        var atuate = LinuxCron.Bind(container);
        if (atuate)
        {
            //Console.WriteLine($"Restarting service: {container.serviceName} by schedulling {container.scheduling} -> {container.containerID}");
            Console.WriteLine($"Service {container.serviceName}: Trying to restart");
            try
            {
                Task.Run(() => Docker.operateContainer(container, "restart"));
            }
            catch (Exception error)
            {
                Console.WriteLine($"Service {container.serviceName}: {error.Message}", Console.typeMessage.FAIL);
            }
        }
    }
}