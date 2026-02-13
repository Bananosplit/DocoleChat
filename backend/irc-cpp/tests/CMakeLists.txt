cmake_minimum_required(VERSION 3.16.0 FATAL_ERROR)

project(irc-server-tests)

find_package(GTest CONFIG REQUIRED)

add_subdirectory(units)
