// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class ScanErrorHandler : IScanErrorHandler {
    public void Handle(Config config, string currentDir, Exception exception) {
      switch (config.ScanErrorMode) {
        case ScanErrorMode.Ask:
          AskHandlers(config, currentDir, exception);
          break;
        case ScanErrorMode.Throw:
          throw new ScanException(exception.Message, exception);
        default:
          return;
      }
    }

    private static void AskHandlers(Config config, string currentDir, Exception exception) {
      config.Handlers.HandleError(new ScanErrorEventArgs(currentDir, exception));
    }
  }
}