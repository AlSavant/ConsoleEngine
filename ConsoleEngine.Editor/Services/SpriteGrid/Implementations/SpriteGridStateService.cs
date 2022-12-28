using DataModel.ComponentModel;
using DataModel.Math.Structures;
using System;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class SpriteGridStateService : ISpriteGridStateService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private Vector2Int gridSize = new Vector2Int(20, 20);
        private bool showGrid = false;
        private bool supportTransparency;
        private bool isDirty = false;

        private int hoveredPixelIndex = -1;

        public void SetHoveredPixel(int index)
        {
            if(hoveredPixelIndex != index)
            {
                hoveredPixelIndex = index;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HoveredPixel"));
            }
            
        }

        public Vector2Int GetHoveredPixel()
        {
            var length = gridSize.x * gridSize.y;
            if (hoveredPixelIndex < 0 || hoveredPixelIndex > length)
                return -Vector2Int.one;
            return new Vector2Int((hoveredPixelIndex % gridSize.x) + 1, (hoveredPixelIndex / gridSize.x) + 1);
        }

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
