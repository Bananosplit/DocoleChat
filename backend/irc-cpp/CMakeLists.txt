cmake_minimum_required(VERSION 3.16.0 FATAL_ERROR)
if(POLICY CMP0167)
  cmake_policy(SET CMP0167 NEW)
endif()

project(irc-server)

set(CMAKE_CXX_STANDARD 17)

find_package(Qt6 COMPONENTS Core Network REQUIRED)
find_package(Boost COMPONENTS regex REQUIRED)

add_library(${PROJECT_NAME}-lib
        src/tcpserver.h src/tcpserver.cpp
	src/message.h	src/message.cpp
	src/parse.cpp	src/parse.h
	)

target_link_libraries(${PROJECT_NAME}-lib PUBLIC Qt6::Core Qt6::Network Boost::regex)
target_include_directories(${PROJECT_NAME}-lib PRIVATE ${CMAKE_CURRENT_LIST_DIR}/src ${Boost_INCLUDE_DIRS})


add_executable(${PROJECT_NAME} app/main.cpp)
target_link_libraries(${PROJECT_NAME} PUBLIC ${PROJECT_NAME}-lib)

add_subdirectory(tests)
