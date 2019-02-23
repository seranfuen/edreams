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
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}