#include <iostream>
#include <thread>
#include <chrono>
#include <cstdlib>
#include <memory>
#include <stdexcept>
#include <array>
#include <csignal>


#include "Yaml.h"
#include "Container.h"
#include "Docker.h"
#include "Cron.h"
#include "Console.h"


using namespace std;


string exec(const char* cmd) {
    array<char, 128> buffer;
    string result;
    unique_ptr<FILE, decltype(&pclose)> pipe(popen(cmd, "r"), pclose);
    if (!pipe) {
        throw runtime_error("popen() failed!");
    }
    while (fgets(buffer.data(), buffer.size(), pipe.get()) != nullptr) {
        result += buffer.data();
    }
    return result;
}

string getCurrentTimezoneFromENV() {
    return exec("cat /etc/timezone");
}

int getWaitFromENV() {
    const char* wait = getenv("WAIT");
    if (wait == nullptr) {
        return 0;
    }
    try {
        return stoi(wait);
    } catch (const invalid_argument& ia) {
        return 0;
    }
}

void Sleep(int time) {
    this_thread::sleep_for(chrono::seconds(time));
}



string getCurrentDateTime() {
    time_t now = time(0);
    tm *ltm = localtime(&now);

    char buffer[50];
    strftime(buffer, sizeof(buffer), "%Y-%m-%d %H:%M:%S", ltm);

    return string(buffer);
}



vector<Container> getContainers(string path) 
{
    Yaml yaml;
    vector<Container> containers = yaml.getContainers(path);

    Docker docker;
    vector<Container> runningContainers = docker.getContainersCreated(containers);

    return runningContainers;
}



int main() {
    signal(SIGINT, [](int signal){ cout << "I received a signal to exit (Ctrl+C), so ... I'm out!" << endl; exit(0); });
    Console Console;


    string timezone = getCurrentTimezoneFromENV();
    Console.WriteLine("Timezone: " + timezone);

    if(timezone == "Etc/UTC") {
        Console.WriteLine("The timezone was not set or it was set to UTC. If you want to set your timezone, use the environment variable TZ=America/Sao_Paulo on your docker-compose file.");
    }

    int wait = getWaitFromENV();
    Console.WriteLine("Waiting for " + to_string(wait) + " seconds before start my work");
    Sleep(wait);

    Console.WriteLine("");
    
    Console.Write("Reading containers ... ");
    vector<Container> runningContainers = getContainers("docker-compose.yml");
    Console.WriteLine("I found the following containers: ");
    for (const auto& container : runningContainers) 
    {
        Console.WriteLine(container.containerID);
    }
    

    Console.WriteLine("");
    Console.WriteLine("Starting my work!");
    Console.WriteLine("");
    


    while (true) {
        time_t now = time(0);
        if (localtime(&now)->tm_min % 10 == 0) {
            Console.WriteLine("");
            Console.WriteLine("Don't worry, I'm alive! I'm still working!");
            Console.WriteLine("");
        }

        for (const auto& container : runningContainers) {
            Cron cron;
            bool test = cron.test(container.scheduling);
            //Console.WriteLine(container.containerID+" - "+container.serviceName + " - " + container.scheduling + " - " + to_string(teste));
            if(test == true){
                const_cast<Container&>(container).Restart();
            }
        }

        //wait to next minute
        time_t now2 = time(0);
        time_t nextMin = now2 + (60 - localtime(&now2)->tm_sec);
        Sleep(difftime(nextMin, now2));

        
        Console.Write("Reading containers again ... ");
        runningContainers = getContainers("docker-compose.yml");
        Console.WriteEnd("I found the following containers: ");
        for (const auto& container : runningContainers) 
        {
            Console.WriteLine(container.containerID);
        }
        

    }
    return 0;
}
