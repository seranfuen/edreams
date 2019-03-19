using System;
using eDream.GUI;
using eDream.libs;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class NewFileViewModelTests
    {
        [Test]
        public void CreateNewFile_delegates_to_FileService()
        {
            var mockService = Substitute.For<IDreamFileService>();
            mockService.FileExists(Arg.Any<string>()).Returns(true);
            var entityUnderTest = new NewFileViewModel(mockService)
                {Folder = @"C:\Example\MyFolder", FileName = "here"};
            entityUnderTest.CreateNewFile();
            mockService.Received(1).CreateDatabaseFile(@"C:\Example\MyFolder\here.xml");
        }

        [Test]
        public void CreateNewFile_with_not_validated_path_throws_InvalidOperationException()
        {
            var mockService = Substitute.For<IDreamFileService>();
            mockService.FileExists(Arg.Any<string>()).Returns(true);
            var entityUnderTest = new NewFileViewModel(mockService)
                { Folder = @"C:\Example\MyFolder", FileName = "??he" };
            Action act = () => entityUnderTest.CreateNewFile();
            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Unable to use a path with errors");
        }

        [Test]
        public void FileAlreadyExists_delegates_to_FileService()
        {
            var mockService = Substitute.For<IDreamFileService>();
            mockService.FileExists(Arg.Any<string>()).Returns(true);
            var entityUnderTest = new NewFileViewModel(mockService)
                {Folder = @"C:\Example\MyFolder", FileName = "here"};
            entityUnderTest.FileAlreadyExists.Should().BeTrue();
            mockService.Received(1).FileExists(@"C:\Example\MyFolder\here.xml");
        }

        [Test]
        public void FileName_by_default_is_mydreams()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>());
            entityUnderTest.FileName.Should().Be("mydreams.xml");
        }

        [Test]
        public void FilePath_if_not_ending_in_xml_appended()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>())
                {Folder = @"C:\Example\MyFolder", FileName = "here"};
            entityUnderTest.FilePath.Should().Be(@"C:\Example\MyFolder\here.xml");
        }

        [Test]
        public void FilePath_is_Folder_with_FileName()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>())
                {Folder = @"C:\Example\MyFolder", FileName = "mydream.xml"};
            entityUnderTest.FilePath.Should().Be(@"C:\Example\MyFolder\mydream.xml");
        }

        [Test]
        public void FilePath_with_empty_FileName_returns_default()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>())
                {Folder = @"C:\Example\MyFolder", FileName = ""};
            entityUnderTest.FilePath.Should().Be(@"C:\Example\MyFolder\mydreams.xml");
        }

        [Test]
        public void IsValid_illegal_characters_in_path_false()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>())
                {Folder = @"C:\Example\MyFolder", FileName = "here?"};
            entityUnderTest.IsValid.Should().BeFalse();
        }

        [Test]
        public void IsValid_normal_filepath_validated()
        {
            var entityUnderTest = new NewFileViewModel(Substitute.For<IDreamFileService>())
                {Folder = @"C:\Example\MyFolder", FileName = "here"};
            entityUnderTest.IsValid.Should().BeTrue();
        }
    }
}