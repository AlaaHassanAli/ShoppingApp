namespace ShoppingApp.Settings
{
    public static class MyFileSettings
    {
        public const string ImagesPath = "/assets/images/products";
        public const string AllowedExtensions = ".jpg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
