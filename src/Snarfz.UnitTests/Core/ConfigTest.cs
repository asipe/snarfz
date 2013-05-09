﻿using System.Collections.Generic;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ConfigTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      Assert.That(mConfig.Root, Is.EqualTo("apath"));
      Assert.That(mConfig.ScanType, Is.EqualTo(ScanType.DirectoryOnly));
      Assert.That(mConfig.ScanErrorMode, Is.EqualTo(ScanErrorMode.Halt));
    }

    [Test]
    public void TestOnDirectoryEventHandlersRegistered() {
      var seen = new List<ScanEventArgs>();
      mConfig.OnDirectory += (o, e) => {
        Assert.That(o, Is.SameAs(mConfig));
        seen.Add(e);
      };
      var arg = CA<ScanEventArgs>();
      mConfig.Handlers.HandleDirectory(arg);
      Assert.That(seen, Is.EqualTo(BA(arg)));
    }

    [SetUp]
    public void DoSetup() {
      mConfig = new Config("apath");
    }

    private Config mConfig;
  }
}