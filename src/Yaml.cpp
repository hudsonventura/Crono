#include <yaml-cpp/yaml.h>
#include <iostream>
#include <fstream>
#include "Yaml.h"
#include <vector>
#include "Container.h"

// Construtor padrão
Yaml::Yaml(){}



// Retorna os containers encontrados no arquivo de configuração
std::vector<Container> Yaml::getContainers(std::string path) {
    std::ifstream file(path);
    if (!file.is_open()) {
        std::cerr << "Erro ao abrir arquivo de configura o: " << path << std::endl;
        return {};
    }

    YAML::Node node = YAML::Load(file);
    std::vector<Container> containers;
    for (const auto& service : node["services"]) {
        std::string name = service.first.as<std::string>();
        auto labels = service.second["labels"];
        if (labels && labels["crono"]) {
            Container container;
            container.serviceName = name;
            container.scheduling = labels["crono"].as<std::string>();
            containers.push_back(container);
        }
    }
    file.close();
    return containers;
}

