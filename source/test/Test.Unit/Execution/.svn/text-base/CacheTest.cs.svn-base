using MonkeyPants.Execution;
using NUnit.Framework;

namespace Test.Unit.Execution
{
	[TestFixture]
	public class CacheTest
	{
		private const string KEY = "a key";
		private const string EXPECTED_VALUE = "anything at all";
		private UserCache cache;

		[SetUp]
		public void Setup()
		{
			cache = new UserCache();
		}

		[Test]
		public void ShouldNotFindAnything()
		{
			Assert.IsNull(cache[KEY]);
		}

		[Test]
		public void ShouldFindTheObjectInserted()
		{
			cache[KEY] = EXPECTED_VALUE;
			Assert.AreEqual(EXPECTED_VALUE, cache[KEY]);
		}

		[Test]
		public void ShouldNotBeAffectedByOtherCachedValues()
		{
			cache["Ya"] = "boo";
			cache[KEY] = EXPECTED_VALUE;
			cache["Yo"] = "bee";
			Assert.AreEqual(EXPECTED_VALUE, cache[KEY]);
		}

		[Test]
		public void ShouldFindTheReplacingObject()
		{
			cache[KEY] = "Ya Boo";
			cache[KEY] = EXPECTED_VALUE;
			Assert.AreEqual(EXPECTED_VALUE, cache[KEY]);
		}
	}
}