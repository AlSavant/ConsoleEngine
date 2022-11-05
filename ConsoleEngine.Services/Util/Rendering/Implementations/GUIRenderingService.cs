using DataModel.Math.Structures;
using DataModel.Rendering;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Util.Rendering.Implementations
{
    internal class GUIRenderingService : IGUIRenderingService
    {
        private readonly Stack<GUIEntry> guiEntries;
        private readonly Stack<GUIEntry> guiEntryPool;

        public GUIRenderingService() 
        {
            guiEntries = new Stack<GUIEntry>();
            guiEntryPool = new Stack<GUIEntry>();
        }

        public void PrintText(Vector2Int position, string text, ConsoleColor color, bool blackIsTransparency = false)
        {
            var entry = CreateEntry();
            entry.rect = new RectInt(position, new Vector2Int(text.Length, 1));
            entry.text = text;
            entry.color = color;
            entry.blackIsTransparency = blackIsTransparency;
            guiEntries.Push(entry);
        }

        public void PrintText(RectInt rect, string text, ConsoleColor color, bool blackIsTransparency = false)
        {
            var entry = CreateEntry();
            entry.rect = rect;
            entry.text = text;
            entry.color = color;
            entry.blackIsTransparency = blackIsTransparency;
            guiEntries.Push(entry);
        }

        private GUIEntry CreateEntry()
        {            
            if(guiEntryPool.Count > 0)
            {
                return guiEntryPool.Pop();                
            }
            return new GUIEntry();
        }

        public GUIEntry GetGetNextGUIEntry()
        {
            if (guiEntries.Count <= 0)
                return null;
            var entry = guiEntries.Pop();
            guiEntryPool.Push(entry);
            return entry;
        }
    }
}
