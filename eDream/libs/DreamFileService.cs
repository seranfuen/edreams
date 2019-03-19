using System.IO;

namespace eDream.libs
{
    public class DreamFileService : IDreamFileService
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void CreateDatabaseFile(string path)
        {
            var writer = new DiaryXmlWriter();
            writer.CreateFile(path);
        }
    }
}