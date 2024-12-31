#ifndef LOG_H
#define LOG_H


#include <iostream>
#include <fstream>
#include <iomanip>
#include <chrono>
#include <ctime>
#include <mutex>
#include <string>



class Log {
public:
    //void Info(std::string message);
    void Error(std::string message){
        static std::mutex mtx;
        std::unique_lock<std::mutex> lock(mtx);

        time_t now = time(0);
        tm *ltm = localtime(&now);

        char filename[64];
        strftime(filename, sizeof(filename), "%F.log", ltm);

        std::string filePath = "crono." + std::string(filename);
        std::ifstream logFile(filePath, std::ios::app);
        if (logFile.is_open()) {
            auto now = std::chrono::system_clock::now();
            auto nowTimeT = std::chrono::system_clock::to_time_t(now);
            auto nowTp = std::chrono::system_clock::now().time_since_epoch();
            auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

            std::tm nowTm = *std::localtime(&nowTimeT);
            //logFile << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' ERROR ' << message << std::endl;
            logFile.close();
        }
    }

    void Info(std::string message) {
        static std::mutex mtx;
        std::unique_lock<std::mutex> lock(mtx);

        time_t now = time(0);
        tm *ltm = localtime(&now);

        char filename[64];
        strftime(filename, sizeof(filename), "%F.log", ltm);

        std::string filePath = "crono." + std::string(filename);
        std::ifstream logFile(filePath, std::ios::app);
        if (logFile.is_open()) {
            auto now = std::chrono::system_clock::now();
            auto nowTimeT = std::chrono::system_clock::to_time_t(now);
            auto nowTp = std::chrono::system_clock::now().time_since_epoch();
            auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

            std::tm nowTm = *std::localtime(&nowTimeT);
            //logFile << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' INFO ' << message << std::endl;
            logFile.close();
        }
    }
};

#endif
