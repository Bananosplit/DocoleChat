//
// Created by impul on 30.03.2023.
//

// You may need to build the project (run Qt uic code generator) to get "ui_MainWindow.h" resolved

#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QLabel>
#include <QLineEdit>
#include <iostream>


MainWindow::MainWindow(QWidget *parent) :
        QMainWindow(parent), ui(new Ui::MainWindow) {
    ui->setupUi(this);

    auto channel = grpc::CreateChannel("localhost:50051", grpc::InsecureChannelCredentials());

    ircClient = std::make_shared<IrcClient>(channel);

    QObject::connect(ui->lineEdit, &QLineEdit::returnPressed, this, &MainWindow::return_pressed);
}

void MainWindow::return_pressed(){
    std::stringstream str;
    str << ":tenko PRIVMSG banana :" << ui->lineEdit->text().toStdString() << "\r\n";
    auto ret = ircClient->SendMessage(str.str());

    ui->textBrowser->append(ui->lineEdit->text() + "\n");

    ui->lineEdit->clear();
}

MainWindow::~MainWindow() {
    delete ui;
}
