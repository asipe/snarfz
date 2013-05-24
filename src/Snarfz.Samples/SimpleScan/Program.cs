using System;
using Snarfz.Core;

namespace SimpleScan {
  internal class Program {
    private static void Main() {
      var config = new Config(@"c:\") { ScanErrorMode = ScanErrorMode.Ignore };
      config.OnDirectory += (o, a) => Console.WriteLine(a.Path);
      config.OnFile += (o, a) => Console.WriteLine(a.Path);
      Snarfzer.NewScanner().Start(config);
    }
  }
}