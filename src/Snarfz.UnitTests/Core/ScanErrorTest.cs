using System;
using NUnit.Framework;
using Snarfz.Core;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScanErrorTest : SnarfzBaseTestCase {
    [Test]
    public void TestHandleThrowsWhenModeIsThrow() {
      mConfig.ScanErrorMode = ScanErrorMode.Throw;
      var original = new Exception("test ex");
      var actual = Assert.Throws<ScanException>(() => mError.Handle("", original));
      Assert.That(actual.InnerException, Is.EqualTo(original));
      Assert.That(actual.Message, Is.EqualTo("test ex"));
    }

    [Test]
    public void TestHandleSilencesWhenModeIsSilence() {
      mConfig.ScanErrorMode = ScanErrorMode.Ignore;
      var original = new Exception("test ex");
      mError.Handle("", original);
    }

    [Test]
    public void TestHandleAsksHandlersWhenModeIsAskAndHandlerDoesNotThrow() {
      var original = new Exception("test ex");
      var handlerCalled = false;
      mConfig.ScanErrorMode = ScanErrorMode.Ask;
      mConfig.OnError += (o, a) => {
        AssertEqual(a, new ScanErrorEventArgs(@"some\path", original));
        handlerCalled = true;
      };
      mError.Handle(@"some\path", original);
      Assert.That(handlerCalled, Is.True);
    }

    [Test]
    public void TestHandleAsksHandlersWhenModeIsAskedAndResultIsThrow() {
      var original = new Exception("test ex 1");
      mConfig.ScanErrorMode = ScanErrorMode.Ask;
      mConfig.OnError += (o, a) => {
        AssertEqual(a, new ScanErrorEventArgs(@"some\path", original));
        throw new InvalidOperationException("test ex 2");
      };
      Assert.Throws<InvalidOperationException>(() => mError.Handle(@"some\path", original));
    }

    [SetUp]
    public void DoSetup() {
      mConfig = CA<Config>();
      mError = new ScanError(mConfig);
    }

    private Config mConfig;
    private ScanError mError;
  }
}