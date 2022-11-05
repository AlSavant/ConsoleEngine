using DataModel.Math.Structures;
using DataModel.Rendering;
using System;

namespace ConsoleEngine.Services.Util.Rendering
{
    public interface IGUIRenderingService : IService
    {
        void PrintText(Vector2Int position, string text, ConsoleColor color, bool blackIsTransparency = false);
        void PrintText(RectInt rect, string text, ConsoleColor color, bool blackIsTransparency = false);
        GUIEntry GetGetNextGUIEntry();
    }
}
