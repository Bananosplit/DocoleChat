cmake_minimum_required(VERSION 3.20)
project(DocoleChat)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC OFF)

message(STATUS ${CMAKE_COMMAND})

set(QT Qt6)
find_package(${QT} COMPONENTS
        Core
        Widgets
        REQUIRED)
find_package(gRPC CONFIG REQUIRED)
find_package(protobuf CONFIG REQUIRED)

add_executable(DocoleChat)

set(sources
    src/ircclient.h
    src/ircclient.cpp
    src/ircserver.h
    src/ircserver.cpp
    src/main.cpp
    src/mainwindow.cpp
    src/mainwindow.h
    src/mainwindow.ui
    src/message.h
    src/message.cpp
    src/ChannelListWidget.h
    src/ChannelListWidget.cpp
    src/channelwidget.h
    src/channelwidget.cpp
    src/chatwidget.h
    src/chatwidget.cpp
    src/chatwidget.ui

    ../../proto/irc.proto
    ${CMAKE_CURRENT_BINARY_DIR}/irc.pb.cc
    ${CMAKE_CURRENT_BINARY_DIR}/irc.pb.h
    ${CMAKE_CURRENT_BINARY_DIR}/irc.grpc.pb.cc
    ${CMAKE_CURRENT_BINARY_DIR}/irc.grpc.pb.h
)
qt_wrap_ui(UI_HEADERS src/mainwindow.ui)
qt_wrap_ui(UI_HEADERS src/chatwidget.ui)
list(APPEND sources ${UI_HEADERS})

target_include_directories(DocoleChat
    PRIVATE
        ${CMAKE_CURRENT_BINARY_DIR}
        ${CMAKE_CURRENT_LIST_DIR}/src)
target_link_libraries(DocoleChat
        ${QT}::Core
        ${QT}::Widgets
        protobuf::libprotobuf
        gRPC::grpc
        gRPC::grpc++
        )
target_sources(DocoleChat PRIVATE ${sources})

get_target_property(grpc_cpp_plugin_location gRPC::grpc_cpp_plugin LOCATION)
protobuf_generate(
    TARGET DocoleChat
    LANGUAGE cpp
    IMPORT_DIRS "${CMAKE_CURRENT_LIST_DIR}/../../proto"
    PROTOC_OUT_DIR "${CMAKE_CURRENT_BINARY_DIR}")
protobuf_generate(
    TARGET DocoleChat
    LANGUAGE grpc
    GENERATE_EXTENSIONS .grpc.pb.h .grpc.pb.cc
    PLUGIN "protoc-gen-grpc=${grpc_cpp_plugin_location}"
    IMPORT_DIRS "${CMAKE_CURRENT_LIST_DIR}/../../proto")

#source_group(TREE ${CMAKE_CURRENT_SOURCE_DIR}/../.. FILES ${sources})
source_group(TREE ${CMAKE_CURRENT_SOURCE_DIR}/../.. FILES ${sources})

#[[
if (WIN32)
    set(DEBUG_SUFFIX)
    if (MSVC AND CMAKE_BUILD_TYPE MATCHES "Debug")
        set(DEBUG_SUFFIX "d")
    endif ()
    set(QT_INSTALL_PATH "${CMAKE_PREFIX_PATH}")
    if (NOT EXISTS "${QT_INSTALL_PATH}/bin")
        set(QT_INSTALL_PATH "${QT_INSTALL_PATH}/..")
        if (NOT EXISTS "${QT_INSTALL_PATH}/bin")
            set(QT_INSTALL_PATH "${QT_INSTALL_PATH}/..")
        endif ()
    endif ()
    if (EXISTS "${QT_INSTALL_PATH}/plugins/platforms/qwindows${DEBUG_SUFFIX}.dll")
        add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
                COMMAND ${CMAKE_COMMAND} -E make_directory
                "$<TARGET_FILE_DIR:${PROJECT_NAME}>/plugins/platforms/")
        add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
                COMMAND ${CMAKE_COMMAND} -E copy
                "${QT_INSTALL_PATH}/plugins/platforms/qwindows${DEBUG_SUFFIX}.dll"
                "$<TARGET_FILE_DIR:${PROJECT_NAME}>/plugins/platforms/")
    endif ()
    foreach (QT_LIB Core)
        add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
                COMMAND ${CMAKE_COMMAND} -E copy
                "${QT_INSTALL_PATH}/bin/${QT}${QT_LIB}${DEBUG_SUFFIX}.dll"
                "$<TARGET_FILE_DIR:${PROJECT_NAME}>")
    endforeach (QT_LIB)
endif ()]]
