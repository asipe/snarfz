using System;
using System.Runtime.Serialization;

namespace Snarfz.Core {
  [Serializable]
  public abstract class SnarfzException : Exception {
    internal SnarfzException() {}
    internal SnarfzException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    internal SnarfzException(string msg) : base(msg) {}
    internal SnarfzException(string msg, Exception e) : base(msg, e) {}
    internal SnarfzException(string msg, params object[] fmt) : base(string.Format(msg, fmt)) {}
  }
}