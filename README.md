# Snarfz

Snarfz is a simple micro framework for enumerating directories and files.   

It provides support for scanning a directory tree, event driven notifications 
of directories and files, pruning support and several modes of error handling.

### Usage

```csharp
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
```

snarfz/src/Snarfz.Samples/ contains a solution with several sample projects

* SimpleScan is a console application which will scan your C:\ drive and ehco the path of all files and diretories to the console ignoring all errors.
* EmptyDirectoryScanner is a console applicaiton which will scan a given directory (provided on the command line) and list all the directories that are empty (no files and no sub directories).  It will halt on any error.
* DirScan is a console application which allows most options of Snarfz to be configured and toyed with

### Nuget

Coming soon

### Building

1. clone the snarfz repository
2. open a powershell command prompt
3. navigate to the root of the snarfz clone 
4. run scipts\bootstrap.ps1 
5. run nant Clean Cycle Deploy
6. a raw deployment can be found in deploy\raw
7. an ilmerged deployment (single assembly) can be found in deploy\merged

After step 4 you can also build via the solution.

### License

Snarfz is licensed under the MIT License
