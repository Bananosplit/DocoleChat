FROM ubuntu:22.04
ENV http_proxy http://i.zhilenkov:Roxa12@proxy1.module.ru:3128
ENV https_proxy $http_proxy
RUN apt-get update
RUN apt-get install -y qt6-base-dev
RUN apt-get install -y cmake g++
RUN apt-get install -y libglx-dev libgl1-mesa-dev
RUN apt-get install -y apt-utils libgrpc++1
WORKDIR .
COPY src src
COPY CMakeLists.txt CMakeLists.txt
RUN cmake -B build .
RUN cmake --build build