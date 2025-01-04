#include "ircserver.h"
#include <iostream>

IrcServer::IrcServer() : IrcService::Service() {}

Status IrcServer::SendMessageW(grpc::ServerContext *context, const IrcMessage *request, IrcReply *response)
{
    std::cout << request->message();
    response->set_message("400");
    return Status::OK;
}
