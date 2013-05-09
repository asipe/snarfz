using System;

namespace Snarfz.Core {
  public class EventHandlers {
    public EventHandlers(Config config) {
      mConfig = config;
    }

    public event EventHandler<DirectoryVisitEventArgs> OnDirectory;

    public void HandleDirectory(DirectoryVisitEventArgs args) {
      var handler = OnDirectory;
      if (handler != null)
        handler(mConfig, args);
    }

    private readonly Config mConfig;
  }
}