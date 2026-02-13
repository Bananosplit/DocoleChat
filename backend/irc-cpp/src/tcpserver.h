#ifndef TCPSERVER_H
#define TCPSERVER_H

#include <QTcpServer>

class TcpServer : public QTcpServer
{
public:
    TcpServer();

    // QTcpServer interface
protected:
    void incomingConnection(qintptr handle);
};

#endif // TCPSERVER_H
