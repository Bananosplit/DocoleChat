#ifndef IRCSERVER_H
#define IRCSERVER_H
#include <grpc++/grpc++.h>
#include "irc.pb.h"
#include "irc.grpc.pb.h"

using grpc::Channel;
using grpc::ClientContext;
using grpc::Status;
using grpc::ClientReader;

using irc::IrcMessage;
using irc::IrcVoid;
using irc::IrcService;

class IrcServer final : public IrcService::Service
{
public:
    IrcServer();

    // Service interface
public:
    Status SendMessageW(grpc::ServerContext *context, const IrcMessage *request, IrcVoid *response);
};

#endif // IRCSERVER_H
