#include <iostream>

#include "Console.h"



// Retorna os containers encontrados no arquivo de configuração
#include <iomanip>
#include <chrono>
#include <ctime>

void Console::Write(std::string message) {
    auto now = std::chrono::system_clock::now();
    auto nowTimeT = std::chrono::system_clock::to_time_t(now);
    auto nowTp = std::chrono::system_clock::now().time_since_epoch();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

    std::tm nowTm = *std::localtime(&nowTimeT);
    std::cout << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' ' << message;
}


void Console::WriteLine(std::string message) {
    auto now = std::chrono::system_clock::now();
    auto nowTimeT = std::chrono::system_clock::to_time_t(now);
    auto nowTp = std::chrono::system_clock::now().time_since_epoch();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

    std::tm nowTm = *std::localtime(&nowTimeT);
    std::cout << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' ' << message << std::endl;
}

void Console::WriteEnd(std::string message) {
    std::cout <<  message << std::endl;
}



void Console::Error(std::string message) {
    auto now = std::chrono::system_clock::now();
    auto nowTimeT = std::chrono::system_clock::to_time_t(now);
    auto nowTp = std::chrono::system_clock::now().time_since_epoch();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

    std::tm nowTm = *std::localtime(&nowTimeT);

    std::cout << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' ' << "\033[1;31m" << message << "\033[0m" << std::endl;

}