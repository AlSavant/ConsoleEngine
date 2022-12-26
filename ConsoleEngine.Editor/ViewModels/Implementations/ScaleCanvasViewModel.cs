using ConsoleEngine.Editor.Model;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class ScaleCanvasViewModel : ViewModel, IScaleCanvasViewModel
    {
        private const char EMPTY = ' ';
        private const char TOP_LEFT = '↖';
        private const char TOP_RIGHT = '↗';
        private const char TOP = '↑';
        private const char LEFT = '←';
        private const char RIGHT = '→';
        private const char PIVOT = '•';
        private const char BOTTOM_LEFT = '↙';
        private const char BOTTOM_RIGHT = '↘';
        private const char BOTTOM = '↓';

        private int currentWidth;
        public int CurrentWidth
        {
            get
            {
                return currentWidth;
            }
            set
            {
                if (SetProperty(ref currentWidth, value, nameof(CurrentWidth)))
                {
                    UpdateGrid();
                }
            }
        }

        private int currentHeight;
        public int CurrentHeight
        {
            get
            {
                return currentHeight;
            }
            set
            {
                if (SetProperty(ref currentHeight, value, nameof(CurrentHeight)))
                {
                    UpdateGrid();
                }
            }
        }

        private int gridWidth;
        public int GridWidth
        {
            get
            {
                return gridWidth;
            }
            set
            {
                if (SetProperty(ref gridWidth, value, nameof(GridWidth)))
                {
                    UpdateGrid();
                }
            }
        }

        private int gridHeight;
        public int GridHeight
        {
            get
            {
                return gridHeight;
            }
            set
            {
                if (SetProperty(ref gridHeight, value, nameof(GridHeight)))
                {
                    UpdateGrid();
                }
            }
        }

        private int pivotIndex = 4;
        public int PivotIndex
        {
            get
            {
                return pivotIndex;
            }
            set
            {
                if (SetProperty(ref pivotIndex, value, nameof(PivotIndex)))
                {
                    UpdateGrid();
                }
            }
        }

        public bool ApplyChanges { get; set; }

        public SmartCollection<char> PivotGrid { get; set; }

        public void Setup(int width, int height)
        {
            CurrentWidth = width;
            CurrentHeight = height;
            GridWidth = CurrentWidth;
            GridHeight = CurrentHeight;
            PivotGrid = new SmartCollection<char>();
            ApplyChanges = false;
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (PivotGrid == null)
                return;
            char[] chars = new char[9];
            int pX = PivotIndex % 3;
            int pY = PivotIndex / 3;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    int currentIndex = y * 3 + x;
                    if (currentIndex == PivotIndex)
                    {
                        chars[currentIndex] = PIVOT;
                        continue;
                    }

                    //Top Middle
                    if (x == pX && y == pY - 1)
                    {
                        if (GridHeight >= CurrentHeight)
                        {
                            chars[currentIndex] = TOP;
                        }
                        else
                        {
                            chars[currentIndex] = BOTTOM;
                        }
                        continue;
                    }

                    //Bottom Middle
                    if (x == pX && y == pY + 1)
                    {
                        if (GridHeight >= CurrentHeight)
                        {
                            chars[currentIndex] = BOTTOM;
                        }
                        else
                        {
                            chars[currentIndex] = TOP;
                        }
                        continue;
                    }

                    //Middle Left
                    if (x == pX - 1 && y == pY)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            chars[currentIndex] = LEFT;
                        }
                        else
                        {
                            chars[currentIndex] = RIGHT;
                        }
                        continue;
                    }

                    //Middle Right
                    if (x == pX + 1 && y == pY)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            chars[currentIndex] = RIGHT;
                        }
                        else
                        {
                            chars[currentIndex] = LEFT;
                        }
                        continue;
                    }

                    //Top Left
                    if (x == pX - 1 && y == pY - 1)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = TOP_LEFT;
                            }
                            else
                            {
                                chars[currentIndex] = BOTTOM_LEFT;
                            }
                        }
                        else
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = TOP_RIGHT;
                            }
                            else
                            {
                                chars[currentIndex] = BOTTOM_RIGHT;
                            }
                        }
                        continue;
                    }

                    //Top Right
                    if (x == pX + 1 && y == pY - 1)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = TOP_RIGHT;
                            }
                            else
                            {
                                chars[currentIndex] = BOTTOM_RIGHT;
                            }
                        }
                        else
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = TOP_LEFT;
                            }
                            else
                            {
                                chars[currentIndex] = BOTTOM_LEFT;
                            }
                        }
                        continue;
                    }

                    //Bottom Left
                    if (x == pX - 1 && y == pY + 1)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = BOTTOM_LEFT;
                            }
                            else
                            {
                                chars[currentIndex] = TOP_LEFT;
                            }
                        }
                        else
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = BOTTOM_RIGHT;
                            }
                            else
                            {
                                chars[currentIndex] = TOP_RIGHT;
                            }
                        }
                        continue;
                    }

                    //Bottom Right
                    if (x == pX + 1 && y == pY + 1)
                    {
                        if (GridWidth >= CurrentWidth)
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = BOTTOM_RIGHT;
                            }
                            else
                            {
                                chars[currentIndex] = TOP_RIGHT;
                            }
                        }
                        else
                        {
                            if (GridHeight >= CurrentHeight)
                            {
                                chars[currentIndex] = BOTTOM_LEFT;
                            }
                            else
                            {
                                chars[currentIndex] = TOP_LEFT;
                            }
                        }
                        continue;
                    }

                    chars[currentIndex] = EMPTY;
                }
            }
            PivotGrid.Reset(chars);
        }

        private ICommand? applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                if (applyCommand == null)
                {
                    applyCommand = new RelayCommand<Window>(
                        window => Apply(window)
                    );
                }
                return applyCommand;
            }
        }

        private void Apply(Window? window)
        {
            ApplyChanges = true;
            window?.Close();
        }

        private ICommand? cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand<Window>(
                        window => Cancel(window)
                    );
                }
                return cancelCommand;
            }
        }

        private void Cancel(Window? window)
        {
            ApplyChanges = false;
            window?.Close();
        }
    }
}
