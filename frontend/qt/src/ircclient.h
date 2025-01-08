#ifndef IRCCLIENT_H
#define IRCCLIENT_H

#include <grpc++/grpc++.h>
#include "irc.pb.h"
#include "irc.grpc.pb.h"
#include <string>
#include <list>

using grpc::Channel;
using grpc::ClientContext;
using grpc::Status;
using grpc::ClientReader;

using irc::IrcMessage;
using irc::IrcReply;
using irc::IrcService;
using irc::IrcVoid;
using irc::IrcToken;

class IrcClient
{
private:
    std::unique_ptr<IrcService::Stub> stub;
    std::shared_ptr<Channel> channel;
    std::string token;
public:
    IrcClient();

    std::string SendMessage(const std::string &message);
    void GetMessages(std::list<std::string> &out);

};

#endif // IRCCLIENT_H
