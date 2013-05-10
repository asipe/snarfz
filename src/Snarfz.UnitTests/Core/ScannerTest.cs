using System;
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
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mSeen, BA(new DirectoryVisitEventArgs(_Root)));
    }

    [Test]
    public void TestScanDirectoriesOnlyWithSubDirectories() {
      var dirs = BuildPaths(_Root, 3);

      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(dirs);
      mDirectory.Setup(d => d.GetDirectories(dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(dirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(dirs[2])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mSeen, BA(new DirectoryVisitEventArgs(_Root),
                            new DirectoryVisitEventArgs(dirs[0]),
                            new DirectoryVisitEventArgs(dirs[1]),
                            new DirectoryVisitEventArgs(dirs[2])));
    }

    [Test]
    public void TestScanDirectoriesOnlyWithMultipleSubDirectories() {
      var rootSubDirs = BuildPaths(_Root, 2);
      var subDir0SubDirs = BuildPaths(rootSubDirs[0], 1);
      var subDir1SubDirs = BuildPaths(rootSubDirs[1], 2);

      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mSeen, BA(new DirectoryVisitEventArgs(_Root),
                            new DirectoryVisitEventArgs(rootSubDirs[0]),
                            new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                            new DirectoryVisitEventArgs(rootSubDirs[1]),
                            new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                            new DirectoryVisitEventArgs(subDir1SubDirs[1])));
    }

    [Test]
    public void TestScanDirectorisOnlyWithErrorDuringScanThrowsWhenScanErrorHandlerThrows() {
      var ex = new Exception("test ex");
      var rootSubDirs = BuildPaths(_Root, 2);
      var subDir0SubDirs = BuildPaths(rootSubDirs[0], 1);

      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Throws(ex);
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mErrorHandler.Setup(h => h.Handle(rootSubDirs[1], ex)).Throws<ScanException>();
      Assert.Throws<ScanException>(() => mScanner.Start(mConfig));
      AssertEqual(mSeen, BA(new DirectoryVisitEventArgs(_Root),
                            new DirectoryVisitEventArgs(rootSubDirs[0]),
                            new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                            new DirectoryVisitEventArgs(rootSubDirs[1])));
    }

    [Test]
    public void TestScanDirectoriesOnlyWithErrorDuringScanPrunesWhenScanErrorHandlerDoesNotThrow() {
      var ex = new Exception("test ex");
      var rootSubDirs = BuildPaths(_Root, 2);
      var subDir1SubDirs = BuildPaths(rootSubDirs[1], 2);

      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Throws(ex);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mErrorHandler.Setup(h => h.Handle(rootSubDirs[0], ex));
      mScanner.Start(mConfig);
      AssertEqual(mSeen, BA(new DirectoryVisitEventArgs(_Root),
                            new DirectoryVisitEventArgs(rootSubDirs[0]),
                            new DirectoryVisitEventArgs(rootSubDirs[1]),
                            new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                            new DirectoryVisitEventArgs(subDir1SubDirs[1])));
    }

    [SetUp]
    public void DoSetup() {
      mSeen = new List<DirectoryVisitEventArgs>();
      mConfig = new Config(_Root);
      mConfig.OnDirectory += (o, a) => mSeen.Add(a);
      mDirectory = Mok<IDirectory>();
      mErrorHandler = Mok<IScanErrorHandler>();
      mScanner = new Scanner(mDirectory.Object, mErrorHandler.Object);
    }

    private Mock<IDirectory> mDirectory;
    private Scanner mScanner;
    private Config mConfig;
    private List<DirectoryVisitEventArgs> mSeen;
    private Mock<IScanErrorHandler> mErrorHandler;
    private const string _Root = @"c:\myfolder";
  }
}