namespace PMFWikipedia.Common.StorageService
{
    public interface IStorageService
    {
        public string CreatePhotoPath();
        public string GetDefaultPath();
        public string SetDefaultPhoto(long id);
    }
}