#ifndef CRON_H
#define CRON_H

#include <string>

#include "Cron.h"

class Cron {
public:
    Cron();
    bool test(std::string scheduling);

private:
    bool matchesField(const std::string& field, int value);
    std::string mapSpecialToCron(const std::string& special);
};

#endif
