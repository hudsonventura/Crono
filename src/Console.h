#ifndef CONSOLE_H
#define CONSOLE_H

#include <string>



class Console {
public:
    static void Write(std::string message);
    static void WriteLine(std::string message);
    static void WriteEnd(std::string message);
    static void Error(std::string message);
};

#endif
