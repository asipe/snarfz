// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections;
using System.IO;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using SupaCharge.Testing;

namespace Snarfz.UnitTests {
  [TestFixture]
  public abstract class SnarfzBaseTestCase : BaseTestCase {
    static SnarfzBaseTestCase() {
      _ObjectComparer.MaxDifferences = 10;
    }

    protected void AssertEqual(object actual, object expected) {
      Assert.That(AreEqual(actual, expected), Is.True, _ObjectComparer.DifferencesString);
    }

    protected static bool AreEqual(object actual, object expected) {
      if (actual is IEnumerable && expected is IEnumerable) {
        actual = ((IEnumerable)actual).Cast<object>().ToArray();
        expected = ((IEnumerable)expected).Cast<object>().ToArray();
      }
      return _ObjectComparer.Compare(actual, expected);
    }

    protected string[] BuildDirPaths(string root, int number) {
      return BuildPaths(root, number, "dir{0}");
    }

    protected string[] BuildFilePaths(string root, int number) {
      return BuildPaths(root, number, "file{0}.txt");
    }

    private string[] BuildPaths(string root, int number, string mask) {
      return Enumerable
        .Range(0, number)
        .Select(x => Path.Combine(root, string.Format(mask, CA<int>())))
        .ToArray();
    }

    private static readonly CompareObjects _ObjectComparer = new CompareObjects();
  }
}