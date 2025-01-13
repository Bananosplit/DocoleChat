#include <stdio.h>
#include <cstdlib>
#include <readline/readline.h>
#include <readline/history.h>
#include <string>
#include <iostream>

#include <grpc++/grpc++.h>
#include "irc.pb.h"
#include "irc.grpc.pb.h"


using grpc::Channel;
using grpc::ClientContext;
using grpc::Status;
using grpc::ClientReader;

using irc::IrcMessage;
using irc::IrcReply;
using irc::IrcService;
using irc::IrcVoid;
using irc::IrcToken;

std::string send_cmd(std::shared_ptr<IrcService::Stub> stub, std::string token, const char *line){
    std::string cmd(line);
    std::string opcode = cmd.substr(0,cmd.find(" "));
    if(opcode == "get"){
        IrcMessage reply;
        IrcToken grpc_token;
        grpc_token.set_token(token);

        ClientContext context;

        std::unique_ptr<ClientReader<IrcMessage>> reader(stub->GetMessages(&context, grpc_token));

        while(reader->Read(&reply)){
            std::cout << reply.message() << std::endl;
        }
        Status status = reader->Finish();
        return "000";
    }

    IrcMessage request;
    request.set_message(cmd+ "\r\n");
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

int main(int argc, char ** argv)
{
    GOOGLE_PROTOBUF_VERIFY_VERSION;

    std::shared_ptr<IrcService::Stub> stub;
    std::shared_ptr<Channel> channel;
    std::string token;

    channel = grpc::CreateChannel("localhost:5085", grpc::InsecureChannelCredentials());
    stub = IrcService::NewStub(channel);

    ClientContext context;

    IrcVoid stubArg;
    IrcToken tokenResponse;
    Status status = stub->GetToken(&context, stubArg, &tokenResponse);
    if(!status.ok()){
        std::cout << status.error_code() << ": " << status.error_message() << std::endl;
        std::cout << "gRPC failed" << std::endl;
        return 1;
    }
    token = tokenResponse.token();
    std::cout << "get token: " << token << std::endl;

    while(1)
    {
        char * line = readline("> ");
        if(!line) break;
        if(*line) add_history(line);
        if(strcmp(line, "exit") == 0) break;

        std::string res = send_cmd(stub, token, line);
        std::cout << res << std::endl;

        free(line);
    }
}
