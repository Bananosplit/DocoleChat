//
// Created by impul on 30.03.2023.
//

// You may need to build the project (run Qt uic code generator) to get "ui_MainWindow.h" resolved

#include "mainwindow.h"
#include "ui_MainWindow.h"
#include <QLabel>
#include <QLineEdit>


MainWindow::MainWindow(QWidget *parent) :
        QMainWindow(parent), ui(new Ui::MainWindow) {
    ui->setupUi(this);

    QObject::connect(ui->lineEdit, &QLineEdit::returnPressed, this, &MainWindow::return_pressed);
}

void MainWindow::return_pressed(){
    ui->textBrowser->append(ui->lineEdit->text() + "\n");
    ui->lineEdit->clear();
}

MainWindow::~MainWindow() {
    delete ui;
}
