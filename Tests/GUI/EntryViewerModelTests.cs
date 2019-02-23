using System;
using eDream.GUI;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class EntryViewerModelTests
    {
        private static DreamEntry InitializeTestEntry()
        {
            return new DreamEntry(new DateTime(2018, 2, 23), "tag1, tag2 (tag2a)", "my dream text");
        }

        // We use SetCulture to be able to control the formatting of the date
        // With this we can at least know that it will be shown properly to the user
        // with their particular culture. This is a dependency we have but I don't think it makes sense
        // to break it by adding a layer of indirection to ask a "service" we can mock to format the date, when
        // the standard .NET framework library offers the ToShortString method which we'll use - and it's unlikely to change
        [Test]
        [SetCulture("es-ES")]
        [SetUICulture("es-ES")]
        public void FromEntry_sets_date()
        {
            var entityUnderTest = EntryViewerModel.FromEntry(InitializeTestEntry(), 100);
            entityUnderTest.DreamDate.Should().Be("23/02/2018");
        }

        [Test]
        public void FromEntry_sets_entry_number()
        {
            var entityUnderTest = EntryViewerModel.FromEntry(InitializeTestEntry(), 100);
            entityUnderTest.EntryNumber.Should().Be("Entry #100");
        }

        [Test]
        public void FromEntry_sets_tags()
        {
            var entityUnderTest = EntryViewerModel.FromEntry(InitializeTestEntry(), 100);
            // Notice the principle of least surprise: this is a side effect of another class (the tag parser) so we need to explain why we get
            // Tag1 instead of tag1. NSubstitute allows us to use the 'because' argument
            entityUnderTest.DreamTags.Should()
                .Be("Tag1, Tag2 (Tag2a)", "The tag parser automatically capitalizes the tags");
        }

        [Test]
        public void FromEntry_sets_text()
        {
            var entityUnderTest = EntryViewerModel.FromEntry(InitializeTestEntry(), 100);
            entityUnderTest.DreamText.Should().Be("my dream text");
        }
    }
}