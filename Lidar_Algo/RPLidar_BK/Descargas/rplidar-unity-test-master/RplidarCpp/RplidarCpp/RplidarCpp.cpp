// RplidarCpp.cpp: 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "rplidar.h"
#include <iostream>

#define _DLLEXPORT __declspec(dllexport)


using namespace rp::standalone::rplidar;


struct LidarData
{
	bool syncBit;
	float theta;
	float distant;
	UINT quality;
};


class LidarMgr
{
public:
	LidarMgr();
	~LidarMgr();

	const LidarMgr operator=(const LidarMgr&) = delete;
	const LidarMgr(const LidarMgr&) = delete;

public:
	static ILidarDriver * lidar_drv;
	bool m_isConnected = false;
	sl_lidar_response_device_info_t devinfo;

	bool m_onMotor = false;
	bool m_onScan = false;

	int onConnect(const char * port);
	bool onDisconnect();

	bool checkSLAMTECLIDARHealth(ILidarDriver* drv)
	{
		sl_result     op_result;
		sl_lidar_response_device_health_t healthinfo;

		op_result = drv->getHealth(healthinfo);
		if (SL_IS_OK(op_result)) { // the macro IS_OK is the preperred way to judge whether the operation is succeed.
			printf("SLAMTEC Lidar health status : %d\n", healthinfo.status);
			if (healthinfo.status == SL_LIDAR_STATUS_ERROR) {
				fprintf(stderr, "Error, slamtec lidar internal error detected. Please reboot the device to retry.\n");
				// enable the following code if you want slamtec lidar to be reboot by software
				// drv->reset();
				return false;
			}
			else {
				return true;
			}

		}
		else {
			fprintf(stderr, "Error, cannot retrieve the lidar health code: %x\n", op_result);
			return false;
		}
	}

	bool startMotor();

	bool startScan();


	bool endMotor();
	bool endScan();

	bool releaseDrv();

	bool grabData(LidarData * ptr) {

		if (!m_onScan) {
			return false;
		}
		sl_lidar_response_measurement_node_hq_t nodes[8192];
		size_t   count = _countof(nodes);

		

		u_result op_result = lidar_drv->grabScanDataHq(nodes, count);
		if (IS_OK(op_result)) {
			lidar_drv->ascendScanData(nodes, count);
			for (int pos = 0; pos < (int)count; ++pos) {
				ptr[pos].syncBit = nodes[pos].flag & SL_LIDAR_RESP_HQ_FLAG_SYNCBIT;
					ptr[pos].theta = (nodes[pos].angle_z_q14 * 90.f) / 16384.f;
					ptr[pos].distant = nodes[pos].dist_mm_q2 / 4.0f;
					ptr[pos].quality = nodes[pos].quality >> SL_LIDAR_RESP_MEASUREMENT_QUALITY_SHIFT;
			}

			return true;
		}

		return false;
	}


};

static LidarMgr s_lidarMgr;

ILidarDriver* LidarMgr::lidar_drv = nullptr;

LidarMgr::LidarMgr()
{

}



int LidarMgr::onConnect(const char * port)
{
	if (m_isConnected) return 0;
	if (lidar_drv == nullptr) {
		lidar_drv = *createLidarDriver();

		printf("lidar_drv created");
	}

	if (lidar_drv == nullptr) {
		return -20;
	}

	IChannel* _channel = (*createSerialPortChannel(port, 1000000));
	if (SL_IS_OK((lidar_drv)->connect(_channel))) {
		sl_result op_result = lidar_drv->getDeviceInfo(devinfo);

		if (SL_IS_OK(op_result))
		{
			m_isConnected = true;
		}
		else {
			delete lidar_drv;
			lidar_drv = NULL;
		}
	}

	return 0;
}

bool LidarMgr::onDisconnect()
{
	endScan();
	endMotor();

	if (m_isConnected) {
		if (lidar_drv != nullptr) {
			lidar_drv->stop();
			printf("lidar_drv is stop.");
			m_isConnected = false;
			return true;

		}
		else {
			printf("lidar_drv is null.");
		}
	}
	return false;
}

bool LidarMgr::startMotor()
{
	if (!m_isConnected) return false;
	if (m_onMotor) return true;

	lidar_drv->setMotorSpeed(600);
	m_onMotor = true;

	return true;
}

bool LidarMgr::startScan()
{
	if (!m_isConnected) return false;
	if (!m_onMotor) return false;
	if (m_onScan) return true;

	lidar_drv->startScan(0,1);
	m_onScan = true;
	return true;
}

bool LidarMgr::endMotor()
{
	if (!m_isConnected) return false;
	if (!m_onMotor) return true;
	if (m_onScan) {
		endScan();
	}
	if (m_onScan) return false;

	lidar_drv->setMotorSpeed(0);
	printf("lidar_drv stop motor");
	m_onMotor = false;

	return true;
}

bool LidarMgr::endScan()
{
	if (!m_isConnected) return false;
	if (!m_onScan) return true;
	lidar_drv->stop();
	printf("lidar_drv stop scan");
	m_onScan = false;

	return true;
}

bool LidarMgr::releaseDrv()
{
	//release
	if (lidar_drv != nullptr) {

		//lidar_drv->DisposeDriver(lidar_drv);
		lidar_drv = nullptr;

		printf("lidar_drv release driver.");

		return true;
	}

	return true;
}



LidarMgr::~LidarMgr()
{
	onDisconnect();
	releaseDrv();
}





extern "C" {


	_DLLEXPORT int OnConnect(const char * port) {
		if (port == nullptr) {
			return -30;
		}
		return s_lidarMgr.onConnect(port);
	}

	_DLLEXPORT bool OnDisconnect() {

		return s_lidarMgr.onDisconnect();
	}


	_DLLEXPORT bool StartMotor() {
		return s_lidarMgr.startMotor();
	}

	_DLLEXPORT bool StartScan() {
		return s_lidarMgr.startScan();
	}


	_DLLEXPORT bool EndMotor() {
		return s_lidarMgr.endMotor();
	}

	_DLLEXPORT bool EndScan() {
		return s_lidarMgr.endScan();
	}

	_DLLEXPORT bool ReleaseDrive() {
		return s_lidarMgr.releaseDrv();
	}


	_DLLEXPORT int GetLDataSize() {
		return sizeof(LidarData);
	}

	_DLLEXPORT void GetLDataSample(LidarData * data) {
		data->syncBit = false;
		data->theta = 0.542f;
		data->distant = 100.0;
		data->quality = 23;
	}

	_DLLEXPORT int GrabData(LidarData * data) {

		if (s_lidarMgr.grabData(data)) {
			return 720;
		}
		else
		{
			return 0;
		}
	}

	_DLLEXPORT void GetLDataSampleArray(LidarData * data) {

		data[0].syncBit = false;
		data[0].theta = 0.542f;
		data[0].distant = 100.0;
		data[0].quality = 23;

		data[1].syncBit = true;
		data[1].theta = 2.333f;
		data[1].distant = 50.001f;
		data[1].quality = 7;
	}
}

