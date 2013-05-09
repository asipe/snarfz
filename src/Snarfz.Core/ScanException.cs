using System;
using System.Runtime.Serialization;

namespace Snarfz.Core {
  [Serializable]
  public class ScanException : SnarfzException {
    internal ScanException() {}
    internal ScanException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    internal ScanException(string msg) : base(msg) {}
    internal ScanException(string msg, Exception e) : base(msg, e) {}
    internal ScanException(string msg, params object[] fmt) : base(string.Format(msg, fmt)) {}
  }
}