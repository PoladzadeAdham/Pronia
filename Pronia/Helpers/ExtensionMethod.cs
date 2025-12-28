using System.Threading.Tasks;

namespace Pronia.Helpers
{
    public static class ExtensionMethod
    {
        public static bool CheckSize(this IFormFile formFile, int mb)
        {
            return formFile.Length < mb * 1024 * 1024;

        }

        public static bool CheckType(this IFormFile formFile, string type = "image")
        {
            return formFile.ContentType.Contains(type);
        }

        public static async Task<string> SaveFileAsync(this IFormFile formFile, string folderPath)
        {
            string uniqueName = Guid.NewGuid().ToString() + formFile.FileName;

            string path = Path.Combine(folderPath, uniqueName);

            using FileStream fileStream = new(path, FileMode.Create);

            await formFile.CopyToAsync(fileStream);

            return uniqueName;
        }


        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

    }
}
