#include "ircclient.h"

IrcClient::IrcClient(std::shared_ptr<Channel> channel) : stub(IrcService::NewStub(channel)){

}

std::string IrcClient::SendMessage(const std::string &message){
    IrcRequest request;
    // request.message = message;
    request.set_message(message);

    IrcResponse reply;

    ClientContext context;

    Status status = stub->SendMessage(&context, request, &reply);

    if(status.ok()){
        return reply.message();
    } else {
        std::cout << status.error_code() << ": " << status.error_message() << std::endl;
        return "gRPC failed";
    }
}

void IrcClient::GetMessages(std::list<std::string> &out){
    IrcResponse reply;
    IrcVoid request;

    ClientContext context;

    // Status status = stub->GetMessages(&context, request, &reply);

    std::unique_ptr<ClientReader<IrcResponse>> reader(stub->GetMessages(&context, request));

    while(reader->Read(&reply)){
        std::cout << reply.message() << std::endl;
        out.push_back((reply.message()));
    }
    Status status = reader->Finish();
}
