// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public abstract class BaseVisitEventArgs : EventArgs {
    protected BaseVisitEventArgs(string path) {
      Path = path;
    }

    public string Path{get;private set;}
    public bool Prune{get;set;}
  }
}