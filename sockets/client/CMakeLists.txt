cmake_minimum_required(VERSION 3.20)
project(socket-client)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC ON)

find_package(Qt5 COMPONENTS
        Core
	Network
        REQUIRED)

add_executable(socket-client src/main.cpp)

target_link_libraries(socket-client
        Qt5::Core
        Qt5::Network
        )

