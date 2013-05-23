// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class BaseVisitEventArgsTest : BaseTestCase {
    private sealed class StubVisitEventArgs : BaseVisitEventArgs {
      public StubVisitEventArgs(string path) : base(path) {}
    }

    [Test]
    public void TestDefaults() {
      var args = new StubVisitEventArgs("apath");
      Assert.That(args.Path, Is.EqualTo("apath"));
      Assert.That(args.Prune, Is.False);
    }
  }
}