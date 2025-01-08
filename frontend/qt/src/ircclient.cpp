#include "ircclient.h"

IrcClient::IrcClient(){
    channel = grpc::CreateChannel("localhost:5085", grpc::InsecureChannelCredentials());

    stub = IrcService::NewStub(channel);

    ClientContext context;

    IrcVoid stubArg;
    IrcToken tokenResponse;
    Status status = stub->GetToken(&context, stubArg, &tokenResponse);
    token = tokenResponse.token();

}

std::string IrcClient::SendMessage(const std::string &message){
    IrcMessage request;
    // request.message = message;
    request.set_message(message);
    request.set_token(token);

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
    IrcMessage reply;
    IrcToken grpc_token;
    grpc_token.set_token(token);

    ClientContext context;

    std::unique_ptr<ClientReader<IrcMessage>> reader(stub->GetMessages(&context, grpc_token));

    while(reader->Read(&reply)){
        std::cout << reply.message() << std::endl;
        out.push_back((reply.message()));
    }
    Status status = reader->Finish();
}
