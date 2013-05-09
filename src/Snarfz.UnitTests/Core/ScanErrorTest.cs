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
      var actual = Assert.Throws<ScanException>(() => mError.Handle(original));
      Assert.That(actual.InnerException, Is.EqualTo(original));
      Assert.That(actual.Message, Is.EqualTo("test ex"));
    }

    [Test]
    public void TestHandleSilencesWhenModeIsSilence() {
      mConfig.ScanErrorMode = ScanErrorMode.Ignore;
      var original = new Exception("test ex");
      mError.Handle(original);
    }

    [Test]
    public void TestHandleAsksHandlersWhenModeIsAskAndHandlerDoesNotThrow() {
      var original = new Exception("test ex");
      var handlerCalled = false;
      mConfig.ScanErrorMode = ScanErrorMode.Ask;
      mConfig.OnError += (o, a) => {
        AssertEqual(a, new ScanErrorEventArgs("", original));
        handlerCalled = true;
      };
      mError.Handle(original);
      Assert.That(handlerCalled, Is.True);
    }

    [Test]
    public void TestHandleAsksHandlersWhenModeIsAskedAndResultIsThrow() {
      var original = new Exception("test ex 1");
      mConfig.ScanErrorMode = ScanErrorMode.Ask;
      mConfig.OnError += (o, a) => {
        AssertEqual(a, new ScanErrorEventArgs("", original));
        throw new InvalidOperationException("test ex 2");
      };
      Assert.Throws<InvalidOperationException>(() => mError.Handle(original));
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