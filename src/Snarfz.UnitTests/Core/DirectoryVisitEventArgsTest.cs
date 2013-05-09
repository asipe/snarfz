using NUnit.Framework;
using Snarfz.Core;
using SupaCharge.Testing;

namespace Snarfz.UnitTests.Core {
  [TestFixture]
  public class DirectoryVisitEventArgsTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var args = new DirectoryVisitEventArgs("apath");
      Assert.That(args.Path, Is.EqualTo("apath"));
    }
  }
}