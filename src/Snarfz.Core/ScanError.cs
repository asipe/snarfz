using System;

namespace Snarfz.Core {
  public class ScanError {
    public ScanError(Config config) {
      mConfig = config;
    }

    public void Handle(string currentDir, Exception exception) {
      switch (mConfig.ScanErrorMode) {
        case ScanErrorMode.Ask:
          AskHandlers(currentDir, exception);
          break;
        case ScanErrorMode.Throw:
          throw new ScanException(exception.Message, exception);
        default:
          return;
      }
    }

    private void AskHandlers(string currentDir, Exception exception) {
      mConfig.Handlers.HandleError(new ScanErrorEventArgs(currentDir, exception));
    }

    private readonly Config mConfig;
  }
}