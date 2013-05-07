using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class EvtArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var args = new EvtArgs("apath");
      Assert.That(args.Directory, Is.EqualTo("apath"));
    }
  }
}