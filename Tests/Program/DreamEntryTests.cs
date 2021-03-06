﻿using System;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamEntryTests
    {
        [TestCase("a", ExpectedResult = 1)]
        [TestCase("hello there", ExpectedResult = 2)]
        [TestCase("hello there.", ExpectedResult = 2)]
        [TestCase("hello_there.", ExpectedResult = 1)]
        [TestCase("I am honored with your presence.", ExpectedResult = 6)]
        [TestCase("I\n\ram", ExpectedResult = 2)]
        [TestCase("I\n\r\n\ram", ExpectedResult = 2)]
        [TestCase("\n\r\n\r", ExpectedResult = 0)]
        [TestCase("   ", ExpectedResult = 0)]
        [TestCase(" a  ", ExpectedResult = 1)]
        [TestCase(" _  ", ExpectedResult = 1)]
        public int NumberOfWords_different_cases(string text)
        {
            return GetDreamEntryWithText(text).NumberOfWords;
        }

        private static DreamEntry GetDreamEntryWithText(string text)
        {
            return new DreamEntry(DateTime.Now, "abc", text);
        }

        [TestCase("Hello, There", "there", ExpectedResult = true)]
        [TestCase("Hello, There", "Hello", ExpectedResult = true)]
        [TestCase("Hello, There", "general", ExpectedResult = false)]
        [TestCase("Hello (general), There", "General", ExpectedResult = true)]
        public bool ContainsTagOrChildTag_cases(string tags, string searchedForTag)
        {
            var entityUnderTest = new DreamEntry(new DateTime(2019, 3, 10), tags, "h" );
            return entityUnderTest.ContainsTagOrChildTag(searchedForTag);
        }

        [Test]
        public void Constructor_does_not_allow_both_null_tags_and_text()
        {
            Action act = () => new DreamEntry(new DateTime(2019, 2, 16), null, "");
            act.Should().ThrowExactly<ArgumentNullException>();

            Action act2 = () => new DreamEntry(new DateTime(2019, 2, 16), "", "");
            act2.Should().ThrowExactly<ArgumentNullException>();

            Action act3 = () => new DreamEntry(new DateTime(2019, 2, 16), "Hello", null);
            act3.Should().NotThrow();

            Action act4 = () => new DreamEntry(new DateTime(2019, 2, 16), "", "Hello");
            act4.Should().NotThrow();
        }

        [Test]
        public void Constructor_strips_time_from_DreamDate()
        {
            var entityUnderTest = new DreamEntry(new DateTime(2019, 2, 16, 12, 56, 23), "hello", "");
            entityUnderTest.Date.Should().Be(new DateTime(2019, 2, 16));
        }

        [Test]
        public void NumberOfWords_changes_after_changing_text()
        {
            var entityUnderTest = GetDreamEntryWithText("");
            entityUnderTest.NumberOfWords.Should().Be(0);
            entityUnderTest.Text = "Hello there my friend.\r\nHow are you?";
            entityUnderTest.NumberOfWords.Should().Be(7);
        }

        [Test]
        public void NumberOfWords_empty_test_is_0()
        {
            var entityUnderTest = GetDreamEntryWithText("");
            entityUnderTest.NumberOfWords.Should().Be(0);
        }

        [Test]
        public void Text_setter_fails_when_passing_null()
        {
            var entityUnderTest = GetDreamEntryWithText("");
            Action act = () => entityUnderTest.Text = null;
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void DreamTextContains_empty_text_empty_value_searched_for_False()
        {
            var entityUnderTEst = GetDreamEntryWithText("");
            entityUnderTEst.DreamTextContains("").Should().BeFalse();
        }

        [Test]
        public void DreamTextContains_empty_value_searched_for_False()
        {
            var entityUnderTEst = GetDreamEntryWithText("Hello");
            entityUnderTEst.DreamTextContains("").Should().BeFalse();
        }

        [TestCase("Hello", "hello", ExpectedResult = true)]
        [TestCase("Hel", "hello", ExpectedResult = false)]
        [TestCase("Hello", "hell", ExpectedResult = true)]
        public bool DreamTextContains_different_cases(string text, string valueSearchedFor)
        {
            var entityUnderTest = GetDreamEntryWithText(text);
            return entityUnderTest.DreamTextContains(valueSearchedFor);
        }
    }
}