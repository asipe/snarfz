// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScanErrorEventArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var ex = new Exception("");
      var args = new ScanErrorEventArgs("apath", ex);
      Assert.That(args.Path, Is.EqualTo("apath"));
      Assert.That(args.Exception, Is.EqualTo(ex));
    }
  }
}