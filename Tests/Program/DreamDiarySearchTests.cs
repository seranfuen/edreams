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
                new DreamEntry(new DateTime(2019, 3, 8), "Beach, Cameo (Friend1)", "Hello there. General Kenobi."),
                new DreamEntry(new DateTime(2019, 3, 7), "Cameo (Friend2), Car", "You are a bold one. Kenobi.")
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
        public void SearchEntriesForAllTags_all_tags_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAllTags(new List<string> {" ", ""}).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAllTags_main_tag_in_two_returns_two()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAllTags(new List<string>
            {
                "Cameo"
            }).Should().HaveCount(2);
        }


        [Test]
        public void SearchEntriesForAllTags_no_dreams_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(new List<DreamEntry>());
            entityUnderTest.SearchEntriesForAllTags(new List<string>
            {
                "Beach"
            }).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAllTags_no_tags_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAllTags(new List<string>()).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAllTags_two_tags_searched_for_do_not_match_any()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAllTags(new List<string>
            {
                "Beach",
                "friend2"
            }).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAllTags_two_tags_searched_for_match()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAllTags(new List<string>
            {
                "Beach",
                "friend1"
            }).Should().HaveCount(1);
        }


        [Test]
        public void SearchEntriesForAnyTag_all_tags_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAnyTag(new List<string> {" ", ""}).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAnyTag_main_tag_in_two_returns_two()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAnyTag(new List<string>
            {
                "Cameo"
            }).Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesForAnyTag_no_dreams_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(new List<DreamEntry>());
            entityUnderTest.SearchEntriesForAnyTag(new List<string>
            {
                "Beach"
            }).Should().BeEmpty();
        }


        [Test]
        public void SearchEntriesForAnyTag_no_tags_empty_result()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAnyTag(new List<string>()).Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForAnyTag_two_tags_searched_for_each_matches_a_different_entry_all_returned()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAnyTag(new List<string>
            {
                "Beach",
                "friend2"
            }).Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesForAnyTag_two_tags_searched_for_one_matches_tag_another_does_not_exist_returns_one()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesForAnyTag(new List<string>
            {
                "Kenobi",
                "friend2"
            }).Should().HaveCount(1);
        }

        [Test]
        public void SearchEntriesForText_empty_text_null_result()
        {
            var entityUnderTest = new DreamDiarySearch(new List<DreamEntry>());
            entityUnderTest.SearchEntriesByText("").Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForText_existing_match_for_Letters_finds_two_entries()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("re").Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesForText_existing_word_finds_two_entries()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("kenobi").Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesForText_not_existing_subtext_finds_nothing()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("Plagueis").Should().BeEmpty();
        }

        [Test]
        public void SearchEntriesForText_several_literals_not_matching_but_unquoted_word_matches()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("\"General Darth\" bold \"darth plagueis\"").Should().HaveCount(1);
        }

        [Test]
        public void SearchEntriesForText_several_literals_two_match()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("\"General kenobi\" Darth \"bold one\"").Should().HaveCount(2);
        }

        [Test]
        public void SearchEntriesForText_two_words_between_quotes_searches_literal_string()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("\"General kenobi\"").Should().HaveCount(1);
        }

        [Test]
        public void SearchEntriesForText_two_words_no_quotes_does_or_search()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("darth GENERAL").Should().HaveCount(1);
        }

        [Test]
        public void SearchEntriesForText_two_words_no_quotes_does_or_search_no_matches()
        {
            var entityUnderTest = new DreamDiarySearch(GetEntriesOnTwoDates());
            entityUnderTest.SearchEntriesByText("darth jarjar").Should().BeEmpty();
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