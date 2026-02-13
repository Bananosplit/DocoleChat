#ifndef MESSAGE_H
#define MESSAGE_H
#include <string>

class Message
{
public:
    std::string name;
    std::string user;
    std::string host;
    std::string command;
    std::string params;

    Message();
};

#endif // MESSAGE_H
