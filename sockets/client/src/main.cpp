#include <QTcpSocket>
#include <QHostAddress>
#include <iostream>
#include <QCoreApplication>
#include <QTimer>

int func() {
	while (true) {
		std::string message;
		std::cout << "Enter message" << std::endl;
		std::cin >> message;
		QTcpSocket socket;
		socket.connectToHost(QHostAddress("127.0.0.1"), 7000);
		socket.waitForConnected();
		socket.write(message.c_str());
		socket.waitForBytesWritten();
		socket.close();
	}
}


int main(int argc, char *argv[]){
	QCoreApplication app(argc, argv);
	QTimer::singleShot(100, func);
	return app.exec();
}
