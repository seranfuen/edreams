using System.Collections.Generic;
using eDream.GUI;
using eDream.libs;
using eDream.program;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class TagWizardViewModelTests
    {
        private static IDreamTag GetMainTag(string mainTag)
        {
            var mock = Substitute.For<IDreamTag>();
            mock.Tag.Returns(mainTag);
            return mock;
        }

        private static TagWizardViewModel InitializeViewModelForTest(IList<DreamMainTag> initialTags = null)
        {
            return new TagWizardViewModel(initialTags ?? new List<DreamMainTag>(), GetAllTags());
        }

        private static IEnumerable<TagStatistic> GetAllTags()
        {
            return new List<TagStatistic>
            {
                TagStatistic.ForMainTag("Beach", 200, 0.2m, 0.1m),
                TagStatistic.ForMainTag("Friend", 180, 0.2m, 0.1m),
                TagStatistic.ForMainTag("Country side", 200, 0.2m, 0.1m),
                TagStatistic.ForChildTag("Pepe", "Friend", 80, 0.2m, 0.2m),
                TagStatistic.ForChildTag("Kenobi", "Friend", 99, 0.2m, 0.2m),
                TagStatistic.ForChildTag("Griveous", "Friend", 10, 0.2m, 0.2m)
            };
        }

        private static IDreamTag GetChildTag(string childTag, string mainTag)
        {
            var tag = GetMainTag(childTag);
            tag.ParentTag.Returns(mainTag);
            return tag;
        }

        [Test]
        public void Insert_child_tag_inserted_with_main_tag()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.AddTag(GetChildTag("Pepe", "Friend"));
            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe)");
        }


        [Test]
        public void Insert_child_tag_twice_not_inserted()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.AddTag(GetChildTag("Pepe", "Friend"));
            entityUnderTest.AddTag(GetChildTag("Kenobi", "Friend"));
            entityUnderTest.AddTag(GetChildTag("Pepe", "Friend"));
            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe, Kenobi)");
        }

        [Test]
        public void Insert_fires_PropertyChanged_event()
        {
            var fired = false;
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.PropertyChanged += (s, e) => fired = e.PropertyName == nameof(entityUnderTest.TagsToAdd);

            entityUnderTest.AddTag(GetChildTag("Pepe", "Friend"));

            fired.Should().BeTrue();
        }

        [Test]
        public void Insert_main_tag_adds_it()
        {
            var entityUnderTest = InitializeViewModelForTest();
            var tag = GetMainTag("Friend");
            entityUnderTest.TagsToAdd.Should().BeEmpty();
            entityUnderTest.AddTag(tag);
            entityUnderTest.TagsToAdd.Should().Be("Friend");
        }

        [Test]
        public void Insert_three_main_tags_one_repeated_not_inserted()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.TagsToAdd.Should().BeEmpty();
            entityUnderTest.AddTag(GetMainTag("Friend"));
            entityUnderTest.AddTag(GetMainTag("Beach"));
            entityUnderTest.AddTag(GetMainTag("Friend"));
            entityUnderTest.TagsToAdd.Should().Be("Friend, Beach");
        }

        [Test]
        public void Insert_two_main_tags_then_child_tag_to_first()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.AddTag(GetMainTag("Friend"));
            entityUnderTest.AddTag(GetMainTag("Beach"));
            entityUnderTest.TagsToAdd.Should().Be("Friend, Beach");
            entityUnderTest.AddTag(GetChildTag("Pepe", "Friend"));
            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe), Beach");
        }

        [Test]
        public void Reset_fires_PropertyChanged_for_TagsToAdd()
        {
            var entityUnderTest = InitializeViewModelForTest();

            entityUnderTest.AddTag(GetMainTag("Car"));

            var fired = false;
            entityUnderTest.PropertyChanged += (s, e) => fired |= e.PropertyName == nameof(entityUnderTest.TagsToAdd);

            entityUnderTest.Reset();
            fired.Should().BeTrue();
        }

        [Test]
        public void Reset_with_initial_tags_sets_them_and_removes_any_others()
        {
            var dreamMainTag = new DreamMainTag("Friend");
            dreamMainTag.AddChildTag(new DreamChildTag("Pepe"));
            dreamMainTag.AddChildTag(new DreamChildTag("Kenobi"));
            var entityUnderTest = InitializeViewModelForTest(new List<DreamMainTag>
            {
                dreamMainTag,
                new DreamMainTag("Beach")
            });

            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe, Kenobi), Beach");
            entityUnderTest.AddTag(GetChildTag("Plagueis", "Friend"));
            entityUnderTest.AddTag(GetMainTag("Car"));
            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe, Kenobi, Plagueis), Beach, Car");
            entityUnderTest.Reset();
            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe, Kenobi), Beach");
        }

        [Test]
        public void Reset_with_no_initial_tags_removes_all_tags()
        {
            var entityUnderTest = InitializeViewModelForTest();

            entityUnderTest.AddTag(GetChildTag("Plagueis", "Friend"));
            entityUnderTest.AddTag(GetMainTag("Car"));
            entityUnderTest.TagsToAdd.Should().Be("Friend (Plagueis), Car");
            entityUnderTest.Reset();
            entityUnderTest.TagsToAdd.Should().BeEmpty();
        }

        [Test]
        public void SearchTerm_containing_string_finds_it()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.SearchTerm = "Kenobi";
            entityUnderTest.TagsToShow.Should().HaveCount(2).And.ContainSingle(x => x.Tag == "Friend").And
                .ContainSingle(x => x.Tag == "Kenobi");
        }

        [Test]
        public void SearchTerm_first_one_term_then_empty_returns_all_again()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.SearchTerm = "Kenobi";
            entityUnderTest.TagsToShow.Should().HaveCount(2);
            entityUnderTest.SearchTerm = "";
            entityUnderTest.TagsToShow.Should().HaveCount(6);
        }

        [Test]
        public void SearchTerm_null_or_empty_shows_all_tags()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.SearchTerm.Should().BeNull();
            entityUnderTest.TagsToShow.Should().HaveCount(6);
        }

        [Test]
        public void SearchTerm_when_changed_fires_SearchTerm_PropertyChanged_and_TagsToShow_PropertyChanged()
        {
            var entityUnderTest = InitializeViewModelForTest();
            var textFired = false;
            var tagsFired = false;
            entityUnderTest.PropertyChanged +=
                (s, e) => textFired |= e.PropertyName == nameof(entityUnderTest.SearchTerm);

            entityUnderTest.PropertyChanged +=
                (s, e) => tagsFired |= e.PropertyName == nameof(entityUnderTest.TagsToShow);

            entityUnderTest.SearchTerm = "Friend";

            textFired.Should().BeTrue();
            tagsFired.Should().BeTrue();
        }

        [Test]
        public void TagsToAdd_initially_empty()
        {
            var entityUnderTest = InitializeViewModelForTest();
            entityUnderTest.TagsToAdd.Should().BeEmpty();
        }

        [Test]
        public void TagsToAdd_with_initial_tags_sets_them()
        {
            var dreamMainTag = new DreamMainTag("Friend");
            dreamMainTag.AddChildTag(new DreamChildTag("Pepe"));
            dreamMainTag.AddChildTag(new DreamChildTag("Kenobi"));
            var entityUnderTest = InitializeViewModelForTest(new List<DreamMainTag>
            {
                dreamMainTag,
                new DreamMainTag("Beach")
            });

            entityUnderTest.TagsToAdd.Should().Be("Friend (Pepe, Kenobi), Beach");
        }
    }
}