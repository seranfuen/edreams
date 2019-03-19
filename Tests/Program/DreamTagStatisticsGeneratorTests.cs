using System;
using System.Collections.Generic;
using System.Linq;
using eDream.libs;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamTagStatisticsGeneratorTests
    {
        [Test]
        public void Constructor_does_not_accept_null_collection()
        {
            Action act = () => new DreamTagStatisticsGenerator(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetStatistics_are_sorted_from_greater_count_to_less()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Mountain (Everest)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result[0].Tag.Should().Be("Friend");
            result[1].Tag.Should().Be("Kenobi");
            result[1].ParentTag.Should().Be("Friend");

            result[2].Tag.Should().Be("Pepe");
            result[2].ParentTag.Should().Be("Friend");

            result[3].Tag.Should().Be("Beach");

            result[4].Tag.Should().Be("Kenobi");
            result[4].ParentTag.Should().Be("Beach");

            result[5].Tag.Should().Be("Mountain");

            result[6].Tag.Should().Be("Everest");
            result[6].ParentTag.Should().Be("Mountain");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void GetStatistics_child_tag_returns_ratio_over_parent_tag()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe, Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });

            var result = entityUnderTest.GetStatistics().ToList();

            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Friend").PercentOfTotalEntriesDisplay.Should()
                .Be("75.0 %");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void GetStatistics_parent_tags_return_PercentageDisplay_for_tag_to_total_ratio()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe, Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });

            var result = entityUnderTest.GetStatistics().ToList();

            result.Single(tag => tag.Tag == "Friend" && tag.ParentTag == null).PercentOfTotalEntriesDisplay.Should()
                .Be("80.0 %");
        }

        [Test]
        public void GetStatistics_with_child_tags_counted_properly()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result.Single(tag => tag.Tag == "Pepe" && tag.ParentTag == "Friend").Count.Should().Be(1);
            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Friend").Count.Should().Be(2);
            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Beach").Count.Should().Be(1);
            result.Single(tag => tag.Tag == "Friend" && tag.ParentTag == null).Count.Should().Be(4);
        }

        [Test]
        public void GetStatistics_with_child_tags_ratio_calculated_over_parent_tag_appearances()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe, Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result.Single(tag => tag.Tag == "Pepe" && tag.ParentTag == "Friend").TagCountToTotalEntriesRatio.Should()
                .BeNull();
            result.Single(tag => tag.Tag == "Pepe" && tag.ParentTag == "Friend").TagCountOverParentTagCountRatio
                .Should()
                .Be(0.25m);
            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Friend").TagCountOverParentTagCountRatio
                .Should().Be(0.75m);
            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Beach").TagCountOverParentTagCountRatio
                .Should().Be(1);
            result.Single(tag => tag.Tag == "Friend" && tag.ParentTag == null).TagCountOverParentTagCountRatio.Should()
                .BeNull();
        }

        [Test]
        public void GetStatistics_with_child_tags_ratio_over_total_days_calculated()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Pepe, Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend (Kenobi)", ""),
                new DreamEntry(new DateTime(2019, 2, 23), "Friend", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result.Single(tag => tag.Tag == "Pepe" && tag.ParentTag == "Friend").DaysWithTagOfTimesTagAppearsOverTotalDaysRatio.Should()
                .BeApproximately(0.3333m, 0.01m);

            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Friend").DaysWithTagOfTimesTagAppearsOverTotalDaysRatio
                .Should().BeApproximately(0.66667m, 0.01m);
            result.Single(tag => tag.Tag == "Kenobi" && tag.ParentTag == "Beach").DaysWithTagOfTimesTagAppearsOverTotalDaysRatio
                .Should().BeApproximately(0.3333m, 0.001m);
        }

        [Test]
        public void GetStatistics_with_empty_collection_returns_empty_list()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>());
            entityUnderTest.GetStatistics().Should().BeEmpty();
        }

        [Test]
        public void GetStatistics_with_two_tags_and_two_days_fills_count_and_percent_of_total_entries()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 21), "Mountain", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result.Single(tag => tag.Tag == "Beach").Count.Should().Be(1);
            result.Single(tag => tag.Tag == "Beach").TagCountToTotalEntriesRatio.Should().Be(0.5m);
            result.Single(tag => tag.Tag == "Mountain").Count.Should().Be(2);
            result.Single(tag => tag.Tag == "Mountain").TagCountToTotalEntriesRatio.Should().Be(1);
        }


        [Test]
        public void GetStatistics_with_two_tags_and_two_days_fills_PercentOfDays()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 21), "Beach", "")
            });
            var result = entityUnderTest.GetStatistics().ToList();
            result.Single(tag => tag.Tag == "Beach").DaysWithTagOfTimesTagAppearsOverTotalDaysRatio.Should().Be(1);
            result.Single(tag => tag.Tag == "Beach").Count.Should().Be(2);
            result.Single(tag => tag.Tag == "Mountain").Count.Should().Be(2);
            result.Single(tag => tag.Tag == "Mountain").DaysWithTagOfTimesTagAppearsOverTotalDaysRatio.Should()
                .Be(0.5m);
        }

        [Test]
        public void TotalDays_with_empty_collection_is_0()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>());
            entityUnderTest.TotalDays.Should().Be(0);
        }


        [Test]
        public void TotalDays_with_four_entries_on_three_different_days_is_3()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 21), "Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 22), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Mountain", "")
            });
            entityUnderTest.TotalDays.Should().Be(3);
        }

        [Test]
        public void TotalEntries_with_empty_collection_is_0()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>());
            entityUnderTest.TotalEntries.Should().Be(0);
        }

        [Test]
        public void TotalEntries_with_four_entries_is_4()
        {
            var entityUnderTest = new DreamTagStatisticsGenerator(new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 20), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 21), "Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 20), "Beach, Mountain", ""),
                new DreamEntry(new DateTime(2019, 2, 21), "Mountain", "")
            });
            entityUnderTest.TotalEntries.Should().Be(4);
        }
    }
}