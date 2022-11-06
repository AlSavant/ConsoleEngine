using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEngine.Services.Util.Resources
{
    public interface IResourcePathParsingService : IService
    {
        bool PathExists { get; }
        string GetFullAssetPath(string relativeAssetPath);
        string GetDirectoryName(string relativePath);
        string GetFormattedPath(string path);
        string GetFileName(string relativePath);
    }
}
