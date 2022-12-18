using ConsoleEngine.Editor.Model;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Editor.Services.Commands
{
    internal interface IPaintPixelCommand : ILogicCommand<KeyValuePair<int, Pixel>>
    {
    }
}
