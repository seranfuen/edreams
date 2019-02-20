using System;
using System.Linq;
using eDream.libs;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamTagStringExtractorTests
    {
        [Test]
        public void Constructor_empty_string_returns_empty_list()
        {
            var result = new DreamTagStringExtractor("").Tags;
            result.Should().BeEmpty();
        }

        [Test]
        public void Constructor_null_string_results_in_exception()
        {
            Action act = () => new DreamTagStringExtractor(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void With_one_tag_and_children_returns_one_tag_exactly()
        {
            var result = new DreamTagStringExtractor("Friends (Pepe, John)").Tags;
            result.Single().Should().Be("Friends (Pepe, John)");
        }

        [Test]
        public void With_one_tag_and_children_with_unclosed_child_tags_returns_one_tag_exactly()
        {
            var result = new DreamTagStringExtractor("Friends (Pepe, John").Tags;
            result.Single().Should().Be("Friends (Pepe, John)");
        }

        [Test]
        public void With_one_tag_returns_one_tag_exactly()
        {
            var result = new DreamTagStringExtractor("Friends ").Tags;
            result.Single().Should().Be("Friends");
        }

        [Test]
        public void With_two_tags_and_child_tags_two_elements_returned()
        {
            var result = new DreamTagStringExtractor("Friends  (Pepe, John), Beach (Gandia)").Tags;
            result.Should().HaveCount(2).And.Contain("Friends  (Pepe, John)").And.Contain("Beach (Gandia)");
        }

        [Test]
        public void With_two_tags_returns_two_elements()
        {
            var result = new DreamTagStringExtractor("Friends, Beach").Tags;
            result.Should().HaveCount(2).And.Contain("Friends").And.Contain("Beach");
        }
    }
}