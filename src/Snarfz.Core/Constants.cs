// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace Snarfz.Core {
  public enum ScanType {
    All,
    DirectoriesOnly,
    FilesOnly
  }

  public enum ScanErrorMode {
    Throw,
    Ignore,
    Ask
  }

  public enum ScanErrorSource {
    Directory,
    File
  }

  public enum EventErrorMode {
    Throw,
    Ignore
  }
}