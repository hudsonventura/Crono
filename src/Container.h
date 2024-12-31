#ifndef CONTAINER_H
#define CONTAINER_H

#include <string>
#include <memory>
#include "Console.h"
#include "Log.h"

class Container {
public:
    std::string serviceName;      
    std::string scheduling; 
    std::string containerID;  

    // Construtores
    Container();

    // Método extra para exibir informações
    void Restart() {
        Console Console;
        Log Log;

        Console.Write("Restarting container "+ serviceName + " ... ");
        
        std::string command = "curl --silent -XPOST --unix-socket /var/run/docker.sock -H 'Content-Type: application/json' http://localhost/containers/" + containerID + "/restart";

        std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(command.c_str(), "r"), pclose);
        if (!pipe) {
            throw std::runtime_error("Failed to run curl");
        }

        char buffer[1024];
        std::string response;
        while (fgets(buffer, sizeof(buffer), pipe.get()) != nullptr) {
            response += buffer;
        }


        if (response.empty()) {
            Console.WriteEnd(" Done!");
            Log.Info(serviceName + " restarted successfully");
        } else {
            Console.Error("Error: " + response);
            Log.Error(serviceName+" error on restart: " + response);
        }
    }
};

#endif
