#include <QTcpSocket>
#include <QHostAddress>
#include <iostream>
#include <QCoreApplication>
#include <QTimer>
#include <string>
std::string ip_addr;

int func() {
	while (true) {
		std::string message;
		std::cout << "Enter message" << std::endl;
		std::cin >> message;
		QTcpSocket socket;
		socket.connectToHost(QHostAddress(ip_addr.c_str()), 7000);
		socket.waitForConnected();
		socket.write(message.c_str());
		socket.waitForBytesWritten();
		socket.close();
	}
}


int main(int argc, char *argv[]){
	std::cout << "Enter ip: ";
	std::cin >> ip_addr;
	QCoreApplication app(argc, argv);
	QTimer::singleShot(100, func);
	return app.exec();
}
