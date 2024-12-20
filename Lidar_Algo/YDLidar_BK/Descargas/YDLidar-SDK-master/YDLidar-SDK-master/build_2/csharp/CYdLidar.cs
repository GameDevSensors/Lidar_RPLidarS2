//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (https://www.swig.org).
// Version 4.1.1
//
// Do not make changes to this file unless you know what you are doing - modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class CYdLidar : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal CYdLidar(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(CYdLidar obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(CYdLidar obj) {
    if (obj != null) {
      if (!obj.swigCMemOwn)
        throw new global::System.ApplicationException("Cannot release ownership as memory is not owned");
      global::System.Runtime.InteropServices.HandleRef ptr = obj.swigCPtr;
      obj.swigCMemOwn = false;
      obj.Dispose();
      return ptr;
    } else {
      return new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
    }
  }

  ~CYdLidar() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          ydlidarPINVOKE.delete_CYdLidar(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public CYdLidar() : this(ydlidarPINVOKE.new_CYdLidar(), true) {
  }

  public bool initialize() {
    bool ret = ydlidarPINVOKE.CYdLidar_initialize(swigCPtr);
    return ret;
  }

  public void GetLidarVersion(LidarVersion version) {
    ydlidarPINVOKE.CYdLidar_GetLidarVersion(swigCPtr, LidarVersion.getCPtr(version));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool turnOn() {
    bool ret = ydlidarPINVOKE.CYdLidar_turnOn(swigCPtr);
    return ret;
  }

  public bool doProcessSimple(LaserScan outscan) {
    bool ret = ydlidarPINVOKE.CYdLidar_doProcessSimple(swigCPtr, LaserScan.getCPtr(outscan));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool turnOff() {
    bool ret = ydlidarPINVOKE.CYdLidar_turnOff(swigCPtr);
    return ret;
  }

  public void disconnecting() {
    ydlidarPINVOKE.CYdLidar_disconnecting(swigCPtr);
  }

  public string DescribeError() {
    string ret = ydlidarPINVOKE.CYdLidar_DescribeError(swigCPtr);
    return ret;
  }

  public DriverError getDriverError() {
    DriverError ret = (DriverError)ydlidarPINVOKE.CYdLidar_getDriverError(swigCPtr);
    return ret;
  }

  public bool setWorkMode(int mode, byte addr) {
    bool ret = ydlidarPINVOKE.CYdLidar_setWorkMode__SWIG_0(swigCPtr, mode, addr);
    return ret;
  }

  public bool setWorkMode(int mode) {
    bool ret = ydlidarPINVOKE.CYdLidar_setWorkMode__SWIG_1(swigCPtr, mode);
    return ret;
  }

  public void enableSunNoise(bool e) {
    ydlidarPINVOKE.CYdLidar_enableSunNoise__SWIG_0(swigCPtr, e);
  }

  public void enableSunNoise() {
    ydlidarPINVOKE.CYdLidar_enableSunNoise__SWIG_1(swigCPtr);
  }

  public void enableGlassNoise(bool e) {
    ydlidarPINVOKE.CYdLidar_enableGlassNoise__SWIG_0(swigCPtr, e);
  }

  public void enableGlassNoise() {
    ydlidarPINVOKE.CYdLidar_enableGlassNoise__SWIG_1(swigCPtr);
  }

  public bool getUserVersion(SWIGTYPE_p_std__string version) {
    bool ret = ydlidarPINVOKE.CYdLidar_getUserVersion(swigCPtr, SWIGTYPE_p_std__string.getCPtr(version));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void setBottomPriority(bool yes) {
    ydlidarPINVOKE.CYdLidar_setBottomPriority__SWIG_0(swigCPtr, yes);
  }

  public void setBottomPriority() {
    ydlidarPINVOKE.CYdLidar_setBottomPriority__SWIG_1(swigCPtr);
  }

  public bool getDeviceInfo(SWIGTYPE_p_device_info di) {
    bool ret = ydlidarPINVOKE.CYdLidar_getDeviceInfo__SWIG_0(swigCPtr, SWIGTYPE_p_device_info.getCPtr(di));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool getDeviceInfo(SWIGTYPE_p_std__vectorT_device_info_ex_t dis) {
    bool ret = ydlidarPINVOKE.CYdLidar_getDeviceInfo__SWIG_1(swigCPtr, SWIGTYPE_p_std__vectorT_device_info_ex_t.getCPtr(dis));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool setlidaropt(int optname, int value) {
    bool ret = ydlidarPINVOKE.CYdLidar_setlidaropt__SWIG_0(swigCPtr, optname, value);
    return ret;
  }

  public bool setlidaropt(int optname, float value) {
    bool ret = ydlidarPINVOKE.CYdLidar_setlidaropt__SWIG_1(swigCPtr, optname, value);
    return ret;
  }

  public bool setlidaropt(int optname, bool value) {
    bool ret = ydlidarPINVOKE.CYdLidar_setlidaropt__SWIG_2(swigCPtr, optname, value);
    return ret;
  }

  public bool setlidaropt(int optname, string value) {
    bool ret = ydlidarPINVOKE.CYdLidar_setlidaropt__SWIG_3(swigCPtr, optname, value);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool getlidaropt_toInt(int optname, SWIGTYPE_p_int optval) {
    bool ret = ydlidarPINVOKE.CYdLidar_getlidaropt_toInt(swigCPtr, optname, SWIGTYPE_p_int.getCPtr(optval));
    return ret;
  }

  public bool getlidaropt_toBool(int optname, SWIGTYPE_p_bool optval) {
    bool ret = ydlidarPINVOKE.CYdLidar_getlidaropt_toBool(swigCPtr, optname, SWIGTYPE_p_bool.getCPtr(optval));
    return ret;
  }

  public bool getlidaropt_toFloat(int optname, SWIGTYPE_p_float optval) {
    bool ret = ydlidarPINVOKE.CYdLidar_getlidaropt_toFloat(swigCPtr, optname, SWIGTYPE_p_float.getCPtr(optval));
    return ret;
  }

  public bool getlidaropt_toString(int optname, SWIGTYPE_p_std__string optval) {
    bool ret = ydlidarPINVOKE.CYdLidar_getlidaropt_toString(swigCPtr, optname, SWIGTYPE_p_std__string.getCPtr(optval));
    return ret;
  }

}
