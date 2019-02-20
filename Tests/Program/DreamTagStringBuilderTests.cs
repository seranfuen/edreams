using System;
using System.Collections.Generic;
using eDream.libs;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamTagStringBuilderTests
    {
        [Test]
        public void Constructor_null_collection_results_in_Exception()
        {
            Action act = () => new DreamTagStringBuilder(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToString_empty_collection_returns_empty_string()
        {
            var entityUnderTest = new DreamTagStringBuilder(new List<DreamMainTag>());
            entityUnderTest.ToString().Should().BeEmpty();
        }

        [Test]
        public void ToString_one_element_returned_as_correct_string()
        {
            var entityUnderTest = new DreamTagStringBuilder(new List<DreamMainTag>
            {
                new DreamMainTag("Beach")
            });
            entityUnderTest.ToString().Should().Be("Beach");
        }

        [Test]
        public void ToString_one_element_with_children_as_correct_string()
        {
            var dreamMainTag = new DreamMainTag("Beach");
            dreamMainTag.ChildTags.Add(new DreamChildTag("Valencia"));
            dreamMainTag.ChildTags.Add(new DreamChildTag("Gandia"));
            var entityUnderTest = new DreamTagStringBuilder(new List<DreamMainTag>
            {
                dreamMainTag
            });
            entityUnderTest.ToString().Should().Be("Beach (Valencia, Gandia)");
        }

        [Test]
        public void ToString_three_elements_with_children_as_correct_string()
        {
            var dreamMainTag1 = new DreamMainTag("Beach");
            dreamMainTag1.ChildTags.Add(new DreamChildTag("Valencia"));
            dreamMainTag1.ChildTags.Add(new DreamChildTag("Gandia"));

            var dreamMainTag2 = new DreamMainTag("Friends");
            dreamMainTag2.ChildTags.Add(new DreamChildTag("John"));

            var entityUnderTest = new DreamTagStringBuilder(new List<DreamMainTag>
            {
                dreamMainTag1,
                new DreamMainTag("Mountain"),
                dreamMainTag2
            });
            entityUnderTest.ToString().Should().Be("Beach (Valencia, Gandia), Mountain, Friends (John)");
        }

        [Test]
        public void ToString_two_element_returned_as_correct_string()
        {
            var entityUnderTest = new DreamTagStringBuilder(new List<DreamMainTag>
            {
                new DreamMainTag("Beach"),
                new DreamMainTag("Friends")
            });
            entityUnderTest.ToString().Should().Be("Beach, Friends");
        }
    }
}