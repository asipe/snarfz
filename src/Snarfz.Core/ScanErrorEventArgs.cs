// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class ScanErrorEventArgs : EventArgs {
    public ScanErrorEventArgs(ScanErrorSource source, string path, Exception exception) {
      Source = source;
      Path = path;
      Exception = exception;
    }

    public ScanErrorSource Source{get;private set;}
    public string Path{get;private set;}
    public Exception Exception{get;private set;}
  }
}