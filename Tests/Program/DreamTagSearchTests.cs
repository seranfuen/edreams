using System.Collections.Generic;
using System.Linq;
using eDream.libs;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamTagSearchTests
    {
        private IEnumerable<IDreamTag> GetDreamTags()
        {
            return new List<IDreamTag>
            {
                GetTagMock("Beach", null),
                GetTagMock("Friend", null),
                GetTagMock("Pepe", "Friend"),
                GetTagMock("Kenobi", "Friend"),
                GetTagMock("Kenobi", null),
                GetTagMock("Mountain", null),
                GetTagMock("Everest", "Mountain")
            };
        }

        private static IDreamTag GetTagMock(string tagName, object parentTagName)
        {
            var mock = Substitute.For<IDreamTag>();
            mock.Tag.Returns(tagName);
            mock.ParentTag.Returns(parentTagName);
            return mock;
        }

        [Test]
        public void Search_for_child_tag_returns_tag_and_parent()
        {
            var entityUnderTest = new DreamTagSearch(GetDreamTags());
            var result = entityUnderTest.SearchForTags("Everest").ToList();
            result.Should().HaveCount(2);
            result.All(x => x.Tag == "Mountain" || x.ParentTag == "Mountain").Should().BeTrue();
        }

        [Test]
        public void Search_for_child_tag_returns_tag_and_parent_only_returns_child_tag_and_its_parent()
        {
            var entityUnderTest = new DreamTagSearch(GetDreamTags());
            var result = entityUnderTest.SearchForTags("Pepe").ToList();
            result.Should().HaveCount(2);
            result.All(x => x.Tag == "Friend" || x.ParentTag == "Friend").Should().BeTrue();
        }

        [Test]
        public void Search_for_parent_tag_returns_tag_and_children()
        {
            var entityUnderTest = new DreamTagSearch(GetDreamTags());
            var result = entityUnderTest.SearchForTags("Friend").ToList();
            result.Should().HaveCount(3);
            result.All(x => x.Tag == "Friend" || x.ParentTag == "Friend").Should().BeTrue();
        }

        [Test]
        public void Search_for_whitespace_string_returns_all_entries()
        {
            var entityUnderTest = new DreamTagSearch(GetDreamTags());
            entityUnderTest.SearchForTags("  ").Should().HaveCount(7);
        }
    }
}