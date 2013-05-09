using System;

namespace Snarfz.Core {
  public class DirectoryVisitEventArgs : EventArgs {
    public DirectoryVisitEventArgs(string path) {
      Path = path;
    }

    public string Path{get;private set;}
  }
}