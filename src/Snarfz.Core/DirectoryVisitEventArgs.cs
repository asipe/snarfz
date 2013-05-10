// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class DirectoryVisitEventArgs : EventArgs {
    public DirectoryVisitEventArgs(string path) {
      Path = path;
    }

    public string Path{get;private set;}
  }
}