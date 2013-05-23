// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;

namespace Snarfz.Core {
  public static class EventHandlerExtensions {
    public static void RaiseEvent<T>(this EventHandler<T> evt, object sender, T args) where T : EventArgs {
      if (evt != null)
        evt(sender, args);
    }
  }
}