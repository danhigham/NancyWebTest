
namespace NancyWebTest
{
    public class NinjectModule : Ninject.Modules.NinjectModule
    {
        enum BuildConfiguration
        {
            Local,
            Amazon,
            Azure
        }

        //  SELECT WHICH STORAGE OPTION TO INJECT BY ADJUSTING THIS CONSTANT             
        const BuildConfiguration SelectedBuild = BuildConfiguration.Local;

        public override void Load()
        {
            switch (SelectedBuild)
            {
                case BuildConfiguration.Local:
                    Bind<IImageStore>().To<FileSystemStore>();
                    break;
                case BuildConfiguration.Amazon:
                    Bind<IImageStore>().To<S3Store>();
                    break;
                case BuildConfiguration.Azure:
                    Bind<IImageStore>().To<AzureStore>();
                    break;

            }
        }
    }
}
