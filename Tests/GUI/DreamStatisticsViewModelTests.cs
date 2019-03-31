using System.Collections.Generic;
using eDream.GUI;
using eDream.libs;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    [SetCulture("en-US")]
    [SetUICulture("en-US")]
    public class DreamStatisticsViewModelTests
    {
        private static IEnumerable<TagStatistic> GetTestTagStatistics()
        {
            return new List<TagStatistic>
            {
                TagStatistic.ForMainTag("Friend", 300, 0.2m, 0.2m),
                TagStatistic.ForMainTag("Beach", 100, 0.3m, 0.2m),
                TagStatistic.ForMainTag("Mountain", 5, 0.2m, 0.2m),
                TagStatistic.ForChildTag("Kenobi", "Friend", 35, 0.2m, 0.2m),
                TagStatistic.ForChildTag("Pepe", "Friend", 11, 0.2m, 0.2m),
                TagStatistic.ForChildTag("Everest", "Mountain", 1, 0.01m, 0.2m)
            };
        }

        private static DreamStatisticsViewModel InitializeEntityUnderTest(int dreamCount = 0, int daysWithDreams = 0)
        {
            return new DreamStatisticsViewModel(GetTestTagStatistics(),
                new DreamCountSummary(dreamCount, daysWithDreams));
        }

        [Test]
        public void DreamCount_with_0_dreams_show_no_dreams_in_diary()
        {
            var entityUnderTest = InitializeEntityUnderTest(0, 0);
            entityUnderTest.DreamCountSummary.Should().Be("No dreams in your diary");
        }


        [Test]
        public void DreamCount_with_1_dream_1_day_shows_singular()
        {
            var entityUnderTest = InitializeEntityUnderTest(1, 1);
            entityUnderTest.DreamCountSummary.Should().Be("1 dream in 1 day (1.0 dreams/day)");
        }

        [Test]
        public void DreamCount_with_20_dreams_in_0_days_shows_no_dreams_due_to_error()
        {
            var entityUnderTest = InitializeEntityUnderTest(20, 0);
            entityUnderTest.DreamCountSummary.Should().Be("No dreams in your diary");
        }

        [Test]
        public void DreamCount_with_20_dreams_in_3_days_shows_dreams_and_average()
        {
            var entityUnderTest = InitializeEntityUnderTest(20, 3);
            entityUnderTest.DreamCountSummary.Should().Be("20 dreams in 3 days (6.7 dreams/day)");
        }

        [Test]
        public void DreamCount_with_3_dreams_1_day_shows_singular_for_day()
        {
            var entityUnderTest = InitializeEntityUnderTest(3, 1);
            entityUnderTest.DreamCountSummary.Should().Be("3 dreams in 1 day (3.0 dreams/day)");
        }

        [Test]
        public void ShowOnlyParentTags_changed_fires_TagStatistics_PropertyChanged()
        {
            var fired = false;
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.PropertyChanged +=
                (s, e) => fired |= e.PropertyName == nameof(entityUnderTest.TagStatistics);
            entityUnderTest.ShowOnlyParentTags = true;
            fired.Should().BeTrue();
        }

        [Test]
        public void ShowOnlyParentTags_is_true_does_not_find_children()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.ShowOnlyParentTags = true;
            entityUnderTest.TagSearch = "Kenobi";
            entityUnderTest.TagStatistics.Should().BeEmpty();
        }

        [Test]
        public void ShowOnlyParentTags_is_true_filters_out_children()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.ShowOnlyParentTags = true;
            entityUnderTest.TagStatistics.Should().HaveCount(3);
            entityUnderTest.TagStatistics[0].Tag.Should().Be("Friend");
            entityUnderTest.TagStatistics[1].Tag.Should().Be("Beach");
            entityUnderTest.TagStatistics[2].Tag.Should().Be("Mountain");
        }

        [Test]
        public void TagSearch_changed_fires_TagStatistics_PropertyChanged()
        {
            var fired = false;
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.PropertyChanged +=
                (s, e) => fired |= e.PropertyName == nameof(entityUnderTest.TagStatistics);
            entityUnderTest.TagSearch = "kenobi";
            fired.Should().BeTrue();
        }

        [Test]
        public void TagSearch_empty_returns_all_statistics()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.TagSearch.Should().BeNullOrWhiteSpace();
            entityUnderTest.TagStatistics.Should().HaveCount(6);
        }

        [Test]
        public void TagSearch_for_child_tag_shows_child_and_parent()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.TagSearch = "kenobi";
            entityUnderTest.TagStatistics.Should().HaveCount(2);
            entityUnderTest.TagStatistics[0].Tag.Should().Be("Friend");
            entityUnderTest.TagStatistics[1].Tag.Should().Be("Kenobi");
        }

        [Test]
        public void TagSearch_for_parent_shows_parent_and_children()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.TagSearch = "friend";
            entityUnderTest.TagStatistics.Should().HaveCount(3);
            entityUnderTest.TagStatistics[0].Tag.Should().Be("Friend");
            entityUnderTest.TagStatistics[1].Tag.Should().Be("Kenobi");
            entityUnderTest.TagStatistics[2].Tag.Should().Be("Pepe");
        }

        [Test]
        public void TagStatistics_returns_them_sorted_by_tag_count_and_with_child_tags_after_parent_Tags()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.TagStatistics[0].Tag.Should().Be("Friend");
            entityUnderTest.TagStatistics[1].Tag.Should().Be("Kenobi");
            entityUnderTest.TagStatistics[2].Tag.Should().Be("Pepe");
            entityUnderTest.TagStatistics[3].Tag.Should().Be("Beach");
            entityUnderTest.TagStatistics[4].Tag.Should().Be("Mountain");
            entityUnderTest.TagStatistics[5].Tag.Should().Be("Everest");
        }
    }
}