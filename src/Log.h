#ifndef LOG_H
#define LOG_H


#include <iostream>
#include <fstream>
#include <iomanip>
#include <chrono>
#include <ctime>
#include <mutex>
#include <string>
#include <sys/stat.h>
#include <unistd.h>
#include "Console.h"


class Log {
public:
    //constructor
    Log() {
        Console Console;
        struct stat info;
        if (stat("./logs", &info) == 0 && (info.st_mode & S_IFDIR)) {
            Console.WriteLine("I'll save logs on ./logs");
            save = true;
        } else {
            Console.WriteLine("I'll NOT save logs. If you want to save logs, create, uncomment the line '#- \"./logs/:/app/logs/\"' on docker-compose.yml");
            save = false;
        }

    }


    //void Info(std::string message);
    void Error(std::string message){
        writeLog(message, "ERROR");
    }

    void Info(std::string message) {
        writeLog(message, "INFO ");
    }



private:
    bool save;

    void writeLog(std::string message, std::string type) {
        if(save == false)
        {
            return;
        }

        static std::mutex mtx;
        std::unique_lock<std::mutex> lock(mtx);

        time_t now = time(0);
        tm *ltm = localtime(&now);
        
        char filename[64];
        strftime(filename, sizeof(filename), "logs/%F-crono.log", ltm);

        std::ofstream logFile(filename, std::ios::app);
        if (logFile.is_open()) {
            auto now = std::chrono::system_clock::now();
            auto nowTimeT = std::chrono::system_clock::to_time_t(now);
            auto nowTp = std::chrono::system_clock::now().time_since_epoch();
            auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(nowTp).count() % 1000;

            std::tm nowTm = *std::localtime(&nowTimeT);
            logFile << std::put_time(&nowTm, "%F %T") << '.' << std::setfill('0') << std::setw(3) << ms << ' ' << type << ' ' << message << std::endl;
            logFile.close();
        }
    }
};

#endif
