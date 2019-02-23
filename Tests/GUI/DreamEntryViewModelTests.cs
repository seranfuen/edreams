using System;
using eDream.GUI;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class DreamEntryViewModelTests
    {
        [TestCase("  ", ExpectedResult = 0)]
        [TestCase(" abc ", ExpectedResult = 1)]
        [TestCase("  abc\r\n", ExpectedResult = 1)]
        [TestCase("  abc\r\n!", ExpectedResult = 2)]
        public int WordCount_reflects_correct_count_cases(string text)
        {
            var entityUnderTest = DreamEntryViewModel.ForNewDream();
            entityUnderTest.DreamText = text;
            return entityUnderTest.WordCount;
        }

        [TestCase("", "", ExpectedResult = false)]
        [TestCase("abc", "", ExpectedResult = true)]
        [TestCase("", "abc", ExpectedResult = true)]
        [TestCase(null, null, ExpectedResult = false)]
        public bool HasNoTextOrTags_for_cases(string text, string tags)
        {
            var entityUnderTest = DreamEntryViewModel.ForNewDream();
            entityUnderTest.DreamText = text;
            entityUnderTest.Tags = tags;
            return entityUnderTest.HasTextOrTags;
        }

        [Test]
        public void DreamText_when_changed_fires_PropertyChanged_for_DreamText_and_WordCount_and_WordCountDisplay()
        {
            var dreamTextFired = false;
            var wordCountFired = false;
            var wordCountDisplayFired = false;
            var entityUnderTest = DreamEntryViewModel.ForNewDream();
            entityUnderTest.PropertyChanged += (s, e) =>
            {
                dreamTextFired |= e.PropertyName == nameof(entityUnderTest.DreamText);
                wordCountFired |= e.PropertyName == nameof(entityUnderTest.WordCount);
                wordCountDisplayFired |= e.PropertyName == nameof(entityUnderTest.WordCountDisplay);
            };

            entityUnderTest.DreamText = "ABC";

            dreamTextFired.Should().BeTrue();
            wordCountFired.Should().BeTrue();
            wordCountDisplayFired.Should().BeTrue();
        }

        [Test]
        public void ForNewDream_date_is_initialized_to_today()
        {
            var entityUnderTest = DreamEntryViewModel.ForNewDream();
            entityUnderTest.DreamDate.Should().Be(DateTime.Now.Date);
        }

        [Test]
        public void FromExistingEntry_copies_text_date_tags()
        {
            var entry = new DreamEntry(new DateTime(2019, 2, 20), "Beach", "Hello");
            var entityUnderTest = DreamEntryViewModel.FromExistingEntry(entry);
            entityUnderTest.DreamText.Should().Be("Hello");
            entityUnderTest.Tags.Should().Be("Beach");
            entityUnderTest.DreamDate.Should().Be(new DateTime(2019, 2, 20));
        }

        [Test]
        public void ToDreamEntry_copies_data()
        {
            var entityUnderTest = DreamEntryViewModel.ForNewDream();
            entityUnderTest.DreamDate = new DateTime(2019, 2, 20);
            entityUnderTest.Tags = "Friends, Beach (Gandía)";
            entityUnderTest.DreamText = "Hello there. General Kenobi";

            var entry = entityUnderTest.ToDreamEntry();
            entry.Date.Should().Be(new DateTime(2019, 2, 20));
            entry.Text.Should().Be("Hello there. General Kenobi");
            entry.GetTagString().Should().Be("Friends, Beach (Gandía)");
        }
    }
}