using Moq;
using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScannerTest : BaseTestCase {
    [Test]
    public void TestScanDirectoriesOnlyNoSubDirectories() {
      var config = new Config(_Root);
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