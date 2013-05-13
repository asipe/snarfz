﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

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

      var config = new Config(root) {ScanErrorMode = ScanErrorMode.Ask};
      config.OnError += (o, a) => {
        Console.WriteLine("!!! ERROR DURING SCAN: " + a.Exception.Message);
        Console.WriteLine("Press Enter To Continue Scan");
        Console.ReadLine();
      };
      config.OnDirectory += (o, a) => Console.WriteLine(a.Path);
      config.OnFile += (o, a) => Console.WriteLine("     {0}", a.Path);
      new Scanner(new DotNetDirectory(), new ScanErrorHandler()).Start(config);
      Snarfzer.NewScanner().Start(config);
    }
  }
}