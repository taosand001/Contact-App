namespace Contact.Infrastructure.Helpers
{
    public static class ImageService
    {
        public static void DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static byte[] GetImage(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
    }
}
