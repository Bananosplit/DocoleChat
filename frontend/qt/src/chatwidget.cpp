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
    QString pattern = "[\r\n]";
    QRegularExpression regex(pattern);
    for(auto &i : messages){
        ui->textBrowser->append(QString::fromStdString(i).remove(regex));
    }
}

void ChatWidget::return_pressed()
{
    std::stringstream str;
    str << "PRIVMSG default :" << ui->lineEdit->text().toStdString() << "\r\n";

    auto ret = ircClient->SendMessage(str.str());

    get_messages();

    ui->lineEdit->clear();
}

void ChatWidget::ircClientChanged(std::shared_ptr<IrcClient> client)
{
    ircClient = client;
}
