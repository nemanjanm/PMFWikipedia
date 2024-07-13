namespace PMFWikipedia.Common.StorageService
{
    public class StorageService : IStorageService
    {
        public string CreatePhotoPath()
        {
            var path = @"wwwroot";
            var imgfolder = "Images";
            var imgpath = Path.Combine(path, imgfolder);
            return imgpath;
        }
    }
}