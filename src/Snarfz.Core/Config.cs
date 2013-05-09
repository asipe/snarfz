using System;

namespace Snarfz.Core {
  public class Config {
    public Config(string root) {
      Root = root;
      Handlers = new EventHandlers(this);
    }

    public string Root{get;private set;}
    public ScanType ScanType{get;set;}
    public EventHandlers Handlers{get;private set;}

    public event EventHandler<ScanEventArgs> OnDirectory {
      add {Handlers.OnDirectory += value;}
      remove {Handlers.OnDirectory -= value;}
    }
  }
}