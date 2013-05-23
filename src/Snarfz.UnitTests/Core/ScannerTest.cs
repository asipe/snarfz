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
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.Directory, rootSubDirs[1], ex)).Throws<ScanException>();
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
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.Directory, rootSubDirs[0], ex));
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
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanAllNoSubDirectoriesSingleFile() {
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanAllNoSubDirectoriesMultipleFiles() {
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
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Throws(ex);
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.File, rootSubDirs[0], ex)).Throws<ScanException>();
      Assert.Throws<ScanException>(() => mScanner.Start(mConfig));
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanAllWithErrorInFileScanPrunesWhenScanErrorHandlerDoesNotThrow() {
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
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.File, rootSubDirs[1], ex));
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

    [Test]
    public void TestScanFilesOnlyNoSubDirectoriesNoFiles() {
      mConfig.ScanType = ScanType.FilesOnly;
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanFilesOnlyNoSubDirectoriesSingleFile() {
      mConfig.ScanType = ScanType.FilesOnly;
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanFilesOnlyNoSubDirectoriesMultipleFiles() {
      mConfig.ScanType = ScanType.FilesOnly;
      var rootFiles = BuildFilePaths(_Root, 3);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]),
                                new FileVisitEventArgs(rootFiles[2])));
    }

    [Test]
    public void TestScanFilesOnlyWithMultipleSubDirectories() {
      mConfig.ScanType = ScanType.FilesOnly;
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
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(subDir1Files[0]),
                                new FileVisitEventArgs(subDir1Files[1]),
                                new FileVisitEventArgs(subDir1Files[2])));
    }

    [Test]
    public void TestScanFilesOnlyWithErrorInFileScanThrowsWhenScanErrorHandlerThrows() {
      mConfig.ScanType = ScanType.FilesOnly;
      var ex = new Exception("test ex");
      var rootSubDirs = BuildDirPaths(_Root, 2);
      var rootFiles = BuildFilePaths(_Root, 1);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Throws(ex);
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.File, rootSubDirs[0], ex)).Throws<ScanException>();
      Assert.Throws<ScanException>(() => mScanner.Start(mConfig));
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanFilesOnlyWithErrorInFileScanPrunesWhenScanErrorHandlerDoesNotThrow() {
      mConfig.ScanType = ScanType.FilesOnly;
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
      mScanErrorHandler.Setup(h => h.Handle(mConfig, ScanErrorSource.File, rootSubDirs[1], ex));
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(subDir1SubDir0Files[0]),
                                new FileVisitEventArgs(subDir1SubDir0Files[1])));
    }

    [Test]
    public void TestScanDirectoriesOnlyRootIsPrunedDoesNotScanFilesNorSubDirectories() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      mConfig.OnDirectory += (o, a) => a.Prune = true;
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root) {Prune = true}));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanFilesOnlyFirstRootFileIsPrunedDoesNotNotifyOnOtherFiles() {
      mConfig.ScanType = ScanType.FilesOnly;
      mConfig.OnFile += (o, a) => a.Prune = true;
      var rootFiles = BuildFilePaths(_Root, 3);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]) {Prune = true}));
    }

    [Test]
    public void TestScanAllFirstRootFileIsPrunedDoesNotNotifyOnOtherFiles() {
      mConfig.OnFile += (o, a) => a.Prune = true;
      var rootFiles = BuildFilePaths(_Root, 3);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]) {Prune = true}));
    }

    [Test]
    public void TestScanFilesOnlySecondRootFileIsPrunedDoesNotNotifyOnOtherFiles() {
      mConfig.ScanType = ScanType.FilesOnly;
      var rootFiles = BuildFilePaths(_Root, 3);
      mConfig.OnFile += (o, a) => a.Prune = a.Path == rootFiles[1];
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]) {Prune = true}));
    }

    [Test]
    public void TestScanAllOnlySecondRootFileIsPrunedDoesNotNotifyOnOtherFiles() {
      var rootFiles = BuildFilePaths(_Root, 3);
      mConfig.OnFile += (o, a) => a.Prune = a.Path == rootFiles[1];
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]) {Prune = true}));
    }

    [Test]
    public void TestScanDirectoriesOnlySubDirIsPrunedDoesNotScanSubDirectoriesBelowIt() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir0Dirs = BuildDirPaths(rootSubDirs[0], 3);
      var subDir2Dirs = BuildDirPaths(rootSubDirs[2], 1);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[2])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(subDir2Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir2Dirs[0])).Returns(BA<string>());
      mConfig.OnDirectory += (o, a) => a.Prune = a.Path == rootSubDirs[1];
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[1]),
                               new DirectoryVisitEventArgs(subDir0Dirs[2]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]) {Prune = true},
                               new DirectoryVisitEventArgs(rootSubDirs[2]),
                               new DirectoryVisitEventArgs(subDir2Dirs[0])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectoriesOnlySubDirIsPrunedDeeperDoesNotScanSubDirectoriesBelowIt() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir0Dirs = BuildDirPaths(rootSubDirs[0], 3);
      var subDir1Dirs = BuildDirPaths(rootSubDirs[1], 3);
      var subDir2Dirs = BuildDirPaths(rootSubDirs[2], 1);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[2])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir1Dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1Dirs[2])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(subDir2Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir2Dirs[0])).Returns(BA<string>());
      mConfig.OnDirectory += (o, a) => a.Prune = a.Path == subDir1Dirs[1];
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[1]),
                               new DirectoryVisitEventArgs(subDir0Dirs[2]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1Dirs[0]),
                               new DirectoryVisitEventArgs(subDir1Dirs[1]) {Prune = true},
                               new DirectoryVisitEventArgs(subDir1Dirs[2]),
                               new DirectoryVisitEventArgs(rootSubDirs[2]),
                               new DirectoryVisitEventArgs(subDir2Dirs[0])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanDirectoriesOnlyMultipleSubDirIsPrunedDoesNotScanSubDirectoriesBelowPrunedDirectories() {
      mConfig.ScanType = ScanType.DirectoriesOnly;
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir1Dirs = BuildDirPaths(rootSubDirs[1], 3);
      var subDir2Dirs = BuildDirPaths(rootSubDirs[2], 1);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(subDir1Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir1Dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir1Dirs[2])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(subDir2Dirs);
      mDirectory.Setup(d => d.GetDirectories(subDir2Dirs[0])).Returns(BA<string>());
      mConfig.OnDirectory += (o, a) => a.Prune = (a.Path == subDir1Dirs[1]) || (a.Path == rootSubDirs[0]);
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]) {Prune = true},
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(subDir1Dirs[0]),
                               new DirectoryVisitEventArgs(subDir1Dirs[1]) {Prune = true},
                               new DirectoryVisitEventArgs(subDir1Dirs[2]),
                               new DirectoryVisitEventArgs(rootSubDirs[2]),
                               new DirectoryVisitEventArgs(subDir2Dirs[0])));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanFilesSubDirectoryFilePrunedDoesNotNotifyOtherFilesInThatSubDirectory() {
      mConfig.ScanType = ScanType.FilesOnly;
      var rootFiles = BuildFilePaths(_Root, 3);
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir0Files = BuildFilePaths(rootSubDirs[0], 2);
      var subDir1Files = BuildFilePaths(rootSubDirs[1], 3);
      var subDir2Files = BuildFilePaths(rootSubDirs[1], 5);

      mConfig.OnFile += (o, a) => a.Prune = (a.Path == rootFiles[1]) ||
                                            (a.Path == subDir0Files[0]) ||
                                            (a.Path == subDir2Files[3]);

      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Returns(subDir0Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[1], "*.*")).Returns(subDir1Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[2], "*.*")).Returns(subDir2Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(BA<string>());
      mScanner.Start(mConfig);
      Assert.That(mDirSeen, Is.Empty);
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]) {Prune = true},
                                new FileVisitEventArgs(subDir0Files[0]) {Prune = true},
                                new FileVisitEventArgs(subDir1Files[0]),
                                new FileVisitEventArgs(subDir1Files[1]),
                                new FileVisitEventArgs(subDir1Files[2]),
                                new FileVisitEventArgs(subDir2Files[0]),
                                new FileVisitEventArgs(subDir2Files[1]),
                                new FileVisitEventArgs(subDir2Files[2]),
                                new FileVisitEventArgs(subDir2Files[3]) {Prune = true}
                               ));
    }

    [Test]
    public void TestScanAllSubDirectoryFilePrunedDoesNotNotifyOtherFilesInThatSubDirectory() {
      var rootFiles = BuildFilePaths(_Root, 3);
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir0Files = BuildFilePaths(rootSubDirs[0], 2);
      var subDir1Files = BuildFilePaths(rootSubDirs[1], 3);
      var subDir2Files = BuildFilePaths(rootSubDirs[1], 5);

      mConfig.OnFile += (o, a) => a.Prune = (a.Path == rootFiles[1]) ||
                                            (a.Path == subDir0Files[0]) ||
                                            (a.Path == subDir2Files[3]);

      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Returns(subDir0Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[1], "*.*")).Returns(subDir1Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[2], "*.*")).Returns(subDir2Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(rootSubDirs[2])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]) {Prune = true},
                                new FileVisitEventArgs(subDir0Files[0]) {Prune = true},
                                new FileVisitEventArgs(subDir1Files[0]),
                                new FileVisitEventArgs(subDir1Files[1]),
                                new FileVisitEventArgs(subDir1Files[2]),
                                new FileVisitEventArgs(subDir2Files[0]),
                                new FileVisitEventArgs(subDir2Files[1]),
                                new FileVisitEventArgs(subDir2Files[2]),
                                new FileVisitEventArgs(subDir2Files[3]) {Prune = true}
                               ));
    }

    [Test]
    public void TestScanAllMixedPruning() {
      var rootFiles = BuildFilePaths(_Root, 3);
      var rootSubDirs = BuildDirPaths(_Root, 3);
      var subDir0Files = BuildFilePaths(rootSubDirs[0], 2);
      var subDir0Dirs = BuildDirPaths(rootSubDirs[0], 3);
      var subDir0Dir0Files = BuildFilePaths(subDir0Dirs[0], 3);
      var subDir1Files = BuildFilePaths(rootSubDirs[1], 3);
      var subDir2Files = BuildFilePaths(rootSubDirs[1], 5);

      mConfig.OnFile += (o, a) => a.Prune = (a.Path == rootFiles[1]) ||
                                            (a.Path == subDir0Files[0]) ||
                                            (a.Path == subDir2Files[3]) ||
                                            (a.Path == subDir0Dir0Files[2]);
      mConfig.OnDirectory += (o, a) => a.Prune = (a.Path == subDir0Dirs[1]);

      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(rootSubDirs);
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[0], "*.*")).Returns(subDir0Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[0])).Returns(subDir0Dirs);
      mDirectory.Setup(d => d.GetFiles(subDir0Dirs[0], "*.*")).Returns(subDir0Dir0Files);
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[0])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(subDir0Dirs[2], "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(subDir0Dirs[2])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[1], "*.*")).Returns(subDir1Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[1])).Returns(BA<string>());
      mDirectory.Setup(d => d.GetFiles(rootSubDirs[2], "*.*")).Returns(subDir2Files);
      mDirectory.Setup(d => d.GetDirectories(rootSubDirs[2])).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root),
                               new DirectoryVisitEventArgs(rootSubDirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[0]),
                               new DirectoryVisitEventArgs(subDir0Dirs[1]) {Prune = true},
                               new DirectoryVisitEventArgs(subDir0Dirs[2]),
                               new DirectoryVisitEventArgs(rootSubDirs[1]),
                               new DirectoryVisitEventArgs(rootSubDirs[2])));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1]) {Prune = true},
                                new FileVisitEventArgs(subDir0Files[0]) {Prune = true},
                                new FileVisitEventArgs(subDir0Dir0Files[0]),
                                new FileVisitEventArgs(subDir0Dir0Files[1]),
                                new FileVisitEventArgs(subDir0Dir0Files[2]) {Prune = true},
                                new FileVisitEventArgs(subDir1Files[0]),
                                new FileVisitEventArgs(subDir1Files[1]),
                                new FileVisitEventArgs(subDir1Files[2]),
                                new FileVisitEventArgs(subDir2Files[0]),
                                new FileVisitEventArgs(subDir2Files[1]),
                                new FileVisitEventArgs(subDir2Files[2]),
                                new FileVisitEventArgs(subDir2Files[3]) {Prune = true}
                               ));
    }

    [Test]
    public void TestScanWithDirectoryEventHandlingThrowingThrowsIfEventErrorHandlerThrows() {
      var original = new Exception("test ex");
      mConfig.OnDirectory += (o, a) => {throw original;};
      mEventErrorHandler.Setup(h => h.Handle(mConfig, original)).Throws(original);
      var ex = Assert.Throws<Exception>(() => mScanner.Start(mConfig));
      Assert.That(ex.Message, Is.EqualTo("test ex"));
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanWithDirectoryEventHandlingThrowingContinuesScanningIfEventErrorHandlerSilences() {
      var original = new Exception("test ex");
      mConfig.OnDirectory += (o, a) => {throw original;};
      mEventErrorHandler.Setup(h => h.Handle(mConfig, original));
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(BA<string>());
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      Assert.That(mFileSeen, Is.Empty);
    }

    [Test]
    public void TestScanWithFileEventHandlingThrowingThrowsIfEventErrorHandlerThrows() {
      var original = new Exception("test ex");
      var rootFiles = BuildFilePaths(_Root, 2);
      mConfig.OnFile += (o, a) => {throw original;};
      mEventErrorHandler.Setup(h => h.Handle(mConfig, original)).Throws(original);
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      var ex = Assert.Throws<Exception>(() => mScanner.Start(mConfig));
      Assert.That(ex.Message, Is.EqualTo("test ex"));
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0])));
    }

    [Test]
    public void TestScanWithFileEventHandlingThrowingContinuesScanningIfEventErrorHandlerSilences() {
      var original = new Exception("test ex");
      var rootFiles = BuildFilePaths(_Root, 2);
      mConfig.OnFile += (o, a) => {throw original;};
      mEventErrorHandler.Setup(h => h.Handle(mConfig, original));
      mDirectory.Setup(d => d.GetFiles(_Root, "*.*")).Returns(rootFiles);
      mDirectory.Setup(d => d.GetDirectories(_Root)).Returns(BA<string>());
      mScanner.Start(mConfig);
      AssertEqual(mDirSeen, BA(new DirectoryVisitEventArgs(_Root)));
      AssertEqual(mFileSeen, BA(new FileVisitEventArgs(rootFiles[0]),
                                new FileVisitEventArgs(rootFiles[1])));
    }

    [SetUp]
    public void DoSetup() {
      mDirSeen = new List<DirectoryVisitEventArgs>();
      mFileSeen = new List<FileVisitEventArgs>();
      mConfig = new Config(_Root);
      mConfig.OnDirectory += (o, a) => mDirSeen.Add(a);
      mConfig.OnFile += (o, a) => mFileSeen.Add(a);
      mDirectory = Mok<IDirectory>();
      mScanErrorHandler = Mok<IScanErrorHandler>();
      mEventErrorHandler = Mok<IEventErrorHandler>();
      mScanner = new Scanner(mDirectory.Object, mScanErrorHandler.Object, mEventErrorHandler.Object);
    }

    private Mock<IDirectory> mDirectory;
    private Scanner mScanner;
    private Config mConfig;
    private List<DirectoryVisitEventArgs> mDirSeen;
    private List<FileVisitEventArgs> mFileSeen;
    private Mock<IScanErrorHandler> mScanErrorHandler;
    private Mock<IEventErrorHandler> mEventErrorHandler;
    private const string _Root = @"c:\myfolder";
  }
}