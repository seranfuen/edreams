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
    public class DreamTagStatisticsTests
    {
        [Test]
        public void Constructor_does_not_accept_nulls()
        {
            Action act = () => new DreamTagStatistics(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TotalDays_with_empty_collection_is_0()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>());
            entityUnderTest.TotalDays.Should().Be(0);
        }

        [Test]
        public void TotalDays_one_entry_is_1()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>
            {
                GetDreamOn(new DateTime(2019, 3, 1))
            });
            entityUnderTest.TotalDays.Should().Be(1);
        }

        [Test]
        public void TotalDays_2_entries_same_day_is_1()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>
            {
                GetDreamOn(new DateTime(2019, 3, 1)),
                GetDreamOn(new DateTime(2019, 3, 1))
            });
            entityUnderTest.TotalDays.Should().Be(1);
        }

        [Test]
        public void TotalDays_3_entries_2_days_is_2()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>
            {
                GetDreamOn(new DateTime(2019, 3, 1)),
                GetDreamOn(new DateTime(2019, 3, 2)),
                GetDreamOn(new DateTime(2019, 3, 1))
            });
            entityUnderTest.TotalDays.Should().Be(2);
        }

        [Test]
        public void TotalEntries_empty_list_is_0()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>());
            entityUnderTest.TotalEntries.Should().Be(0);
        }

        [Test]
        public void TotalEntries_three_entries_is_3()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>
            {
                GetDreamOn(new DateTime(2019, 3, 1)),
                GetDreamOn(new DateTime(2019, 3, 2)),
                GetDreamOn(new DateTime(2019, 3, 1))
            });
            entityUnderTest.TotalEntries.Should().Be(3);
        }


        [Test]
        public void Statistics_calculated_properly()
        {
            var entityUnderTest = new DreamTagStatistics(new List<DreamEntry>
            {
                GetDreamOn(new DateTime(2019, 3, 1), "Friend (John), Car"),
                GetDreamOn(new DateTime(2019, 3, 2), "Friend (Pepe, John), Beach"),
                GetDreamOn(new DateTime(2019, 3, 1), "Car, Friend (Alfredo)")
            });
            var friendStat = entityUnderTest.TagStatistics.Single(x => x.Tag == "Friend");
            friendStat.TagCount.Should().Be(3);
            friendStat.ChildStatTags.Should().ContainSingle(x => x.Tag == "John" && x.TagCount == 2);
            friendStat.ChildStatTags.Should().ContainSingle(x => x.Tag == "Pepe" && x.TagCount == 1);
            friendStat.ChildStatTags.Should().ContainSingle(x => x.Tag == "Alfredo" && x.TagCount == 1);

            entityUnderTest.TagStatistics.Single(x => x.Tag == "Car").TagCount.Should().Be(2);
            entityUnderTest.TagStatistics.Single(x => x.Tag == "Beach").TagCount.Should().Be(1);
        }

        private static DreamEntry GetDreamOn(DateTime day, string tag = "Hello")
        {
            return new DreamEntry(day, tag, "There General Kenobi");
        }
    }
}