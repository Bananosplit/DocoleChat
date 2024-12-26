#include <QApplication>
#include <QDebug>
#include <iostream>
#include "mainwindow.h"

#include <grpc++/grpc++.h>

#include "ircclient.h"

int main(int argc, char *argv[]) {
    GOOGLE_PROTOBUF_VERIFY_VERSION;

    // auto channel = grpc::CreateChannel("localhost:50051", grpc::InsecureChannelCredentials());
    // ircClient = std::make_shared<IrcClient>(channel);
    // QObject::connect(ui->lineEdit, &QLineEdit::returnPressed, this, &MainWindow::return_pressed);

    QApplication a(argc, argv);
    MainWindow mw;
    mw.show();
    return QApplication::exec();
}
