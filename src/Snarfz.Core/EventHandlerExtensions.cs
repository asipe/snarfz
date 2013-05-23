using System;

namespace Snarfz.Core {
  public static class EventHandlerExtensions {
    static public void RaiseEvent<T>(this EventHandler<T> evt, object sender, T args) where T : EventArgs {
      if (evt != null)
        evt(sender, args);
    }
  }
}