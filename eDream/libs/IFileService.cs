namespace eDream.libs
{
    public interface IFileService
    {
        bool FileExists(string path);
        void CreateDatabaseFile(string path);
    }
}