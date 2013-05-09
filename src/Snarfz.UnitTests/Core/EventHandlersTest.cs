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

    [SetUp]
    public void DoSetup() {
      mConfig = new Config("");
      mHandlers = new EventHandlers(mConfig);
    }

    private EventHandlers mHandlers;
    private Config mConfig;
  }
}