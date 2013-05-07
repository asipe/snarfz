using System;

namespace Snarfz.Core {
  public class EvtArgs : EventArgs {
    public EvtArgs(string directory) {
      Directory = directory;
    }

    public string Directory{get;set;}
  }
}