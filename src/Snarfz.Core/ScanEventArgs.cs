using System;

namespace Snarfz.Core {
  public class ScanEventArgs : EventArgs {
    public ScanEventArgs(string path) {
      Path = path;
    }

    public string Path{get;private set;}
  }
}