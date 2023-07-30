using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public static class Packages
    {
        public static void InstallUnityPackage(string packageName) => UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");

        public static async Task ReplacePackagesFromGist(string id, string user = "Crazy-Fisch")
        {
            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }
        
        private static string GetGistUrl(string id, string user) =>
            $"https://gist.githubusercontent.com/{user}/{id}/raw";

        private static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();    
        }
    }
}