using System;

namespace Snarfz.Core {
  public class EventErrorHandler : IEventErrorHandler {
    public void Handle(Config config, Exception exception) {
      switch (config.EventErrorMode) {
        case EventErrorMode.Throw:
          throw new ScanException(exception.Message, exception);
        default:
          return;
      }
    }
  }
}