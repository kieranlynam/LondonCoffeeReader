namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IShareSource
    {
        bool IsEnabled { get; set; }

        void Share(SharePackage package);
    }

    public class SharePackage
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }
    }
}
