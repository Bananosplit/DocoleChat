#include "parse.h"
#include <boost/regex.hpp>
#include <sstream>

Message parse_msg(const std::string &msg)
{
    Message result;
    std::stringstream ss(msg);

    if(msg[0] == ':'){
        char deliter;
        std::string prefix;
        std::getline(ss, prefix, ' ');

        boost::regex pattern(":([a-zA-Z][\d\w]*)(![\S]*)?(@[\w])?");

        boost::smatch results;

        std::string::const_iterator start = prefix.begin();
        std::string::const_iterator end = prefix.end();

        ss >> deliter;
        std::getline(ss, result.name, '!');
        ss >> deliter;

        // result
    }

    return result;
}
