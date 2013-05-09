using System;

namespace Snarfz.Core {
  public class ScanErrorEventArgs : EventArgs {
    public ScanErrorEventArgs(string path, Exception exception) {
      Path = path;
      Exception = exception;
    }

    public string Path{get;private set;}
    public Exception Exception{get;private set;}
  }
}