#include <iostream>

#include "Console.h"



// Retorna os containers encontrados no arquivo de configuração
void Console::Write(std::string message) {
    std::cout << message;
}


void Console::WriteLine(std::string message) {
    std::cout << message << std::endl;
}
