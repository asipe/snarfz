using System;

namespace Snarfz.Core {
  public class Config {
    public Config(string root) {
      Root = root;
    }

    public string Root{get;private set;}
    public ScanType ScanType{get;set;}
    public event EventHandler<EvtArgs> OnDirectory;

    public void HandleDirectory(EvtArgs e) {
      var handler = OnDirectory;
      if (handler != null)
        handler(this, e);
    }
  }
}