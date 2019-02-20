using System;
using System.Collections.Generic;
using System.Linq;
using eDream.libs;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class MainTagParserTests
    {
        [Test]
        public void Constructor_null_collection_throws_exception()
        {
            Action act = () => new MainTagParser(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void DreamTags_with_empty_string_returns_empty_collection()
        {
            var entityUnderTest = new MainTagParser(new List<string>());
            entityUnderTest.DreamTags.Should().BeEmpty();
        }

        [Test]
        public void DreamTags_with_two_main_tags_returns_both()
        {
            var entityUnderTest = new MainTagParser(new List<string>
            {
                "Friends", "Mountains"
            });
            entityUnderTest.DreamTags.Should().HaveCount(2).And.ContainSingle(x => x.Tag == "Friends").And
                .ContainSingle(x => x.Tag == "Mountains");
        }

        [Test]
        public void DreamTags_with_two_main_tags_and_children_returns_full_children()
        {
            var entityUnderTest = new MainTagParser(new List<string>
            {
                "Friends (John)", "Mountains (Everest, Teide)"
            });
            entityUnderTest.DreamTags.Should().HaveCount(2).And.ContainSingle(x => x.Tag == "Friends" && x.ChildTags.Single().Tag == "John").And
                .ContainSingle(x => x.Tag == "Mountains" && x.ChildTags.Count == 2);
        }
    }
}