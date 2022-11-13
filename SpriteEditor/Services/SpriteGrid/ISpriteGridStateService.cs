using DataModel.Math.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEditor.Services.SpriteGrid
{
    internal interface ISpriteGridStateService : IService
    {
        Vector2Int GetGridSize();
        int GetGridWidth();
        int GetGridHeight();
        bool CanShowGrid();
        void SetGridVisibility(bool isVisible);
        void SetGridWidth(int width);
        void SetGridHeight(int height);
        void SetGridSize(Vector2Int size);
    }
}
