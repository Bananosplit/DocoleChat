#include <QCoreApplication>
#include <QTcpServer>
#include <QTcpSocket>
#include <QTimer>
#include <iostream>
#include <memory>

QTcpServer server;

void getConnection() {
	auto socket = std::shared_ptr<QTcpSocket>(server.nextPendingConnection());
	socket->waitForReadyRead();
	QByteArray message = socket->readAll();	
	std::cout << message.toStdString() << std::endl;
}

void serverStart() {
	int port = 7000;
	QObject::connect(&server, &QTcpServer::newConnection, getConnection);
	if (server.listen(QHostAddress::Any, port)) {
		std::cout << "server started on port " << port << std::endl;
	}
}

int main(int argc, char *argv[]){
	QCoreApplication app(argc, argv);
	QTimer::singleShot(100, serverStart);
	return app.exec();
}
