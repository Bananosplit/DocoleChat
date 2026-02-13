#include <gtest/gtest.h>
#include "parse.h"

// Demonstrate some basic assertions.
TEST(Parse, BasicAssertions) {
    Message message;
    message = parse_msg(":nick!user@host PRIVMSG :Йоу, чё как?");
    EXPECT_STRCASEEQ(message.name.c_str(), "nick");

    message = parse_msg(":nick");
    EXPECT_STRCASEEQ(message.name.c_str(), "nick");
}
