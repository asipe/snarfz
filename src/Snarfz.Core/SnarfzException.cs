using System;
using System.Runtime.Serialization;

namespace Snarfz.Core {
  [Serializable]
  public abstract class SnarfzException : Exception {
    protected SnarfzException() {}
    protected SnarfzException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    protected SnarfzException(string msg) : base(msg) {}
    protected SnarfzException(string msg, Exception e) : base(msg, e) {}
    protected SnarfzException(string msg, params object[] fmt) : base(string.Format(msg, fmt)) {}
  }
}