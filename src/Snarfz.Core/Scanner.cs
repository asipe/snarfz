// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public class Scanner {
    public Scanner(IDirectory directory, IScanErrorHandler errorHandler) {
      mDirectory = directory;
      mErrorHandler = errorHandler;
    }

    public void Start(Config config) {
      ProcessDirectory(config, config.Root);
    }

    private void ProcessDirectory(Config config, string currentDir) {
      NotifyOnDirectory(config, currentDir);

      if (mPruned)
        return;

      ProcessFilesForDir(config, currentDir);
      ProcessSubDirectoriesForDir(config, currentDir);
    }

    private void NotifyOnDirectory(Config config, string currentDir) {
      if (config.ScanType == ScanType.FilesOnly)
        return;

      var args = new DirectoryVisitEventArgs(currentDir);
      config.Handlers.HandleDirectory(args);
      mPruned = args.Prune;
    }
    
    private void ProcessSubDirectoriesForDir(Config config, string currentDir) {
      foreach (var dir in GetSubDirectories(config, currentDir))
        ProcessDirectory(config, dir);
    }

    private void ProcessFilesForDir(Config config, string dir) {
      if (config.ScanType == ScanType.DirectoriesOnly)
        return;

      foreach (var file in GetFile(config, dir)) {
        var args = new FileVisitEventArgs(file);
        config.Handlers.HandleFile(args);
        if (args.Prune)
          break;
      }
    }

    private IEnumerable<string> GetFile(Config config, string currentDir) {
      return GetItems(config, ScanErrorSource.File, currentDir, () => mDirectory.GetFiles(currentDir, "*.*"));
    }

    private IEnumerable<string> GetSubDirectories(Config config, string currentDir) {
      return GetItems(config, ScanErrorSource.Directory, currentDir, () => mDirectory.GetDirectories(currentDir));
    }

    private IEnumerable<string> GetItems(Config config, ScanErrorSource source, string currentPath, Func<IEnumerable<string>> getter) {
      try {
        return getter();
      } catch (Exception e) {
        mErrorHandler.Handle(config, source, currentPath, e);
      }
      return Enumerable.Empty<string>();
    }

    private readonly IDirectory mDirectory;
    private readonly IScanErrorHandler mErrorHandler;
    private bool mPruned;
  }
}