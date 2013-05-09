using System;

namespace Snarfz.Core {
  public class ScanError {
    public ScanError(Config config) {
      mConfig = config;
    }

    public void Handle(Exception exception) {
      if (mConfig.ScanErrorMode == ScanErrorMode.Throw)
        throw new ScanException(exception.Message, exception);
    }

    private readonly Config mConfig;
  }
}