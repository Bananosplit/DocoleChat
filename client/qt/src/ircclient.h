#ifndef IRCCLIENT_H
#define IRCCLIENT_H

#include <grpc++/grpc++.h>
#include "irc.pb.h"
#include "irc.grpc.pb.h"

using grpc::Channel;
using grpc::ClientContext;
using grpc::Status;

using irc::IrcRequest;
using irc::IrcResponse;
using irc::IrcService;

class IrcClient
{
private:
    std::unique_ptr<IrcService::Stub> stub;
public:
    IrcClient(std::shared_ptr<Channel> channel);

    std::string SendMessage(const std::string &message);

};

#endif // IRCCLIENT_H
