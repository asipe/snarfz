// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class ScanErrorEventArgs : EventArgs {
    public ScanErrorEventArgs(string path, Exception exception) {
      Path = path;
      Exception = exception;
    }

    public string Path{get;private set;}
    public Exception Exception{get;private set;}
  }
}