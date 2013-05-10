// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class EventHandlers {
    public EventHandlers(Config config) {
      mConfig = config;
    }

    public event EventHandler<DirectoryVisitEventArgs> OnDirectory;
    public event EventHandler<ScanErrorEventArgs> OnError;

    public void HandleDirectory(DirectoryVisitEventArgs args) {
      var handler = OnDirectory;
      if (handler != null)
        handler(mConfig, args);
    }

    public void HandleError(ScanErrorEventArgs args) {
      var handler = OnError;
      if (handler != null)
        handler(mConfig, args);
    }

    private readonly Config mConfig;
  }
}