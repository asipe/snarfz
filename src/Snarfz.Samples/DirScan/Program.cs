// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Snarfz.Core;

namespace DirScan {
  internal class Program {
    private static void Main(string[] args) {
      try {
        if (args.Length == 0) {
          Console.WriteLine("USAGE: /dir:<root path to scan> /scantype:<All, FilesOnly, DirectoriesOnly> /scanerrormode:<Ask, Throw, Ignore> /eventerrormode:<Throw, Ignore>");
          return;
        }
        Execute(args);
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }

    private static void Execute(string[] args) {
      var root = ExtractArg(args, "/dir:");
      var scanType = (ScanType)Enum.Parse(typeof(ScanType), ExtractArg(args, "/scantype:"), true);
      var scanErrorMode = (ScanErrorMode)Enum.Parse(typeof(ScanErrorMode), ExtractArg(args, "/scanerrormode:"), true);
      var eventErrorMode = (EventErrorMode)Enum.Parse(typeof(EventErrorMode), ExtractArg(args, "/eventerrormode:"), true);

      Console.WriteLine("Scanning {0}", root);
      Console.WriteLine("----");

      var config = new Config(root) {
                                      ScanType = scanType,
                                      ScanErrorMode = scanErrorMode,
                                      EventErrorMode = eventErrorMode
                                    };
      config.OnError += (o, a) => {
        Console.WriteLine("!!! ERROR DURING SCAN({0} - {1}):", a.Source, a.Path);
        Console.WriteLine(a.Exception.Message);
        Console.WriteLine("Press Enter To Continue Scan");
        Console.ReadLine();
      };
      config.OnDirectory += (o, a) => Console.WriteLine(a.Path);
      config.OnFile += (o, a) => Console.WriteLine("     {0}", a.Path);
      Snarfzer.NewScanner().Start(config);
    }

    private static string ExtractArg(IEnumerable<string> args, string argName) {
      return args
        .First(a => a.StartsWith(argName))
        .Split(new[] {':'}, 2)[1];
    }
  }
}