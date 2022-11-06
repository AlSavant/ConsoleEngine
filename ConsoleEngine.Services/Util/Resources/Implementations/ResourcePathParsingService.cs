using System;
using System.IO;

namespace ConsoleEngine.Services.Util.Resources.Implementations
{
    internal sealed class ResourcePathParsingService : IResourcePathParsingService
    {
        private readonly string root;
        private readonly bool pathExists;

        public ResourcePathParsingService()
        {
            root = AppDomain.CurrentDomain.BaseDirectory;
            root += "/Resources/";
            if (!Directory.Exists(root))
            {
                pathExists = false;
                return;
            }
            pathExists = true;
        }

        public bool PathExists
        {
            get
            {
                return pathExists;
            }
        }

        public string GetFullAssetPath(string relativeAssetPath)
        {
            return Path.Combine(root, relativeAssetPath);
        }

        public string GetDirectoryName(string relativePath)
        {
            return Path.GetDirectoryName(root + relativePath);
        }

        public string GetFileName(string relativePath)
        {
            return Path.GetFileName(root + relativePath);
        }

        public string GetFormattedPath(string path)
        {
            var formattedPath = path.Replace("\\", "/");
            var index = formattedPath.IndexOf(root) + root.Length;
            formattedPath = formattedPath.Substring(index);
            if (formattedPath.EndsWith(".xml"))
            {
                formattedPath = formattedPath.Substring(0, formattedPath.Length - 4);
            }
            return formattedPath;
        }
    }
}
