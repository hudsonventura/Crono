#ifndef YAML_H
#define YAML_H

#include <vector>
#include "Container.h"

class Yaml {
public:
    Yaml();              // Construtor
    std::vector<Container> getContainers(std::string path);
};

#endif

