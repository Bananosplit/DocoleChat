#include <QApplication>
#include <QDebug>
#include <iostream>
#include "mainwindow.h"

int main(int argc, char *argv[]) {
    QApplication a(argc, argv);
    qDebug() << "Hello World";
    MainWindow mw;
    mw.show();
    return QApplication::exec();
    //return 0;
}
