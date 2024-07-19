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

        public string GetDefaultPath()
        {
            var path = "Images";
            var image = Path.Combine(path, "defaultAvatar.jpg");
            return image;
        }

        public string SetDefaultPhoto(long id)
        {
            var imagePath = Path.Combine(CreatePhotoPath(), "user" + id + ".jpg");
            var copyPath = Path.Combine(CreatePhotoPath(), "defaultAvatar.jpg");
            if (File.Exists(copyPath))
            {
                System.IO.File.Copy(copyPath, imagePath);
            }
            var returnPath = Path.Combine("Images", "user" + id + ".jpg");
            return returnPath;
        }
    }
}