using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public class Scanner {
    public Scanner(IDirectory directory) {
      mDirectory = directory;
    }

    public void Start(Config config) {
      ProcessDirectory(config.Handlers, config.Root);
    }

    private void ProcessDirectory(EventHandlers handlers, string current) {
      handlers.HandleDirectory(new ScanEventArgs(current));
      foreach (var dir in mDirectory.GetDirectories(current))
        ProcessDirectory(handlers, dir);
    }

    private readonly IDirectory mDirectory;
  }
}