// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public static class Snarfzer {
    public static Scanner NewScanner() {
      return new Scanner(new DotNetDirectory(), new ScanErrorHandler());
    }
  }
}