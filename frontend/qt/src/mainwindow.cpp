//
// Created by impul on 30.03.2023.
//

// You may need to build the project (run Qt uic code generator) to get "ui_MainWindow.h" resolved

#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QLabel>
#include <QLineEdit>
#include <iostream>
#include <string>
#include <list>
#include <QInputDialog>
#include "ircserver.h"
#include "irc.grpc.pb.h"
#include "irc.pb.h"
#include <grpc++/grpc++.h>
#include <QTimer>

void server_thread_func(){
    std::string server_address("0.0.0.0:50051");

    // grpc::ServerBuilder builder;
    // builder.AddListeningPort(server_address, grpc::InsecureServerCredentials());
    // builder.RegisterService(&service);

    // auto ircServer = std::shared_ptr<grpc::Server>(builder.BuildAndStart());
    // std::cout << "Server listening on " << server_address << std::endl;

    // ircServer->Wait();
}

MainWindow::MainWindow(QWidget *parent) :
        QMainWindow(parent), ui(new Ui::MainWindow) {
    ui->setupUi(this);

    // server_thread = std::make_unique<std::thread>(server_thread_func);
    // server_thread->detach();

    ircClient = std::make_shared<IrcClient>();

    std::stringstream irc_message;

    bool ok;
    QString text = QInputDialog::getText(this, "Input dialog", "Enter nickname", QLineEdit::Normal, "", &ok);
    if(ok){
        nick = text.toStdString();
        std::stringstream str;
        str << ":  NICK " << nick << "\r\n";
        ircClient->SendMessage(str.str());
    }

    irc_message << ":" << nick << " JOIN " << "default" << "\r\n";
    auto ret = ircClient->SendMessage(irc_message.str());

    std::list<std::string> messages;
    ircClient->GetMessages(messages);
    for(auto &i : messages){
        ui->textBrowser->append(QString::fromStdString(i));
    }

    QObject::connect(ui->lineEdit, &QLineEdit::returnPressed, this, &MainWindow::return_pressed);
}

void MainWindow::return_pressed(){

    std::stringstream str;
    str << ":" << nick << " PRIVMSG default :" << ui->lineEdit->text().toStdString() << "\r\n";

    std::list<std::string> messages;    

    auto ret = ircClient->SendMessage(str.str());

    ircClient->GetMessages(messages);
    for(auto &i : messages){
        ui->textBrowser->append(QString::fromStdString(i));
    }

    // ui->textBrowser->append(QString(nick) + ": " + ui->lineEdit->text());
    // ui->textBrowser->append(QString::fromStdString(str.str()));

    ui->lineEdit->clear();
}

MainWindow::~MainWindow() {
    // ircServer->Shutdown();
    delete ui;
}
