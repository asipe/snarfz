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
      config.Handlers.HandleDirectory(new DirectoryVisitEventArgs(currentDir));
      foreach (var dir in GetSubDirectories(currentDir))
        ProcessDirectory(config, dir);
    }

    private IEnumerable<string> GetSubDirectories(string currentDir) {
      try {
        return mDirectory.GetDirectories(currentDir);
      } catch (Exception e) {
        mErrorHandler.Handle(currentDir, e);
      }
      return Enumerable.Empty<string>();
    }

    private readonly IDirectory mDirectory;
    private readonly IScanErrorHandler mErrorHandler;
  }
}