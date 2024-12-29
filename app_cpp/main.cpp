#include <iostream>
#include <thread>
#include <chrono>
#include <cstdlib>
#include <memory>
#include <stdexcept>
#include <array>
#include <csignal>

using namespace std;


string exec(const char* cmd) {
    array<char, 128> buffer;
    string result;
    unique_ptr<FILE, decltype(&pclose)> pipe(popen(cmd, "r"), pclose);
    if (!pipe) {
        throw runtime_error("popen() failed!");
    }
    while (fgets(buffer.data(), buffer.size(), pipe.get()) != nullptr) {
        result += buffer.data();
    }
    return result;
}

string getCurrentTimezoneFromENV() {
    return exec("cat /etc/timezone");
}

int getWaitFromENV() {
    const char* wait = getenv("WAIT");
    if (wait == nullptr) {
        return 0;
    }
    try {
        return stoi(wait);
    } catch (const invalid_argument& ia) {
        return 0;
    }
}

void wait_time(int time) {
    this_thread::sleep_for(chrono::seconds(time));
}

void showMessage(string message) {
    cout << message << endl;
}

string getCurrentDateTime() {
    time_t now = time(0);
    tm *ltm = localtime(&now);

    char buffer[50];
    strftime(buffer, sizeof(buffer), "%Y-%m-%d %H:%M:%S", ltm);

    return string(buffer);
}

int main() {
    signal(SIGINT, [](int signal){ cout << "I received a signal to exit (Ctrl+C), so ... I'm out!" << endl; exit(0); });

    string timezone = getCurrentTimezoneFromENV();
    showMessage("Timezone: " + timezone);

    if(timezone == "Etc/UTC") {
        showMessage("The timezone was not set or it was set to UTC. If you want to set your timezone, use the environment variable TZ=America/Sao_Paulo on your docker-compose file.");
    }

    int wait = getWaitFromENV();
    showMessage("Waiting for " + to_string(wait) + " seconds before start my work");
    wait_time(wait);

    while (true) {
        string minhaString = "Olá, mundo! " + getCurrentDateTime();
        cout << minhaString << endl;

        wait_time(3);

       
        

    }
    return 0;
}

