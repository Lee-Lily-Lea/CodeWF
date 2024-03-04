﻿namespace CodeWF.Tools.Desktop.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INotificationService _notificationService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IRegionManager _regionManager;

    public ObservableCollection<ToolMenuItem> SearchMenuItems { get; set; }
    private MenuItem? _selectedMenuItem;

    public MenuItem? SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
            SetProperty(ref _selectedMenuItem, value);
            ChangeTool();
        }
    }

    public ObservableCollection<ToolMenuItem> MenuItems { get; private set; }

    public MainViewModel(IToolManagerService toolManagerService, INotificationService notificationService,
        IEventAggregator eventAggregator, IRegionManager regionManager)
    {
        Title = AppInfo.Name;
        _notificationService = notificationService;
        _eventAggregator = eventAggregator;
        _regionManager = regionManager;
        RegisterPrismEvent();
        SearchMenuItems = new ObservableCollection<ToolMenuItem>();
        MenuItems = toolManagerService.MenuItems;
        MenuItems.CollectionChanged += ToolListChanged;
    }

    private void RegisterPrismEvent()
    {
        _eventAggregator.GetEvent<TestEvent>().Subscribe(args =>
        {
            _notificationService?.Show("Prism Event",
                $"主工程Prism事件处理程序：Args = {args.Args}, Now = {DateTime.Now}");
        });
    }

    private void ToolListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SearchMenuItems.Clear();
        MenuItems.ForEach(firstMenuItem =>
        {
            if (firstMenuItem.Children.Any())
            {
                firstMenuItem.Children.ForEach(secondMenuItem => SearchMenuItems.Add(secondMenuItem));
            }
        });
    }

    private void ChangeTool()
    {
        //_regionManager.RequestNavigate(RegionNames.ContentRegion, _selectedMenuItem.Header);
    }
}