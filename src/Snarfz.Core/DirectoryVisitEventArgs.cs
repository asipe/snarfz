// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Snarfz.Core {
  public class DirectoryVisitEventArgs : BaseVisitEventArgs {
    public DirectoryVisitEventArgs(string path) : base(path) {}
  }
}