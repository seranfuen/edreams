using System;
using System.Collections.Generic;
using eDream.libs;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamDiarySearchTests
    {
        private static IEnumerable<DreamEntry> GetEntriesOnTwoDates()
        {
            return new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 3, 8), "A", "B"),
                new DreamEntry(new DateTime(2019, 3, 7), "A", "B")
            };
        }

        [Test]
        public void Constructor_does_not_accept_null_collection()
        {
            Action act = () => new DreamDiarySearch(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void SearchEntriesBetweenDates_with_empty_collection_returns_empty()
        {
            var entityUnderTest = new DreamDiarySearch(new List<DreamEntry>());
            entityUnderTest.SearchEntriesBetweenDates(DateTime.Now.AddDays(-1), DateTime.Now).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesBetweenDates_with_two_dates_both_same_returns_entry_on_that_date()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesBetweenDates(new DateTime(2019, 3, 8), new DateTime(2019, 3,
                    8)).Should().HaveCount(1).And
                .ContainSingle(x => x.Date.Equals(new DateTime(2019, 3, 8)));
        }

        [Test]
        public void SearchEntriesBetweenDates_with_two_dates_first_date_later_than_last_returns_empty()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesBetweenDates(new DateTime(2019, 3, 8), new DateTime(2019, 3,
                7)).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesBetweenDates_with_two_dates_range_starts_on_first_date_ends_on_last_date_returns_two()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesBetweenDates(new DateTime(2019, 3, 7), new DateTime(2019, 3,
                8)).Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesOnDate_two_dates_not_found_returns_empty()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesOnDate(new DateTime(2019, 3, 9)).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesOnDate_two_dates_one_found()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesOnDate(new DateTime(2019, 3, 8)).Should().HaveCount(1).And
                .ContainSingle(x => x.Date.Equals(new DateTime(2019, 3, 8)));
        }

        [Test]
        public void SearchEntriesOnDate_with_empty_collection_returns_empty()
        {
            var entityUnderTest = new DreamDiarySearch(new List<DreamEntry>());
            entityUnderTest.SearchEntriesOnDate(DateTime.Now).Should().BeEmpty();
        }
    }
}