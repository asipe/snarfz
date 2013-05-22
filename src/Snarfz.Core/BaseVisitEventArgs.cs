using System;

namespace Snarfz.Core {
  public abstract class BaseVisitEventArgs : EventArgs {
    protected BaseVisitEventArgs(string path) {
      Path = path;
    }

    public string Path{get;private set;}
    public bool Prune{get;set;}
  }
}