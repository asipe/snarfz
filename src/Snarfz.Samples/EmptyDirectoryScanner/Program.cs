using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snarfz.Core;

namespace EmptyDirectoryScanner {
  internal class Program {
    private static void Main(string[] args) {
      try {
        if (args.Length == 0) {
          Console.WriteLine("USAGE: /dir:<root path to scan>");
          return;
        }
        Execute(args);
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }

    private static void Execute(IEnumerable<string> args) {
      var root = ExtractArg(args, "/dir:");
      Console.WriteLine("Scanning {0}", root);
      Console.WriteLine("----");
      var config = new Config(root) {ScanType = ScanType.DirectoriesOnly};
      config.OnDirectory += ReviewDirectory;
      Snarfzer.NewScanner().Start(config);
    }

    private static void ReviewDirectory(object sender, DirectoryVisitEventArgs args) {
      if ((Directory.GetDirectories(args.Path).Length == 0) && (Directory.GetFiles(args.Path).Length == 0))
        Console.WriteLine(args.Path);
    }

    private static string ExtractArg(IEnumerable<string> args, string argName) {
      return args
        .First(a => a.StartsWith(argName))
        .Split(new[] {':'}, 2)[1];
    }
  }
}