using eDream.GUI;
using eDream.libs;
using eDream.program;

namespace eDream
{
    public interface IEdreamsFactory
    {
        IDreamDiaryViewModel CreateDreamDiaryViewModel();
        FrmNewFileCreator CreateNewFileCreator();
        IDreamReaderWriterFactory CreateDreamReaderWriterFactory();
    }
}