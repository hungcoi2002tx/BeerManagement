namespace Client.Ultils
{
    public static class Extensitons
    {
        public static (string key, string value) ValidateImageUpload(this IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return ("UploadImage", "Please upload an image");
            }
            else if (imageFile.Length > 4 * 1024 * 1024) // 4MB
            {
                return ("UploadImage", "The file size cannot exceed 4MB.");
            }
            else if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(imageFile.FileName).ToLowerInvariant()))
            {
                return ("UploadImage", "The file type must be one of the following: .jpg, .jpeg, .png, .gif.");
            }
            return (null, null);
        }
    }
}
