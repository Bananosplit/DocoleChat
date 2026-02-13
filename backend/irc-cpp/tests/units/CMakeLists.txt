cmake_minimum_required(VERSION 3.16.0 FATAL_ERROR)

project(unit_tests)

add_executable(
    unit_tests
    test_parse.cpp
)
target_link_libraries(
  unit_tests
  GTest::gtest_main
  irc-server-lib
)
target_include_directories(unit_tests PRIVATE ${CMAKE_CURRENT_LIST_DIR}/../../src)

include(GoogleTest)
gtest_discover_tests(unit_tests)
