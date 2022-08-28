using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crono
{
    internal class Docker
    {
        /*
         * varre os container analisando quais deles possuem o agendamento de tarefas cron do arquivo docker-compose.yml (servicosYaml)
         */
        public static List<Container> getContainersID(List<Container> containers, JArray runningContainers)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                //Console.WriteLine("AQUI0! ->" + containers[0].serviceName);
                Container container = containers[i];
                for (int j = 0; j < runningContainers.Count; j++)
                {
                    JToken containerRunning = runningContainers[j];
                    string containerRunningName = containerRunning["Labels"]["com.docker.compose.service"].ToString();
                    string containerRunningID = containerRunning["Id"].ToString();

                    if (containerRunningName == container.serviceName)
                    {
                        containers[i].containerID = containerRunningID;
                    }

                }
            }
            return containers;
        }

        /*
         * consulta o docker buscando todos os containers de pé
         */
        public static JArray getContainersRunning()
        {
            string response = ExternalApp.run("curl", "--silent -XGET --unix-socket /var/run/docker.sock -H 'Content-Type: application/json' http://localhost/containers/json");
            //Console.WriteLine("AQUI1! ->"+ response);
            //string response = "[{\"Id\":\"edabb7ba81dbe2878427730e1b6b60f434e5619308e2e11a0389547abc7d3e81\",\"Names\":[\"/chrono_db_oracle_1\"],\"Image\":\"postgres:14\",\"ImageID\":\"sha256:b2261d3c6ce0b23bed32e7567a92646b880de73d802550b1275baa0997aa34d0\",\"Command\":\"docker-entrypoint.sh postgres\",\"Created\":1660159512,\"Ports\":[{\"PrivatePort\":5432,\"Type\":\"tcp\"}],\"Labels\":{\"com.docker.compose.config-hash\":\"61c6e34c90b0c00a3417266c79e817a93ac2d4fac7294ad7724f0322a7416890\",\"com.docker.compose.container-number\":\"1\",\"com.docker.compose.oneoff\":\"False\",\"com.docker.compose.project\":\"chrono\",\"com.docker.compose.project.config_files\":\"docker-compose.yml\",\"com.docker.compose.project.working_dir\":\"/mnt/c/Users/HUDSON~1.VEN/source/repos/Dropbox/Chrono\",\"com.docker.compose.service\":\"db_oracle\",\"com.docker.compose.version\":\"1.29.2\"},\"State\":\"running\",\"Status\":\"Up Less than a second\",\"HostConfig\":{\"NetworkMode\":\"chrono_default\"},\"NetworkSettings\":{\"Networks\":{\"chrono_default\":{\"IPAMConfig\":null,\"Links\":null,\"Aliases\":null,\"NetworkID\":\"4d302dd5f3aaafcfabdfe82471f4e2b9f216e19bb1e9378bf19ec15dfc1285cc\",\"EndpointID\":\"01d80f9854f81a40cd61476d2ce5408c0f71ecbaf057893d2f91ddab165ea1e4\",\"Gateway\":\"172.28.0.1\",\"IPAddress\":\"172.28.0.3\",\"IPPrefixLen\":16,\"IPv6Gateway\":\"\",\"GlobalIPv6Address\":\"\",\"GlobalIPv6PrefixLen\":0,\"MacAddress\":\"02:42:ac:1c:00:03\",\"DriverOpts\":null}}},\"Mounts\":[{\"Type\":\"volume\",\"Name\":\"f0e71c41824054c367eaf8adeaef8176f0891186c059f65b1eaed225acec2b40\",\"Source\":\"\",\"Destination\":\"/var/lib/postgresql/data\",\"Driver\":\"local\",\"Mode\":\"\",\"RW\":true,\"Propagation\":\"\"}]},{\"Id\":\"f39f8446414b642c5dbdac31f5276e513ce42709237a0306236e3678a278b1c0\",\"Names\":[\"/chrono_crono_1\"],\"Image\":\"crono\",\"ImageID\":\"sha256:08c264a636bc6f8ef5e14dced0445c2ddbb2c8ace43445a616cb70cd141bbb11\",\"Command\":\"dotnet Crono.dll\",\"Created\":1660159512,\"Ports\":[],\"Labels\":{\"com.docker.compose.config-hash\":\"29ce4dc547c1b572aa5376f0aa2b061684ad67b11343f2a7c69a1afd350cd7fb\",\"com.docker.compose.container-number\":\"1\",\"com.docker.compose.oneoff\":\"False\",\"com.docker.compose.project\":\"chrono\",\"com.docker.compose.project.config_files\":\"docker-compose.yml\",\"com.docker.compose.project.working_dir\":\"/mnt/c/Users/HUDSON~1.VEN/source/repos/Dropbox/Chrono\",\"com.docker.compose.service\":\"crono\",\"com.docker.compose.version\":\"1.29.2\"},\"State\":\"running\",\"Status\":\"Up Less than a second\",\"HostConfig\":{\"NetworkMode\":\"chrono_default\"},\"NetworkSettings\":{\"Networks\":{\"chrono_default\":{\"IPAMConfig\":null,\"Links\":null,\"Aliases\":null,\"NetworkID\":\"4d302dd5f3aaafcfabdfe82471f4e2b9f216e19bb1e9378bf19ec15dfc1285cc\",\"EndpointID\":\"f9346c1d3d950a2b8f4fbb32ea14407393b085f227aaa6cfe1f7b925945aa829\",\"Gateway\":\"172.28.0.1\",\"IPAddress\":\"172.28.0.4\",\"IPPrefixLen\":16,\"IPv6Gateway\":\"\",\"GlobalIPv6Address\":\"\",\"GlobalIPv6PrefixLen\":0,\"MacAddress\":\"02:42:ac:1c:00:04\",\"DriverOpts\":null}}},\"Mounts\":[{\"Type\":\"bind\",\"Source\":\"/mnt/c/Users/HUDSON~1.VEN/source/repos/Dropbox/Chrono/docker-compose.yml\",\"Destination\":\"/app/docker-compose.yml\",\"Mode\":\"rw\",\"RW\":true,\"Propagation\":\"rprivate\"},{\"Type\":\"bind\",\"Source\":\"/var/run/docker.sock\",\"Destination\":\"/var/run/docker.sock\",\"Mode\":\"ro\",\"RW\":false,\"Propagation\":\"rprivate\"}]},{\"Id\":\"16f8f7262360db3bfceea1bb54ee073c0b8f770e64e09de42e0d575c93d47b49\",\"Names\":[\"/chrono_db_1\"],\"Image\":\"postgres:14\",\"ImageID\":\"sha256:b2261d3c6ce0b23bed32e7567a92646b880de73d802550b1275baa0997aa34d0\",\"Command\":\"docker-entrypoint.sh postgres\",\"Created\":1660159512,\"Ports\":[{\"PrivatePort\":5432,\"Type\":\"tcp\"}],\"Labels\":{\"com.docker.compose.config-hash\":\"afbef7ae1b4b4b3c4ab493cd375add6f5097ff8d24018b9efe5ae5edc60c6bc5\",\"com.docker.compose.container-number\":\"1\",\"com.docker.compose.oneoff\":\"False\",\"com.docker.compose.project\":\"chrono\",\"com.docker.compose.project.config_files\":\"docker-compose.yml\",\"com.docker.compose.project.working_dir\":\"/mnt/c/Users/HUDSON~1.VEN/source/repos/Dropbox/Chrono\",\"com.docker.compose.service\":\"db\",\"com.docker.compose.version\":\"1.29.2\"},\"State\":\"running\",\"Status\":\"Up Less than a second\",\"HostConfig\":{\"NetworkMode\":\"chrono_default\"},\"NetworkSettings\":{\"Networks\":{\"chrono_default\":{\"IPAMConfig\":null,\"Links\":null,\"Aliases\":null,\"NetworkID\":\"4d302dd5f3aaafcfabdfe82471f4e2b9f216e19bb1e9378bf19ec15dfc1285cc\",\"EndpointID\":\"6023cd5fd10d03cd468947036a5679edc1b90672962613ea259b89210d3dc0ad\",\"Gateway\":\"172.28.0.1\",\"IPAddress\":\"172.28.0.2\",\"IPPrefixLen\":16,\"IPv6Gateway\":\"\",\"GlobalIPv6Address\":\"\",\"GlobalIPv6PrefixLen\":0,\"MacAddress\":\"02:42:ac:1c:00:02\",\"DriverOpts\":null}}},\"Mounts\":[{\"Type\":\"volume\",\"Name\":\"3ccd6dd28edad7bf948a298a7b333b987dae707e02a2451f9328086860a205bc\",\"Source\":\"\",\"Destination\":\"/var/lib/postgresql/data\",\"Driver\":\"local\",\"Mode\":\"\",\"RW\":true,\"Propagation\":\"\"}]}]";
            return JArray.Parse(response.Replace("{\"message\":\"page not found\"}", ""));
            
        }

        public static void operateContainer(Container container, string command)
        {
            //realiza o restart do container
            string retorno = ExternalApp.run("curl", $"--silent -XPOST --unix-socket /run/docker.sock -H 'Content-Type: application/json' http://localhost/containers/{container.containerID}/{command}");
            retorno = retorno.Replace("{\"message\":\"page not found\"}", "");
            if (retorno.Contains("No such container"))
            {
                throw new Exception($"No such container. Service {container.serviceName}, ID: {container.containerID}");
            }
            Console.WriteLine($"Container has been restarted successfully. Service name {container.serviceName}, ID {container.containerID}", Console.typeMessage.SUCESS);
        }

        
    }
}
