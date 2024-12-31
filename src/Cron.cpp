#include <iostream>
#include <sstream>
#include <vector>
#include <ctime>
#include <string>
#include <unordered_map>

#include "Cron.h"

Cron::Cron(){}

bool Cron::test(std::string scheduling) {
    // Verifica se é um valor especial
    if (scheduling[0] == '@') {
        scheduling = mapSpecialToCron(scheduling);
    }

    // Quebra a string em campos separados por espaço
    std::istringstream ss(scheduling);
    std::vector<std::string> fields;
    std::string field;

    while (ss >> field) {
        fields.push_back(field);
    }

    // Verifica se temos exatamente 5 campos
    if (fields.size() != 5) {
        std::cerr << "Erro: Configuração do crontab inválida!" << std::endl;
        return false;
    }

    // Obtém a data e hora atuais
    std::time_t now = std::time(nullptr);
    std::tm* localTime = std::localtime(&now);

    int minute = localTime->tm_min;
    int hour = localTime->tm_hour;
    int dayOfMonth = localTime->tm_mday;
    int month = localTime->tm_mon + 1;  // `tm_mon` começa em 0
    int dayOfWeek = localTime->tm_wday; // `tm_wday` (0 = domingo)

    // Verifica se cada campo corresponde
    return matchesField(fields[0], minute) &&
           matchesField(fields[1], hour) &&
           matchesField(fields[2], dayOfMonth) &&
           matchesField(fields[3], month) &&
           matchesField(fields[4], dayOfWeek);
}

bool Cron::matchesField(const std::string& field, int value) {
    if (field == "*") {
        return true; // Aceita qualquer valor
    }

    std::istringstream ss(field);
    std::string token;

    // Verifica listas ou intervalos
    while (std::getline(ss, token, ',')) {
        if (token.find('-') != std::string::npos) {
            // É um intervalo (ex.: "1-5")
            int start, end;
            std::sscanf(token.c_str(), "%d-%d", &start, &end);
            if (value >= start && value <= end) {
                return true;
            }
        } else {
            // É um valor único (ex.: "5")
            if (std::stoi(token) == value) {
                return true;
            }
        }
    }

    return false; // Não corresponde a nenhum critério
}

std::string Cron::mapSpecialToCron(const std::string& special) {
    static const std::unordered_map<std::string, std::string> specialMapping = {
        {"@yearly", "0 0 1 1 *"},
        {"@annually", "0 0 1 1 *"},
        {"@monthly", "0 0 1 * *"},
        {"@weekly", "0 0 * * 0"},
        {"@daily", "0 0 * * *"},
        {"@midnight", "0 0 * * *"},
        {"@hourly", "0 * * * *"}
    };

    auto it = specialMapping.find(special);
    if (it != specialMapping.end()) {
        return it->second;
    }

    std::cerr << "Erro: Valor especial inválido!" << std::endl;
    return "* * * * *"; // Retorna um padrão inválido para evitar falsos positivos
}
