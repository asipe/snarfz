using System;

namespace Snarfz.Core {
  public class ScanEventArgs : EventArgs {
    public ScanEventArgs(string directory) {
      Directory = directory;
    }

    public string Directory{get;set;}
  }
}