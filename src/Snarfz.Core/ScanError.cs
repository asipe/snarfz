using System;

namespace Snarfz.Core {
  public class ScanError {
    public ScanError(Config config) {
      mConfig = config;
    }

    public void Handle(Exception exception) {
      switch (mConfig.ScanErrorMode) {
        case ScanErrorMode.Ask:
          AskHandlers(exception);
          break;
        case ScanErrorMode.Throw:
          throw new ScanException(exception.Message, exception);
        default:
          return;
      }
    }

    private void AskHandlers(Exception exception) {
      mConfig.Handlers.HandleError(new ScanErrorEventArgs("", exception));
    }

    private readonly Config mConfig;
  }
}