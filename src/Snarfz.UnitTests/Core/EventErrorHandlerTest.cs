// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using Snarfz.Core;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class EventErrorHandlerTest : SnarfzBaseTestCase {
    [Test]
    public void TestHandleThrowsWhenModeIsThrow() {
      mConfig.EventErrorMode = EventErrorMode.Throw;
      var original = new Exception("test ex");
      var actual = Assert.Throws<ScanException>(() => mHandler.Handle(mConfig, original));
      Assert.That(actual.InnerException, Is.EqualTo(original));
      Assert.That(actual.Message, Is.EqualTo("test ex"));
    }

    [Test]
    public void TestHandleSilencesWhenModeIsSilence() {
      mConfig.EventErrorMode = EventErrorMode.Ignore;
      var original = new Exception("test ex");
      mHandler.Handle(mConfig, original);
    }

    [SetUp]
    public void DoSetup() {
      mConfig = CA<Config>();
      mHandler = new EventErrorHandler();
    }

    private Config mConfig;
    private EventErrorHandler mHandler;
  }
}