#include "ircclient.h"

IrcClient::IrcClient(std::shared_ptr<Channel> channel) : stub(IrcService::NewStub(channel)){

}

std::string IrcClient::SendMessage(const std::string &message){
    IrcRequest request;
    // request.message = message;
    request.set_message(message);

    IrcResponse reply;

    ClientContext context;

    Status status = stub->SendMessageW(&context, request, &reply);

    if(status.ok()){
        return reply.message();
    } else {
        std::cout << status.error_code() << ": " << status.error_message() << std::endl;
        return "gRPC failed";
    }
}
