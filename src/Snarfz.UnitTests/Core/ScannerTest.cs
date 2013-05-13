// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

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
      mConfig.ScanType = ScanType.DirectoriesOnly;
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectoriesOnlyWithSubDirectories() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var dirs = BuildDirPaths(_Root, 3);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(dirs);
      mDirectory.Setup(d => d.GetDirectories(dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(dirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(dirs[2])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(dirs[0]),
                               new DirectoryVisitEventArgs(dirs[1]),
                               new DirectoryVisitEventArgs(dirs[2])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectoriesOnlyWithMultipleSubDirectories() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var subDir0SubDirs = BuildDirPaths(rootSubDirs[0], 1);
      var subDir1SubDirs = BuildDirPaths(rootSubDirs[1], 2);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[1])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectorisOnlyWithErrorDuringScanThrowsWhenScanErrorHandlerThrows() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var subDir0SubDirs = BuildDirPaths(rootSubDirs[0], 1);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Throws(ex);
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mErrorHandler.Setup(h => h.Handle(mConfig, rootSubDirs[1], ex)).Throws<ScanException>();
      Assert.Throws<ScanException>(() => mScanner.Start(mConfig));
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectoriesOnlyWithErrorDuringScanPrunesWhenScanErrorHandlerDoesNotThrow() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var subDir1SubDirs = BuildDirPaths(rootSubDirs[1], 2);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Throws(ex);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mErrorHandler.Setup(h => h.Handle(mConfig, rootSubDirs[0], ex));
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[1])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanAllNoSubDirectoriesNoFiles() {
      mConfig.ScanType = ScanType.All;
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanAllNoSubDirectoriesSingleFile() {
      mConfig.ScanType = ScanType.All;
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanAllNoSubDirectoriesMultipleFiles() {
      mConfig.ScanType = ScanType.All;
      var rootFiles = BuildFilePaths(_Root, 3);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]),
                                new FileVisitEventArgs(rootFiles[2])));
    }

    [Test]
    public void TestScanAllWithMultipleSubDirectories() {
      mConfig.ScanType = ScanType.All;
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var rootFiles = BuildFilePaths(_Root, 1);
      var subDir0SubDirs = BuildDirPaths(rootSubDirs[0], 1);
      var subDir0Files = BA<string>();
      var subDir1SubDirs = BuildDirPaths(rootSubDirs[1], 2);
      var subDir1Files = BuildFilePaths(rootSubDirs[1], 3);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Returns(subDir0Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[1], "*.*")).Returns(subDir1Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetFiles(subDir0SubDirs[0], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(subDir1SubDirs[0], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(subDir1SubDirs[1], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[1])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(subDir1Files[0]),
                                new FileVisitEventArgs(subDir1Files[1]),
                                new FileVisitEventArgs(subDir1Files[2])));
    }

    [Test]
    public void TestScanAllWithErrorInFileScanThrowsWhenScanErrorHandlerThrows() {
      mConfig.ScanType = ScanType.All;
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Throws(ex);
      mErrorHandler.Setup(h => h.Handle(mConfig, rootSubDirs[0], ex)).Throws<ScanException>();
      Assert.Throws<ScanException>(() => mScanner.Start(mConfig));
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanAllWithErrorInFileScanPrunesWhenScanErrorHandlerDoesNotThrow() {
      mConfig.ScanType = ScanType.All;
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var rootFiles = BuildFilePaths(_Root, 1);
      var subDir0SubDirs = BuildDirPaths(rootSubDirs[0], 1);
      var subDir0Files = BA<string>();
      var subDir1SubDirs = BuildDirPaths(rootSubDirs[1], 2);
      var subDir1SubDir0Files = BuildFilePaths(subDir1SubDirs[0], 2);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Returns(subDir0Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0SubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[1], "*.*")).Throws(ex);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1SubDirs);
      mDirectory.Setup(d => d.GetFiles(subDir0SubDirs[0], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(subDir1SubDirs[0], "*.*")).Returns(subDir1SubDir0Files);
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(subDir1SubDirs[1], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1SubDirs[1])).Returns(BA<string>());
      mErrorHandler.Setup(h => h.Handle(mConfig, rootSubDirs[1], ex));
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0SubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[0]),
                               new DirectoryVisitEventArgs(subDir1SubDirs[1])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(subDir1SubDir0Files[0]),
                                new FileVisitEventArgs(subDir1SubDir0Files[1])));
    }

    [SetUp]
    public void DoSetup() {
      mDirSeen = new List<DirectoryVisitEventArgs>();
      mFileSeen = new List<FileVisitEventArgs>();
      mConfig = new Config(_Root);
      mConfig.OnDirectory += (o, a) => mDirSeen.Add(a);
      mConfig.OnFile += (o, a) => mFileSeen.Add(a);
      mDirectory = Mok<IDirectory>();
      mErrorHandler = Mok<IScanErrorHandler>();
      mScanner = new Scanner(mDirectory.Object, mErrorHandler.Object);
    }

    private Mock<IDirectory> mDirectory;
    private Scanner mScanner;
    private Config mConfig;
    private List<DirectoryVisitEventArgs> mDirSeen;
    private List<FileVisitEventArgs> mFileSeen;
    private Mock<IScanErrorHandler> mErrorHandler;
    private const string _Root = @"c:\myfolder";
  }
}