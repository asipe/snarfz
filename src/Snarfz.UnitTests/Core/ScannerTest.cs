using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScannerTest : SnarfzBaseTestCase {
    [Test]
    public void TestScanDirectoriesOnlyNoSubDirectories() {
      var config = new Config(_Root);
      var seen = new List<EvtArgs>();
      config.OnDirectory += (o, a) => seen.Add(a);
      mScanner.Start(config);
      AssertEqual(seen, BA(new EvtArgs(_Root)));
    }

    [SetUp]
    public void DoSetup() {
      mDirectory = Mok<IDirectory>();
      mScanner = new Scanner(mDirectory.Object);
    }

    private Mock<IDirectory> mDirectory;
    private Scanner mScanner;
    private const string _Root = @"c:\myfolder";
  }
}