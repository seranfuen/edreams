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
            Kernel.Bind<IDreamDiaryBus>().To<DreamDiaryBus>();
            Kernel.Bind<IDreamSettings>().To<DreamSettings>();
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}