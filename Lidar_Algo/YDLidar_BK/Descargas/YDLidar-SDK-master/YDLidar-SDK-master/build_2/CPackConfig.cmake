# This file will be configured to contain variables for CPack. These variables
# should be set in the CMake list file of the project before CPack module is
# included. The list of available CPACK_xxx variables and their associated
# documentation may be obtained using
#  cpack --help-variable-list
#
# Some variables are common to all generators (e.g. CPACK_PACKAGE_NAME)
# and some are specific to a generator
# (e.g. CPACK_NSIS_EXTRA_INSTALL_COMMANDS). The generator specific variables
# usually begin with CPACK_<GENNAME>_xxxx.


set(CPACK_BUILD_SOURCE_DIRS "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master;C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2")
set(CPACK_CMAKE_GENERATOR "Visual Studio 17 2022")
set(CPACK_COMPONENT_UNSPECIFIED_HIDDEN "TRUE")
set(CPACK_COMPONENT_UNSPECIFIED_REQUIRED "TRUE")
set(CPACK_DEBIAN_PACKAGE_DESCRIPTION "YDLIDAR SDK.")
set(CPACK_DEBIAN_PACKAGE_MAINTAINER "Tony Yang")
set(CPACK_DEBIAN_PACKAGE_PRIORITY "optional")
set(CPACK_DEBIAN_PACKAGE_SECTION "devel")
set(CPACK_DEBIAN_PACKAGE_SHLIBDEPS "OFF")
set(CPACK_DEFAULT_PACKAGE_DESCRIPTION_FILE "C:/Program Files/CMake/share/cmake-3.27/Templates/CPack.GenericDescription.txt")
set(CPACK_DEFAULT_PACKAGE_DESCRIPTION_SUMMARY "ydlidar_sdk built using CMake")
set(CPACK_GENERATOR "ZIP")
set(CPACK_INNOSETUP_ARCHITECTURE "x64")
set(CPACK_INSTALL_CMAKE_PROJECTS "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2;ydlidar_sdk;ALL;/")
set(CPACK_INSTALL_PREFIX "C:/Program Files (x86)/ydlidar_sdk")
set(CPACK_MODULE_PATH "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/cmake")
set(CPACK_NSIS_DISPLAY_NAME "ydlidar_sdk-1.1.15 1.1.15")
set(CPACK_NSIS_INSTALLER_ICON_CODE "")
set(CPACK_NSIS_INSTALLER_MUI_ICON_CODE "")
set(CPACK_NSIS_INSTALL_ROOT "$PROGRAMFILES64")
set(CPACK_NSIS_PACKAGE_NAME "ydlidar_sdk-1.1.15 1.1.15")
set(CPACK_NSIS_UNINSTALL_NAME "Uninstall")
set(CPACK_OUTPUT_CONFIG_FILE "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/CPackConfig.cmake")
set(CPACK_OUTPUT_FILE_PREFIX "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2")
set(CPACK_PACKAGE_DEFAULT_LOCATION "/")
set(CPACK_PACKAGE_DESCRIPTION_FILE "C:/Program Files/CMake/share/cmake-3.27/Templates/CPack.GenericDescription.txt")
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY "ydlidar_sdk built using CMake")
set(CPACK_PACKAGE_FILE_NAME "ydlidar_sdk-1.1.15")
set(CPACK_PACKAGE_INSTALL_DIRECTORY "ydlidar_sdk-1.1.15 1.1.15")
set(CPACK_PACKAGE_INSTALL_REGISTRY_KEY "ydlidar_sdk-1.1.15 1.1.15")
set(CPACK_PACKAGE_NAME "ydlidar_sdk-1.1.15")
set(CPACK_PACKAGE_RELOCATABLE "true")
set(CPACK_PACKAGE_VENDOR "Humanity")
set(CPACK_PACKAGE_VERSION "1.1.15")
set(CPACK_PACKAGE_VERSION_MAJOR "0")
set(CPACK_PACKAGE_VERSION_MINOR "1")
set(CPACK_PACKAGE_VERSION_PATCH "1")
set(CPACK_PACKAGING_INSTALL_PREFIX "C:/Program Files (x86)/ydlidar_sdk")
set(CPACK_RESOURCE_FILE_LICENSE "C:/Program Files/CMake/share/cmake-3.27/Templates/CPack.GenericLicense.txt")
set(CPACK_RESOURCE_FILE_README "C:/Program Files/CMake/share/cmake-3.27/Templates/CPack.GenericDescription.txt")
set(CPACK_RESOURCE_FILE_WELCOME "C:/Program Files/CMake/share/cmake-3.27/Templates/CPack.GenericWelcome.txt")
set(CPACK_SET_DESTDIR "true")
set(CPACK_SOURCE_GENERATOR "ZIP;TBZ2")
set(CPACK_SOURCE_OUTPUT_CONFIG_FILE "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/CPackSourceConfig.cmake")
set(CPACK_SOURCE_PACKAGE_FILE_NAME "ydlidar_sdk-1.1.15")
set(CPACK_SYSTEM_NAME "win64")
set(CPACK_THREADS "1")
set(CPACK_TOPLEVEL_TAG "win64")
set(CPACK_WIX_SIZEOF_VOID_P "8")

if(NOT CPACK_PROPERTIES_FILE)
  set(CPACK_PROPERTIES_FILE "C:/Users/tayde/Downloads/YDLidar-SDK-master/YDLidar-SDK-master/build_2/CPackProperties.cmake")
endif()

if(EXISTS ${CPACK_PROPERTIES_FILE})
  include(${CPACK_PROPERTIES_FILE})
endif()
