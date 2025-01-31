#include "chatwidget.h"
#include "ui_chatwidget.h"
#include <QTimer>
#include <QRegularExpression>
#include <assert.h>
#include <mainwindow.h>

ChatWidget::ChatWidget(QWidget *parent)
    : QWidget(parent), ui(new Ui::ChatWidget)
{
    ui->setupUi(this);

    message_timer = new QTimer(this);

    QObject::connect(ui->lineEdit, &QLineEdit::returnPressed, this, &ChatWidget::return_pressed);
    QObject::connect(message_timer, &QTimer::timeout, this, &ChatWidget::get_messages);
    message_timer->start(1000);
}

ChatWidget::~ChatWidget()
{
    delete ui;
}

void ChatWidget::get_messages()
{
    std::list<std::string> messages;

    ircClient->GetMessages(messages);
    QRegularExpression regex("[\r\n]");
    for(auto &i : messages){
        QString mes = QString::fromStdString(i).remove(regex);
        QRegularExpression server_regex(":server.*");
        if(mes.contains(server_regex)){
            ui->textBrowser->append(mes);
        } else {
            std::cout << mes.toStdString() << std::endl;
            // dynamic_cast<MainWindow>(parent())->ui->stat
        }
    }
}

void ChatWidget::return_pressed()
{
    std::stringstream str;
    str << "PRIVMSG " << name << " :" << ui->lineEdit->text().toStdString() << "\r\n";

    ircClient->SendMessage(str.str());

    get_messages();

    ui->lineEdit->clear();
}

void ChatWidget::ircClientChanged(std::shared_ptr<IrcClient> client)
{
    ircClient = client;
}
