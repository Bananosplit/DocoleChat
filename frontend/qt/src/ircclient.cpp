#include "ircclient.h"

IrcClient::IrcClient(){
    std::shared_ptr<Channel> channel = grpc::CreateChannel("localhost:8080", grpc::InsecureChannelCredentials());

    stub = IrcService::NewStub(channel);

    token = "irWeytewfOdMatAmtyepjushvoufrokicAtHa";

    ClientContext context;

    std::string pass_message = "PASS letmein\r\n";

    IrcVoid reply;
    IrcMessage irc_message;
    irc_message.set_message(pass_message);
    irc_message.set_token(token);
    Status status = stub->SendMessage(&context, irc_message, &reply);
    if(!status.ok()){
        std::cout << status.error_code() << ": " << status.error_message() << std::endl;
        exit(1);
    }
}

void IrcClient::SendMessage(const std::string &message){
    IrcMessage request;
    request.set_message(message);
    request.set_token(token);

    IrcVoid reply;

    ClientContext context;

    Status status = stub->SendMessage(&context, request, &reply);

    if(!status.ok()){
        std::cerr << status.error_code() << ": " << status.error_message() << std::endl;
    }
}

void IrcClient::GetMessages(std::list<std::string> &out){
    IrcMessage reply;
    IrcToken grpc_token;
    grpc_token.set_token(token);

    ClientContext context;

    std::unique_ptr<ClientReader<IrcMessage>> reader(stub->GetMessages(&context, grpc_token));

    while(reader->Read(&reply)){
        // std::cout << reply.message() << std::endl;
        out.push_back((reply.message()));
    }
    Status status = reader->Finish();
}
