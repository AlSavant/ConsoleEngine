using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Services.Commands;
using ConsoleEngine.Editor.Services.Encoding;
using ConsoleEngine.Editor.Services.Factories;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.SpriteGrid;
using ConsoleEngine.Editor.Services.Util;
using ConsoleEngine.Editor.Views;
using DataModel.ComponentModel;
using DataModel.Math.Structures;
using DataModel.Rendering;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ConsoleEngine.Editor.Services.Commands.SpriteCanvas;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.Commands.Serialization;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class SpriteEditorViewModel : ViewModel, ISpriteEditorViewModel
    {                        
        public ObservableCollection<string> RecentFiles {  get { return spriteSavePathService.RecentFiles; } }        

        public bool CanBrowseRecents
        {
            get
            {
                return RecentFiles.Count > 0;
            }
        }        

        private readonly IViewModelFactory viewModelFactory;
        private readonly IMatrixOperationsService matrixOperationsService;
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ICharToOEM437ConverterService charToOEM437ConverterService;
        private readonly IHistoryActionService historyActionService;
        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly ISpriteSavePathService spriteSavePathService;

        public ILogicCommand UndoCommand { get; private set; }
        public ILogicCommand RedoCommand { get; private set; }
        public ILogicCommand SaveSpriteCommand { get; private set; }
        public ILogicCommand SaveSpriteWithLocationCommand { get; private set; }

        public ICommand RotateGrid90CWCommand { get; private set; }
        public ICommand RotateGrid90CCWCommand { get; private set; }
        public ICommand RotateGrid180Command { get; private set; }
        public ICommand FlipGridVerticallyCommand { get; private set; } 
        public ICommand FlipGridHorizontallyCommand { get; private set; }
        public ICommand QuitApplicationCommand { get; private set; }
        public ICommand OpenWithLocationCommand { get; private set; }
        public ICommand OpenSpriteCommand { get; private set; }

        public SpriteEditorViewModel(
            IOpenSpriteCommand openSpriteCommand,
            IOpenSpriteWithLocationCommand openSpriteWithLocationCommand,
            IQuitApplicationCommand quitApplicationCommand,
            ISaveSpriteWithLocationCommand saveSpriteWithLocationCommand,
            ISaveSpriteCommand saveSpriteCommand,
            ISpriteSavePathService spriteSavePathService,
            ICanvasDrawingService canvasDrawingService,
            IFlipGridHorizontallyCommand flipGridHorizontallyCommand,
            IFlipGridVerticallyCommand flipGridVerticallyCommand,
            IRotateGrid180Command rotateGrid180Command,
            IRotateGrid90CWCommand rotateGrid90CWCommand,
            IRotateGrid90CCWCommand rotateGrid90CCWCommand,
            IUndoActionCommand undoActionCommand,
            IRedoActionCommand redoActionCommand,
            IHistoryActionService historyActionService,
            ICharToOEM437ConverterService charToOEM437ConverterService,
            IViewModelFactory viewModelFactory,
            IMatrixOperationsService matrixOperationsService,
            ISpriteGridStateService spriteGridStateService)
        {            
            this.spriteSavePathService = spriteSavePathService;
            this.canvasDrawingService = canvasDrawingService;
            this.historyActionService = historyActionService;
            this.charToOEM437ConverterService = charToOEM437ConverterService;
            this.viewModelFactory = viewModelFactory;
            this.matrixOperationsService = matrixOperationsService;
            this.spriteGridStateService = spriteGridStateService;
            historyActionService.SetMaxActionBuffer(10);

            QuitApplicationCommand = quitApplicationCommand;
            UndoCommand = undoActionCommand;
            RedoCommand = redoActionCommand;
            SaveSpriteCommand = saveSpriteCommand;
            SaveSpriteWithLocationCommand = saveSpriteWithLocationCommand;
            RotateGrid90CWCommand = rotateGrid90CWCommand;
            RotateGrid90CCWCommand = rotateGrid90CCWCommand;
            RotateGrid180Command = rotateGrid180Command;
            FlipGridHorizontallyCommand = flipGridHorizontallyCommand;
            FlipGridVerticallyCommand = flipGridVerticallyCommand;
            OpenSpriteCommand = openSpriteCommand;
            OpenWithLocationCommand = openSpriteWithLocationCommand;

            historyActionService.PropertyChanged += OnHistoryActionChanged;
            spriteGridStateService.PropertyChanged += OnDirtyCanvasChanged;
            RecentFiles.CollectionChanged += OnRecentFilesChanged;           
        }       

        private void OnHistoryActionChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (!args.PropertyName.Equals("HistoryChanged"))
                return;                             
            OnPropertyChanged(nameof(UndoAction));
            OnPropertyChanged(nameof(RedoAction));            
            UndoCommand.NotifyCanExecuteChanged();
            RedoCommand.NotifyCanExecuteChanged();
        }

        private void OnRecentFilesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {            
            OnPropertyChanged(nameof(CanBrowseRecents));            
        }

        private void OnDirtyCanvasChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (!args.PropertyName.Equals("IsDirty"))
                return;
            SaveSpriteCommand.NotifyCanExecuteChanged();
            SaveSpriteWithLocationCommand.NotifyCanExecuteChanged();
        }

        public string UndoAction
        {
            get
            {
                return historyActionService.UndoActionName;
            }
        }

        public string RedoAction
        {
            get
            {
                return historyActionService.RedoActionName;
            }
        }                                                             

        private ICommand importArtCommand;
        public ICommand ImportArtCommand
        {
            get
            {
                if (importArtCommand == null)
                {
                    importArtCommand = new RelayCommand(ImportArt);                        
                }
                return importArtCommand;
            }
        }

        private char InvalidCharacter
        {
            get
            {
                var e = CodePagesEncodingProvider.Instance.GetEncoding(437);
                var s = e.GetString(new byte[] { 32 });
                return s[0];
            }
        }

        private void ImportArt()
        {
            /*if (string.IsNullOrEmpty(ImportedArt))
                return;
            var lines = ImportedArt.Split('\n');
            GridHeight = lines.Length;
            var ordered = lines.OrderByDescending(x => x.Length);
            int leftPad = lines.Length <= 1 ? 0 : -1;
            GridWidth = ordered.First().Length + leftPad;
            OnGridResized();
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < GridWidth; x++)
                {
                    if (x >= line.Length)
                        continue;
                    var pixel = Pixels[y * GridWidth + x];
                    if (charLookup.ContainsKey(line[x]))
                    {
                        pixel.Character = line[x];
                        pixel.Color = SelectedColor;
                    }
                    else
                    {
                        pixel.Character = InvalidCharacter;
                        pixel.Color = ColorEntry.FromConsoleColor(ConsoleColor.Black);
                    }
                }
            }
            ImportedArt = string.Empty;
            OnPropertyChanged(nameof(GridHeight));
            OnPropertyChanged(nameof(GridWidth));
            OnPropertyChanged(nameof(PixelWidth));
            AddHistoryState("Import Art");
            IsDirty = true;*/
        }               

        private ICommand newSpriteCommand;
        public ICommand NewSpriteCommand
        {
            get
            {
                if (newSpriteCommand == null)
                {
                    newSpriteCommand = new RelayCommand(NewSprite);                    
                }
                return newSpriteCommand;
            }
        }

        private void NewSprite()
        {
            /*if (!DiscardChanges())
                return;
            if (GridWidth == 20 && GridHeight == 20)
            {                
                //Clear();
            }
            else
            {
                gridWidth = 20;
                gridHeight = 20;
                OnGridResized();
            }
            spriteGridStateService.SetGridVisibility(true);            
            IsDirty = false;*/
        }                                                

        private ICommand openCanvasDialogCommand;
        public ICommand OpenCanvasDialogCommand
        {
            get
            {
                if (openCanvasDialogCommand == null)
                {
                    openCanvasDialogCommand = new RelayCommand(OpenCanvasDialog);
                }
                return openCanvasDialogCommand;
            }
        }

        private void OpenCanvasDialog()
        {
            viewModelFactory.CreateViewModel<IScaleCanvasViewModel>();
        }
    }
}
