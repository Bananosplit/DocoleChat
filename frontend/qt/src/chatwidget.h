#ifndef CHATWIDGET_H
#define CHATWIDGET_H

#include <QWidget>
#include <ircclient.h>
#include <string.h>

QT_BEGIN_NAMESPACE
namespace Ui { class ChatWidget; }
QT_END_NAMESPACE

class ChatWidget : public QWidget
{
    Q_OBJECT

private:
    QTimer *message_timer;
    Ui::ChatWidget *ui;
    std::shared_ptr<IrcClient> ircClient;
    std::string name;

public:
    explicit ChatWidget(QWidget *parent = nullptr);

    ~ChatWidget() override;

public slots:
    void get_messages();

    void return_pressed();

    void ircClientChanged(std::shared_ptr<IrcClient> client);

signals:
};

#endif // CHATWIDGET_H
