// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public class Config {
    public Config(string root) {
      Root = root;
      Handlers = new EventHandlers(this);
    }

    public string Root{get;private set;}
    public ScanType ScanType{get;set;}
    public EventHandlers Handlers{get;private set;}
    public ScanErrorMode ScanErrorMode{get;set;}

    public event EventHandler<DirectoryVisitEventArgs> OnDirectory {
      add {Handlers.OnDirectory += value;}
      remove {Handlers.OnDirectory -= value;}
    }

    public event EventHandler<FileVisitEventArgs> OnFile {
      add {Handlers.OnFile += value;}
      remove {Handlers.OnFile -= value;}
    }

    public event EventHandler<ScanErrorEventArgs> OnError {
      add {Handlers.OnError += value;}
      remove {Handlers.OnError -= value;}
    }
  }
}