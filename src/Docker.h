// Container.h
#ifndef DOCKER_H
#define DOCKER_H

#include <vector>
#include "Container.h"

class Docker {
public:
    Docker();              // Construtor
    std::vector<Container> getContainersCreated(const std::vector<Container> containers);

private:
    std::vector<std::string> extractContainerNames(const std::string jsonString);
};


#endif

