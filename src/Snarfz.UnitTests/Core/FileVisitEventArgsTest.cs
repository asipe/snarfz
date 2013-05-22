// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class FileVisitEventArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var args = new FileVisitEventArgs("apath");
      Assert.That(args.Path, Is.EqualTo("apath"));
      Assert.That(args.Prune, Is.False);
    }
  }
}