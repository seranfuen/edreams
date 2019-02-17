using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamMainTagTests
    {
        [Test]
        public void Constructor_null_passed_becomes_empty_string()
        {
            var entityUnderTest = new DreamMainTag(null);
            entityUnderTest.Tag.Should().BeEmpty();
        }

        [Test]
        public void AddChildTag_adds_it()
        {
            var entityUnderTest = new DreamMainTag("Friend");
            entityUnderTest.AddChildTag(new DreamChildTag("John"));
            entityUnderTest.ChildTags.Should().ContainSingle(x => x.Tag == "John");
        }

        [Test]
        public void AddChildTag_same_twice_is_not_added()
        {
            var entityUnderTest = new DreamMainTag("Friend");
            entityUnderTest.AddChildTag(new DreamChildTag("John"));
            entityUnderTest.AddChildTag(new DreamChildTag("john"));
            entityUnderTest.ChildTags.Should().ContainSingle(x => x.Tag == "John");
        }
    }
}