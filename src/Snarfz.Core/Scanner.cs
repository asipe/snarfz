using SupaCharge.Core.IOAbstractions;

namespace Snarfz.Core {
  public class Scanner {
    public Scanner(IDirectory directory) {
      mDirectory = directory;
    }

    public void Start(Config config) {
    }

    private readonly IDirectory mDirectory;
  }
}