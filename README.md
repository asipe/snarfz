# Snarfz

Snarfz is a simple micro framework for enumerating directories and files.   

It provides support for scanning a directory tree, event driven notifications 
of directories and files, pruning support and several modes of error handling.

Snarfz Supports
* receiving events for files only, directories only or both files and directories
* pruning support for sub directories and/or files
* 3 modes of error handling support during a scan (ignore, ask, halt)
* 2 modes of error handling support during event handling (ignore, halt)

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

nuget package page:  https://nuget.org/packages/Snarfz/

install via package manager:  Install-Package Snarfz

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

The MIT License (MIT)

    Copyright (c) 2013 Andy Sipe (ajs.general@gmail.com)
    
    Permission is hereby granted, free of charge, to any person obtaining a copy of 
    this software and associated documentation files (the "Software"), to deal in the 
    Software without restriction, including without limitation the rights to use, copy, 
    modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
    and to permit persons to whom the Software is furnished to do so, subject to 
    the following conditions:
    
    The above copyright notice and this permission notice shall be included 
    in all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
    OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

