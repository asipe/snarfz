// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public interface IScanErrorHandler {
    void Handle(Config config, ScanErrorSource source, string currentPath, Exception exception);
  }
}