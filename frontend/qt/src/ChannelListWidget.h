#ifndef CHANNELLISTWIDGET_H
#define CHANNELLISTWIDGET_H

#include <QWidget>

class ChannelListWidget : public QWidget
{
    Q_OBJECT



public:
    explicit ChannelListWidget(QWidget *parent = nullptr);

public slots:
    void update();
signals:
};

#endif // CHANNELLISTWIDGET_H
