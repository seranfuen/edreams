using System;
using System.IO;
using System.Windows.Forms;
using eDream.libs;

namespace eDream.GUI
{
    public class NewFileViewModel
    {
        private const string DefaultFileName = "mydreams.xml";

        public const string RequiredFileExtension = "xml";
        private readonly IFileService _fileService;

        public NewFileViewModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string FileName { get; set; } = DefaultFileName;
        public string Folder { get; set; } = Application.StartupPath;
        public string FilePath => Path.Combine(Folder ?? "", GetFileName());
        public bool IsValid => FileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        public bool FileAlreadyExists => _fileService.FileExists(FilePath);

        private string GetFileName()
        {
            if (string.IsNullOrWhiteSpace(FileName)) return DefaultFileName;
            return !Path.HasExtension(RequiredFileExtension)
                ? Path.ChangeExtension(FileName, RequiredFileExtension)
                : FileName;
        }

        public void CreateNewFile()
        {
            if (!IsValid)
                throw new InvalidOperationException("Unable to use a path with errors");

            _fileService.CreateDatabaseFile(FilePath);
        }
    }
}