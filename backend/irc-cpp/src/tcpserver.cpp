#include "tcpserver.h"
#include <QTcpSocket>

TcpServer::TcpServer() {}

void TcpServer::incomingConnection(qintptr handle)
{
    QTcpSocket *socket = new QTcpSocket();

    socket->setSocketDescriptor(handle);

}
