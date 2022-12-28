using ConsoleEngine.Editor.Services.Commands;
using ConsoleEngine.Editor.Services.Factories;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.SpriteGrid;
using DataModel.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
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
        private readonly IHistoryActionService historyActionService;
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
        public ICommand NewSpriteCommand { get; private set; }

        public SpriteEditorViewModel(
            INewSpriteCommand newSpriteCommand,
            IOpenSpriteCommand openSpriteCommand,
            IOpenSpriteWithLocationCommand openSpriteWithLocationCommand,
            IQuitApplicationCommand quitApplicationCommand,
            ISaveSpriteWithLocationCommand saveSpriteWithLocationCommand,
            ISaveSpriteCommand saveSpriteCommand,
            ISpriteSavePathService spriteSavePathService,
            IFlipGridHorizontallyCommand flipGridHorizontallyCommand,
            IFlipGridVerticallyCommand flipGridVerticallyCommand,
            IRotateGrid180Command rotateGrid180Command,
            IRotateGrid90CWCommand rotateGrid90CWCommand,
            IRotateGrid90CCWCommand rotateGrid90CCWCommand,
            IUndoActionCommand undoActionCommand,
            IRedoActionCommand redoActionCommand,
            IHistoryActionService historyActionService,
            IViewModelFactory viewModelFactory,
            ISpriteGridStateService spriteGridStateService)
        {            
            this.spriteSavePathService = spriteSavePathService;           
            this.historyActionService = historyActionService;            
            this.viewModelFactory = viewModelFactory;            
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
            NewSpriteCommand = newSpriteCommand;

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

        private ICommand? openCanvasDialogCommand;
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
