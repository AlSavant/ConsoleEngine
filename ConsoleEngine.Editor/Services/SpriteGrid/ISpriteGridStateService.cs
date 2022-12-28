using DataModel.ComponentModel;
using DataModel.Math.Structures;

namespace ConsoleEngine.Editor.Services.SpriteGrid
{
    internal interface ISpriteGridStateService : IService, INotifyPropertyChanged
    {        
        Vector2Int GetGridSize();
        int GetGridWidth();
        int GetGridHeight();
        bool CanShowGrid();
        void SetGridVisibility(bool isVisible);
        void SetGridWidth(int width);
        void SetGridHeight(int height);
        void SetGridSize(Vector2Int size);
        void SetTransparencyMode(bool enabled);
        bool SupportsTransparency();
        bool IsDirty();
        void SetDirtyStatus(bool status);
    }
}
