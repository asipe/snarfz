using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public class Scanner {
    public Scanner(IDirectory directory) {
      mDirectory = directory;
    }

    public void Start(Config config) {
      ProcessDirectory(config, config.Root);
    }

    private void ProcessDirectory(Config config, string current) {
      config.HandleDirectory(new EvtArgs(current));
      foreach (var dir in mDirectory.GetDirectories(current))
        ProcessDirectory(config, dir);
    }

    private readonly IDirectory mDirectory;
  }
}