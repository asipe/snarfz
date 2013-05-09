using System;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScanErrorTest : BaseTestCase {
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

    [SetUp]
    public void DoSetup() {
      mConfig = CA<Config>();
      mError = new ScanError(mConfig);
    }

    private Config mConfig;
    private ScanError mError;
  }
}