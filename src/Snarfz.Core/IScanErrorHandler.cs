using System;

namespace Snarfz.Core {
  public interface IScanErrorHandler {
    void Handle(string currentDir, Exception exception);
  }
}