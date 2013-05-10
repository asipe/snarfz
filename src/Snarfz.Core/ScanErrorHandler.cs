// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class ScanErrorHandler : IScanErrorHandler {
    public ScanErrorHandler(Config config) {
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