//
// Created by impul on 30.03.2023.
//

#ifndef DOCOLECHAT_MAINWINDOW_H
#define DOCOLECHAT_MAINWINDOW_H

#include <QMainWindow>
#include "ircclient.h"
#include <thread>


QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow {
    Q_OBJECT
    std::shared_ptr<IrcClient> ircClient;
    std::string nick;

public:
    explicit MainWindow(QWidget *parent = nullptr);

    ~MainWindow() override;




private:
    Ui::MainWindow *ui;
private slots:
signals:
    void ircClientChanged(std::shared_ptr<IrcClient> channel);
};


#endif //DOCOLECHAT_MAINWINDOW_H
