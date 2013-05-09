using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class ScanEventArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var args = new ScanEventArgs("apath");
      Assert.That(args.Path, Is.EqualTo("apath"));
    }
  }
}