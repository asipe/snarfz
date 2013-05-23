using System;

namespace Snarfz.Core {
  public interface IEventErrorHandler {
    void Handle(Config config, Exception exception);
  }
}