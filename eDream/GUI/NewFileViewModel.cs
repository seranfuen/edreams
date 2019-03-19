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
        private readonly IDreamFileService _dreamFileService;

        public NewFileViewModel(IDreamFileService dreamFileService)
        {
            _dreamFileService = dreamFileService;
        }

        public string FileName { get; set; } = DefaultFileName;
        public string Folder { get; set; } = Application.StartupPath;
        public string FilePath => Path.Combine(Folder ?? "", GetFileName());
        public bool IsValid => FileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        public bool FileAlreadyExists => _dreamFileService.FileExists(FilePath);

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

            _dreamFileService.CreateDatabaseFile(FilePath);
        }
    }
}