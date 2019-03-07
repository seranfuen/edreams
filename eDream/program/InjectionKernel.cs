using eDream.GUI;
using eDream.libs;
using Ninject;

namespace eDream.program
{
    public static class InjectionKernel
    {
        private static readonly IKernel Kernel = new StandardKernel();

        static InjectionKernel()
        {
            Kernel.Bind<IFileService>().To<DreamFileService>();
            Kernel.Bind<IDreamDiaryPersistenceService>().To<DreamSaveLoadService>();
            Kernel.Bind<IDreamDiaryPaths>().To<DreamDiaryPaths>();
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}