using DataModel.Math.Structures;

namespace SpriteEditor.Services.SpriteGrid.Implementations
{
    internal sealed class SpriteGridStateService : ISpriteGridStateService
    {
        private Vector2Int gridSize = new Vector2Int(20,20);
        private bool showGrid = true;

        public Vector2Int GetGridSize()
        {
            return gridSize;
        }
        
        public int GetGridWidth()
        {
            return gridSize.x;
        }

        public int GetGridHeight()
        {
            return gridSize.y;
        }

        public bool CanShowGrid()
        {
            return showGrid;
        }

        public void SetGridVisibility(bool isVisible)
        {
            showGrid = isVisible;
        }

        public void SetGridWidth(int width)
        {
            gridSize = new Vector2Int(width, gridSize.y);
        }

        public void SetGridHeight(int height)
        {
            gridSize = new Vector2Int(gridSize.x, height);
        }

        public void SetGridSize(Vector2Int size)
        {
            gridSize = size;
        }

    }
}
