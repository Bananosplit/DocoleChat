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
#include <QRegularExpression>

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
    QObject::connect(this, &MainWindow::ircClientChanged, ui->chatWidget, &ChatWidget::ircClientChanged);
    emit ircClientChanged(ircClient);


    std::stringstream irc_message;
    irc_message << ":" << nick << " JOIN " << "default" << "\r\n";
    ircClient->SendMessage(irc_message.str());

}


MainWindow::~MainWindow() {
    // ircServer->Shutdown();
    delete ui;
}
