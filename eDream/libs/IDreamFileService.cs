namespace eDream.libs
{
    public interface IDreamFileService
    {
        bool FileExists(string path);
        void CreateDatabaseFile(string path);
    }
}