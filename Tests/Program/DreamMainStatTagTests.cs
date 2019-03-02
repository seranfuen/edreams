using System.Linq;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamMainStatTagTests
    {
        [Test]
        public void TagCount_initially_is_0()
        {
            var entityUnderTest = new DreamMainStatTag("Hello");
            entityUnderTest.TagCount.Should().Be(0);
        }

        [Test]
        public void IncreaseCount_causes_TagCount_to_increase()
        {
            var entityUnderTest = new DreamMainStatTag("Hello");
            entityUnderTest.TagCount.Should().Be(0);
            entityUnderTest.IncreaseCount();
            entityUnderTest.TagCount.Should().Be(1);
            entityUnderTest.IncreaseCount();
            entityUnderTest.TagCount.Should().Be(2);
        }

        [Test]
        public void IncreaseChildCount_adds_child_and_increases_it()
        {
            var entityUnderTest = new DreamMainStatTag("Hello");
            entityUnderTest.IncreaseChildCount("there");
            entityUnderTest.ChildStatTags.Single(x => x.Tag == "There").TagCount.Should().Be(1);
            entityUnderTest.IncreaseChildCount("THERE");
            entityUnderTest.ChildStatTags.Single(x => x.Tag == "There").TagCount.Should().Be(2);
        }
    }
}