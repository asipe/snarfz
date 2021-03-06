﻿// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Snarfz.Core {
  [Serializable]
  public class ScanException : SnarfzException {
    public ScanException() {}
    public ScanException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    public ScanException(string msg) : base(msg) {}
    public ScanException(string msg, Exception e) : base(msg, e) {}
    public ScanException(string msg, params object[] fmt) : base(string.Format(msg, fmt)) {}
  }
}