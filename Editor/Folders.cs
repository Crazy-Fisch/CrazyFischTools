using System.IO;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public static class Folders
    {
        public static void CreateDirectories(string root, params string[] folders)
        {
            foreach (var folder in folders)
                Directory.CreateDirectory(Path.Combine(Application.dataPath, root, folder));
        }
    }
}