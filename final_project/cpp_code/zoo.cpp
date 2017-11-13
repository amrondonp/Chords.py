/*
 * This inclusion should be put at the beginning.  It will include <Python.h>.
 */
#include <boost/python.hpp>
#include <string>

/*
 * This is the C++ function we write and want to expose to Python.
 */
const std::string hello()
{
    return std::string("hello, zoo");
}

/*
 * This is a macro Boost.Python provides to signify a Python extension module.
 */
BOOST_PYTHON_MODULE(zoo)
{
    // An established convention for using boost.python.
    using namespace boost::python;

    // Expose the function hello().
    def("hello", hello);
}
