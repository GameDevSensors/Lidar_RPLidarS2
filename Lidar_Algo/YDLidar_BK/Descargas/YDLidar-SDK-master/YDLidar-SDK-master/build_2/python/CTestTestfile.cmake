# CMake generated Testfile for 
# Source directory: C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python
# Build directory: C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/python
# 
# This file includes the relevant testing commands required for 
# testing this directory and lists subdirectories to be tested as well.
if(CTEST_CONFIGURATION_TYPE MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  add_test(ydlidar_py_test "C:/Program Files/Python312/python.exe" "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/test/pytest.py")
  set_tests_properties(ydlidar_py_test PROPERTIES  ENVIRONMENT "PYTHONPATH=:C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/python" _BACKTRACE_TRIPLES "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;42;add_test;C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;0;")
elseif(CTEST_CONFIGURATION_TYPE MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  add_test(ydlidar_py_test "C:/Program Files/Python312/python.exe" "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/test/pytest.py")
  set_tests_properties(ydlidar_py_test PROPERTIES  ENVIRONMENT "PYTHONPATH=:C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/python" _BACKTRACE_TRIPLES "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;42;add_test;C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;0;")
elseif(CTEST_CONFIGURATION_TYPE MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  add_test(ydlidar_py_test "C:/Program Files/Python312/python.exe" "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/test/pytest.py")
  set_tests_properties(ydlidar_py_test PROPERTIES  ENVIRONMENT "PYTHONPATH=:C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/python" _BACKTRACE_TRIPLES "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;42;add_test;C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;0;")
elseif(CTEST_CONFIGURATION_TYPE MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
  add_test(ydlidar_py_test "C:/Program Files/Python312/python.exe" "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/test/pytest.py")
  set_tests_properties(ydlidar_py_test PROPERTIES  ENVIRONMENT "PYTHONPATH=:C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/python" _BACKTRACE_TRIPLES "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;42;add_test;C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/python/CMakeLists.txt;0;")
else()
  add_test(ydlidar_py_test NOT_AVAILABLE)
endif()
subdirs("examples")
