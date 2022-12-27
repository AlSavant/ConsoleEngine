using DataModel.ComponentModel;
using DataModel.Math.Structures;
using System;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class SpriteGridStateService : ISpriteGridStateService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private Vector2Int gridSize = new Vector2Int(20, 20);
        private bool showGrid = true;
        private bool supportTransparency;
        private bool isDirty = false;

        public bool IsDirty()
        {
            return isDirty;
        }

        public void SetDirtyStatus(bool status)
        {
            if (isDirty == status)
                return;
            isDirty = status;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsDirty"));
        }

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
            if(showGrid != isVisible)
            {
                showGrid = isVisible;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridVisibility"));
        }

        public void SetGridWidth(int width)
        {
            if (width == gridSize.x)
                return;
            gridSize = new Vector2Int(width, gridSize.y);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridSize"));
        }

        public void SetGridHeight(int height)
        {
            if (height == gridSize.y)
                return;
            gridSize = new Vector2Int(gridSize.x, height);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridSize"));
        }

        public void SetGridSize(Vector2Int size)
        {
            if (gridSize == size)
                return;
            gridSize = size;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridSize"));
        }

        public void SetTransparencyMode(bool enabled)
        {
            supportTransparency = enabled;
        }

        public bool SupportsTransparency()
        {
            return supportTransparency;
        }
    }
}
