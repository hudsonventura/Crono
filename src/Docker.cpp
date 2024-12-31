
#include <json/json.h>

#include <memory>
#include <stdexcept>
#include <string>
#include <iostream>
#include <fstream>
#include <vector>

#include "Docker.h"
#include "Container.h"



// Construtor padrão
Docker::Docker(){}



// Método para exibir informações
// Retorna os containers encontrados no docker
std::vector<Container> Docker::getContainersCreated(const std::vector<Container> containers) {
    std::string command = "docker ps --format \"{{.Names}}|{{.ID}}\"";

    char buffer[1024];
    std::string commandStr = "curl --silent -XGET --unix-socket /var/run/docker.sock -H 'Content-Type: application/json' http://localhost/containers/json?all=1";
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(commandStr.c_str(), "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("Failed to run curl");
    }

    std::string response;
    while (fgets(buffer, sizeof(buffer), pipe.get()) != nullptr) {
        response += buffer;
    }

    if (response.empty() || response == "{\"message\":\"page not found\"}") {
        throw std::runtime_error("I ran curl but the response was empty");
    }

    std::vector<std::string> containersIDs = extractContainerNames(response);

    std::vector<Container> updatedContainers;
    for (auto& container : containers) {
        for (const auto& containerID : containersIDs) {
            if (containerID.find(container.serviceName) != std::string::npos) {
                // Crie uma cópia, modifique o campo e armazene no vetor atualizado
                Container updatedContainer = container;
                updatedContainer.containerID = containerID;
                updatedContainers.push_back(updatedContainer);
            }
        }
    }

    return updatedContainers;
}




// Function to extract the names of each container from a JSON string
std::vector<std::string> Docker::extractContainerNames(const std::string jsonString) {
    std::vector<std::string> containerNames;
    Json::Value root;
    Json::CharReaderBuilder reader;
    std::string errs;

    std::istringstream s(jsonString);
    if (Json::parseFromStream(reader, s, &root, &errs)) {
        for (const auto& container : root) {
            if (container.isMember("Names") && container["Names"].isArray() && !container["Names"].empty()) {
                std::string name = container["Names"][0].asString();
                containerNames.push_back(name.substr(1, name.size() - 1));
            }
        }
    } else {
        throw std::runtime_error("Failed to parse JSON: " + errs);
    }

    return containerNames;
}

