//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (https://www.swig.org).
// Version 4.1.1
//
// Do not make changes to this file unless you know what you are doing - modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class LaserPointVector : global::System.IDisposable, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<LaserPoint>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal LaserPointVector(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(LaserPointVector obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(LaserPointVector obj) {
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

  ~LaserPointVector() {
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
          ydlidarPINVOKE.delete_LaserPointVector(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public LaserPointVector(global::System.Collections.IEnumerable c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (LaserPoint element in c) {
      this.Add(element);
    }
  }

  public LaserPointVector(global::System.Collections.Generic.IEnumerable<LaserPoint> c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (LaserPoint element in c) {
      this.Add(element);
    }
  }

  public bool IsFixedSize {
    get {
      return false;
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public LaserPoint this[int index]  {
    get {
      return getitem(index);
    }
    set {
      setitem(index, value);
    }
  }

  public int Capacity {
    get {
      return (int)capacity();
    }
    set {
      if (value < 0 || (uint)value < size())
        throw new global::System.ArgumentOutOfRangeException("Capacity");
      reserve((uint)value);
    }
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsSynchronized {
    get {
      return false;
    }
  }

  public void CopyTo(LaserPoint[] array)
  {
    CopyTo(0, array, 0, this.Count);
  }

  public void CopyTo(LaserPoint[] array, int arrayIndex)
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

  public void CopyTo(int index, LaserPoint[] array, int arrayIndex, int count)
  {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (index < 0)
      throw new global::System.ArgumentOutOfRangeException("index", "Value is less than zero");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (count < 0)
      throw new global::System.ArgumentOutOfRangeException("count", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (index+count > this.Count || arrayIndex+count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");
    for (int i=0; i<count; i++)
      array.SetValue(getitemcopy(index+i), arrayIndex+i);
  }

  public LaserPoint[] ToArray() {
    LaserPoint[] array = new LaserPoint[this.Count];
    this.CopyTo(array);
    return array;
  }

  global::System.Collections.Generic.IEnumerator<LaserPoint> global::System.Collections.Generic.IEnumerable<LaserPoint>.GetEnumerator() {
    return new LaserPointVectorEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new LaserPointVectorEnumerator(this);
  }

  public LaserPointVectorEnumerator GetEnumerator() {
    return new LaserPointVectorEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class LaserPointVectorEnumerator : global::System.Collections.IEnumerator
    , global::System.Collections.Generic.IEnumerator<LaserPoint>
  {
    private LaserPointVector collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public LaserPointVectorEnumerator(LaserPointVector collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public LaserPoint Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (LaserPoint)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object global::System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        currentObject = collectionRef[currentIndex];
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new global::System.InvalidOperationException("Collection modified.");
      }
    }

    public void Dispose() {
        currentIndex = -1;
        currentObject = null;
    }
  }

  public void Clear() {
    ydlidarPINVOKE.LaserPointVector_Clear(swigCPtr);
  }

  public void Add(LaserPoint x) {
    ydlidarPINVOKE.LaserPointVector_Add(swigCPtr, LaserPoint.getCPtr(x));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = ydlidarPINVOKE.LaserPointVector_size(swigCPtr);
    return ret;
  }

  private uint capacity() {
    uint ret = ydlidarPINVOKE.LaserPointVector_capacity(swigCPtr);
    return ret;
  }

  private void reserve(uint n) {
    ydlidarPINVOKE.LaserPointVector_reserve(swigCPtr, n);
  }

  public LaserPointVector() : this(ydlidarPINVOKE.new_LaserPointVector__SWIG_0(), true) {
  }

  public LaserPointVector(LaserPointVector other) : this(ydlidarPINVOKE.new_LaserPointVector__SWIG_1(LaserPointVector.getCPtr(other)), true) {
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public LaserPointVector(int capacity) : this(ydlidarPINVOKE.new_LaserPointVector__SWIG_2(capacity), true) {
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  private LaserPoint getitemcopy(int index) {
    LaserPoint ret = new LaserPoint(ydlidarPINVOKE.LaserPointVector_getitemcopy(swigCPtr, index), true);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private LaserPoint getitem(int index) {
    LaserPoint ret = new LaserPoint(ydlidarPINVOKE.LaserPointVector_getitem(swigCPtr, index), false);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, LaserPoint val) {
    ydlidarPINVOKE.LaserPointVector_setitem(swigCPtr, index, LaserPoint.getCPtr(val));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(LaserPointVector values) {
    ydlidarPINVOKE.LaserPointVector_AddRange(swigCPtr, LaserPointVector.getCPtr(values));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public LaserPointVector GetRange(int index, int count) {
    global::System.IntPtr cPtr = ydlidarPINVOKE.LaserPointVector_GetRange(swigCPtr, index, count);
    LaserPointVector ret = (cPtr == global::System.IntPtr.Zero) ? null : new LaserPointVector(cPtr, true);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, LaserPoint x) {
    ydlidarPINVOKE.LaserPointVector_Insert(swigCPtr, index, LaserPoint.getCPtr(x));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, LaserPointVector values) {
    ydlidarPINVOKE.LaserPointVector_InsertRange(swigCPtr, index, LaserPointVector.getCPtr(values));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    ydlidarPINVOKE.LaserPointVector_RemoveAt(swigCPtr, index);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    ydlidarPINVOKE.LaserPointVector_RemoveRange(swigCPtr, index, count);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public static LaserPointVector Repeat(LaserPoint value, int count) {
    global::System.IntPtr cPtr = ydlidarPINVOKE.LaserPointVector_Repeat(LaserPoint.getCPtr(value), count);
    LaserPointVector ret = (cPtr == global::System.IntPtr.Zero) ? null : new LaserPointVector(cPtr, true);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    ydlidarPINVOKE.LaserPointVector_Reverse__SWIG_0(swigCPtr);
  }

  public void Reverse(int index, int count) {
    ydlidarPINVOKE.LaserPointVector_Reverse__SWIG_1(swigCPtr, index, count);
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, LaserPointVector values) {
    ydlidarPINVOKE.LaserPointVector_SetRange(swigCPtr, index, LaserPointVector.getCPtr(values));
    if (ydlidarPINVOKE.SWIGPendingException.Pending) throw ydlidarPINVOKE.SWIGPendingException.Retrieve();
  }

}
