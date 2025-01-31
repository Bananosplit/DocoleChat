#include <stdio.h>
#include <cstdlib>
#include <readline/readline.h>
#include <readline/history.h>
#include <string>
#include <iostream>

#include <grpc++/grpc++.h>
#include "irc.pb.h"
#include "irc.grpc.pb.h"
#include <vector>


using grpc::Channel;
using grpc::ClientContext;
using grpc::Status;
using grpc::ClientReader;

using irc::IrcMessage;
// using irc::IrcReply;
using irc::IrcService;
using irc::IrcVoid;
using irc::IrcToken;

std::shared_ptr<IrcService::Stub> stub;
std::string token = "";

std::string command_get(const char *line){
    return "000";
}

void send_cmd(const char *line){

    std::string cmd(line);
    std::string opcode = cmd.substr(0,cmd.find(" "));      


    if(opcode == "/connect"){
        std::shared_ptr<Channel> channel;
        std::string token;

        channel = grpc::CreateChannel("localhost:8080", grpc::InsecureChannelCredentials());
        stub = IrcService::NewStub(channel);
        std::cout << "000" << std::endl;
        return;
        // return "000";
    }

    if(!stub){
        std::cout << "001" << std::endl;
        return;
        // return "001";
    }
    if(token.empty()){
        std::cout << "002" << std::endl;
        return;
        // return "002";
    }

    if(opcode == "/get"){
        IrcMessage reply;
        IrcToken grpc_token;
        grpc_token.set_token(token);

        ClientContext context;

        std::unique_ptr<ClientReader<IrcMessage>> reader(stub->GetMessages(&context, grpc_token));

        while(reader->Read(&reply)){
            std::cout << reply.message();
        }
        Status status = reader->Finish();
        return;
        // return "000";
    }

    if(opcode == "/setclient"){
        std::cout << "000" << std::endl;
        return;
        // return "000";
    }
    IrcMessage request;
    request.set_message(cmd+ "\r\n");
    request.set_token(token);

    IrcVoid reply;

    ClientContext context;
    Status status = stub->SendMessage(&context, request, &reply);

    if(status.ok()){
        // return reply.message();
        // stub->getMess
    } else {
        std::cout << status.error_code() << ": " << status.error_message() << std::endl;
        // return "gRPC failed";
    }
}

int main(int argc, char ** argv)
{
    GOOGLE_PROTOBUF_VERIFY_VERSION;

    srand(std::time(0));

    char buffer[50];
    for(int i = 0; i < 20; i++){
        buffer[i] = char('a' + rand() % ('z' - 'a'));
    }
    buffer[20] = '\0';
    token = std::string(buffer);

    while(1)
    {
        char * line = readline("> ");
        if(!line) break;
        if(*line) add_history(line);
        if(strcmp(line, "exit") == 0) break;

        send_cmd(line);

        free(line);
    }
}
