//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (https://www.swig.org).
// Version 4.1.1
//
// Do not make changes to this file unless you know what you are doing - modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class LaserConfig : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal LaserConfig(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(LaserConfig obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(LaserConfig obj) {
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

  ~LaserConfig() {
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
          ydlidarPINVOKE.delete_LaserConfig(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public float min_angle {
    set {
      ydlidarPINVOKE.LaserConfig_min_angle_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_min_angle_get(swigCPtr);
      return ret;
    } 
  }

  public float max_angle {
    set {
      ydlidarPINVOKE.LaserConfig_max_angle_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_max_angle_get(swigCPtr);
      return ret;
    } 
  }

  public float angle_increment {
    set {
      ydlidarPINVOKE.LaserConfig_angle_increment_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_angle_increment_get(swigCPtr);
      return ret;
    } 
  }

  public float time_increment {
    set {
      ydlidarPINVOKE.LaserConfig_time_increment_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_time_increment_get(swigCPtr);
      return ret;
    } 
  }

  public float scan_time {
    set {
      ydlidarPINVOKE.LaserConfig_scan_time_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_scan_time_get(swigCPtr);
      return ret;
    } 
  }

  public float min_range {
    set {
      ydlidarPINVOKE.LaserConfig_min_range_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_min_range_get(swigCPtr);
      return ret;
    } 
  }

  public float max_range {
    set {
      ydlidarPINVOKE.LaserConfig_max_range_set(swigCPtr, value);
    } 
    get {
      float ret = ydlidarPINVOKE.LaserConfig_max_range_get(swigCPtr);
      return ret;
    } 
  }

  public LaserConfig() : this(ydlidarPINVOKE.new_LaserConfig(), true) {
  }

}
