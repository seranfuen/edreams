using eDream.GUI;

namespace eDream
{
    public interface IEdreamsFactory
    {
        IDreamDiaryViewModel CreateDreamDiaryViewModel();
        FrmNewFileCreator CreateNewFileCreator();
    }
}