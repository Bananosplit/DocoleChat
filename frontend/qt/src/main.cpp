#include <QApplication>
#include <QDebug>
#include <iostream>
#include "mainwindow.h"

#include <grpc++/grpc++.h>

#include "ircclient.h"

int main(int argc, char *argv[]) {
    GOOGLE_PROTOBUF_VERIFY_VERSION;

    QApplication a(argc, argv);
    MainWindow mw;
    mw.show();
    return QApplication::exec();
}
