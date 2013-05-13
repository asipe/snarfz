// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class EventHandlersTest : BaseTestCase {
    [Test]
    public void TestHandleDirectoryWithNoHandlers() {
      mHandlers.HandleDirectory(null);
    }

    [Test]
    public void TestHandleDirectoryWithHandlers() {
      var seen = new List<DirectoryVisitEventArgs>();

      for (var x = 0; x < 2; ++x)
        mHandlers.OnDirectory += (o, e) => {
          Assert.That(o, Is.SameAs(mConfig));
          seen.Add(e);
        };

      var arg = CA<DirectoryVisitEventArgs>();
      mHandlers.HandleDirectory(arg);
      Assert.That(seen, Is.EqualTo(BA(arg, arg)));
    }

    [Test]
    public void TestHandleError() {
      mHandlers.HandleError(null);
    }

    [Test]
    public void TestHandleErrorWithHandlers() {
      var seen = new List<ScanErrorEventArgs>();

      for (var x = 0; x < 2; ++x)
        mHandlers.OnError += (o, e) => {
          Assert.That(o, Is.SameAs(mConfig));
          seen.Add(e);
        };

      var arg = CA<ScanErrorEventArgs>();
      mHandlers.HandleError(arg);
      Assert.That(seen, Is.EqualTo(BA(arg, arg)));
    }

    [Test]
    public void TestHandleFileWithNoHandlers() {
      mHandlers.HandleFile(null);
    }

    [Test]
    public void TestHandleFileWithHandlers() {
      var seen = new List<FileVisitEventArgs>();

      for (var x = 0; x < 2; ++x)
        mHandlers.OnFile += (o, e) => {
          Assert.That(o, Is.SameAs(mConfig));
          seen.Add(e);
        };

      var arg = CA<FileVisitEventArgs>();
      mHandlers.HandleFile(arg);
      Assert.That(seen, Is.EqualTo(BA(arg, arg)));
    }

    [SetUp]
    public void DoSetup() {
      mConfig = new Config("");
      mHandlers = new EventHandlers(mConfig);
    }

    private EventHandlers mHandlers;
    private Config mConfig;
  }
}