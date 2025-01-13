cmake_minimum_required(VERSION 3.20.0 FATAL_ERROR)

project(frontend-cli)

if(LINUX)
    list(APPEND CMAKE_PREFIX_PATH $ENV{HOME}/.local)
endif()

set(CMAKE_CXX_STANDARD 17)
find_package(Protobuf CONFIG REQUIRED)
find_package(gRPC CONFIG REQUIRED)


add_executable(frontend-cli)




target_include_directories(frontend-cli PRIVATE
    ${CMAKE_CURRENT_BINARY_DIR}
    )
target_link_libraries(frontend-cli
        protobuf::libprotobuf
        gRPC::grpc
        gRPC::grpc++
        readline
        )

get_filename_component(proto "../../proto/irc.proto" ABSOLUTE)
get_filename_component(proto_path "${proto}" PATH)

# Generated sources
set(proto_srcs "${CMAKE_CURRENT_BINARY_DIR}/irc.pb.cc")
set(proto_hdrs "${CMAKE_CURRENT_BINARY_DIR}/irc.pb.h")
set(grpc_srcs "${CMAKE_CURRENT_BINARY_DIR}/irc.grpc.pb.cc")
set(grpc_hdrs "${CMAKE_CURRENT_BINARY_DIR}/irc.grpc.pb.h")

get_target_property(_GRPC_CPP_PLUGIN_EXECUTABLE gRPC::grpc_cpp_plugin LOCATION)
get_target_property(_PROTOBUF_PROTOC protobuf::protoc LOCATION)
message("_PROTOBUF_PROTOC ${_PROTOBUF_PROTOC}")
message("_GRPC_CPP_PLUGIN_EXECUTABLE ${_GRPC_CPP_PLUGIN_EXECUTABLE}")

add_custom_command(
      OUTPUT "${proto_srcs}" "${proto_hdrs}" "${grpc_srcs}" "${grpc_hdrs}"
      COMMAND ${_PROTOBUF_PROTOC}
      ARGS --grpc_out "${CMAKE_CURRENT_BINARY_DIR}"
        --cpp_out "${CMAKE_CURRENT_BINARY_DIR}"
        -I "${proto_path}"
        --plugin=protoc-gen-grpc="${_GRPC_CPP_PLUGIN_EXECUTABLE}"
        "${proto}"
      DEPENDS "${proto}")

set(sources src/main.cpp ${proto_srcs} ${proto_hdrs} ${grpc_srcs} ${grpc_hdrs})
target_sources(frontend-cli PRIVATE ${sources})

source_group(TREE ${CMAKE_CURRENT_SOURCE_DIR}/../.. FILES ${sources})
