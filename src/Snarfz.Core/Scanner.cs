using System;
using System.Collections.Generic;
using System.Linq;
using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public class Scanner {
    public Scanner(IDirectory directory) {
      mDirectory = directory;
    }

    public void Start(Config config) {
      ProcessDirectory(config, config.Root);
    }

    private void ProcessDirectory(Config config, string currentDir) {
      config.Handlers.HandleDirectory(new DirectoryVisitEventArgs(currentDir));
      foreach (var dir in GetSubDirectories(config, currentDir))
        ProcessDirectory(config, dir);
    }

    private IEnumerable<string> GetSubDirectories(Config config, string currentDir) {
      try {
        return mDirectory.GetDirectories(currentDir);
      } catch (Exception e) {
        new ScanError(config).Handle(e);
      }
      return Enumerable.Empty<string>();
    }

    private readonly IDirectory mDirectory;
  }
}