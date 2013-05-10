// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ConfigTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      Assert.That(mConfig.Root, Is.EqualTo("apath"));
      Assert.That(mConfig.ScanType, Is.EqualTo(ScanType.All));
      Assert.That(mConfig.ScanErrorMode, Is.EqualTo(ScanErrorMode.Throw));
    }

    [Test]
    public void TestOnDirectoryEventHandlersRegistered() {
      var seen = new List<DirectoryVisitEventArgs>();
      mConfig.OnDirectory += (o, e) => {
        Assert.That(o, Is.SameAs(mConfig));
        seen.Add(e);
      };
      var arg = CA<DirectoryVisitEventArgs>();
      mConfig.Handlers.HandleDirectory(arg);
      Assert.That(seen, Is.EqualTo(BA(arg)));
    }

    [Test]
    public void TestOnErrorEventHandlersRegistered() {
      var seen = new List<ScanErrorEventArgs>();
      mConfig.OnError += (o, e) => {
        Assert.That(o, Is.SameAs(mConfig));
        seen.Add(e);
      };
      var arg = CA<ScanErrorEventArgs>();
      mConfig.Handlers.HandleError(arg);
      Assert.That(seen, Is.EqualTo(BA(arg)));
    }

    [SetUp]
    public void DoSetup() {
      mConfig = new Config("apath");
    }

    private Config mConfig;
  }
}