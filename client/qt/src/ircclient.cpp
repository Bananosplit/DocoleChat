#include "ircclient.h"

IrcClient::IrcClient(){
    channel = grpc::CreateChannel("localhost:5085", grpc::InsecureChannelCredentials());

    stub = IrcService::NewStub(channel);
}

std::string IrcClient::SendMessage(const std::string &message){
    IrcMessage request;
    // request.message = message;
    request.set_message(message);

    IrcReply reply;

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
    // IrcMessage reply;
    // IrcVoid request;

    // ClientContext context;

    // // Status status = stub->GetMessages(&context, request, &reply);

    // std::unique_ptr<ClientReader<IrcMessage>> reader(stub->GetMessages(&context, request));

    // while(reader->Read(&reply)){
    //     std::cout << reply.message() << std::endl;
    //     out.push_back((reply.message()));
    // }
    // Status status = reader->Finish();
}
