using System;
using Snarfz.Core;
using SupaCharge.Core.IOAbstractions;

namespace DirScan {
  internal class Program {
    private static void Main(string[] args) {
      try {
        Execute(args[0]);
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }

    private static void Execute(string root) {
      Console.WriteLine("Scanning {0}", root);
      Console.WriteLine("----");

      var config = new Config(root);
      config.OnDirectory += (e, a) => Console.WriteLine(a.Directory);
      new Scanner(new DotNetDirectory()).Start(config);
    }
  }
}