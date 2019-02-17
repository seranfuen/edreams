using System;
using System.Linq;
using eDream.libs;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamTagParserTests
    {
        [Test]
        public void ParseStringToDreamTags_empty_string_returns_empty_tags()
        {
            DreamTagParser.ParseStringToDreamTags("").Should().BeEmpty();
        }

        [Test]
        public void ParseStringToDreamTags_null_string_results_in_ArgumentNullException()
        {
            Action act = () => DreamTagParser.ParseStringToDreamTags(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ParseStringToDreamTags_single_tag_returned()
        {
            var result = DreamTagParser.ParseStringToDreamTags("Movies").Single();
            result.Tag.Should().Be("Movies");
            result.ChildTags.Should().BeEmpty();
        }

        [Test]
        public void ParseStringToDreamTags_single_tag_with_children_returned_correctly()
        {
            var result = DreamTagParser.ParseStringToDreamTags("Movies (Die Hard, Heat)").Single();
            result.Tag.Should().Be("Movies");
            result.ChildTags.Should().ContainSingle(x => x.Tag == "Die Hard").And.ContainSingle(x => x.Tag == "Heat");
        }

        [Test]
        public void ParseStringToDreamTags_two_tags_with_children_returned_correctly()
        {
            var result = DreamTagParser.ParseStringToDreamTags("Movies (Die Hard, Heat), Beach");
            result.Should().ContainSingle(x => x.Tag == "Movies" && x.ChildTags.Count == 2);
            result.Should().ContainSingle(x => x.Tag == "Beach" && x.ChildTags.Count == 0);
        }

        [Test]
        public void ParseStringToDreamTags_single_tag_with_non_closed_parenthesis_omits_last_child()
        {
            var result = DreamTagParser.ParseStringToDreamTags("Movies (Die Hard, Heat").Single();
            result.Tag.Should().Be("Movies");
            result.ChildTags.Should()
                .HaveCount(2,
                    "If we cannot find the closing parenthesis just take everything").And
                .ContainSingle(x => x.Tag == "Die Hard").And.ContainSingle(x => x.Tag == "Heat");
        }

        [Test]
        public void ParseStringToDreamTags_one_tag_is_empty_only_parenthesis_is_ignored()
        {
            var result = DreamTagParser.ParseStringToDreamTags("(Die Hard), Beach");
            result.Single().Tag.Should().Be("Beach");
        }
    }
}