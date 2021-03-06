﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Squirrel.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.IO;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Squirrel;
    using Orc.Squirrel.Example.Services;
    using Squirrel.ViewModels;

    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IDispatcherService _dispatcherService;
        private readonly IUpdateService _updateService;
        private readonly IUpdateExecutableLocationService _updateExecutableLocationService;

        public MainViewModel(IUIVisualizerService uiVisualizerService, IDispatcherService dispatcherService,
            IUpdateService updateService, IUpdateExecutableLocationService updateExecutableLocationService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => updateService);

            _uiVisualizerService = uiVisualizerService;
            _dispatcherService = dispatcherService;
            _updateService = updateService;
            _updateExecutableLocationService = updateExecutableLocationService;
            CheckForUpdates = new TaskCommand(OnCheckForUpdatesExecuteAsync, OnCheckForUpdatesCanExecute);
            Update = new TaskCommand(OnUpdateExecuteAsync, OnUpdateCanExecute);
            ShowInstalledDialog = new Command(OnShowInstalledDialogExecute);

            Title = "Orc.Squirrel example";

#if DEBUG
            UpdateUrl = "https://downloads.wildgums.com/flexgrid/alpha";
            ExecutableFileName = Environment.ExpandEnvironmentVariables("%localappdata%\\WildGums\\Flex Grid_alpha\\FlexGrid.exe");
#endif
        }

        #region Properties
        public override string Title
        {
            get { return "Squirrel example"; }
        }

        public bool IsInstallingUpdate { get; private set; }

        public bool IsUpdateAvailable { get; private set; }

        public string UpdateUrl { get; set; }

        public string ExecutableFileName { get; set; }

        public int Progress { get; set; }
        #endregion

        #region Commands
        public TaskCommand CheckForUpdates { get; private set; }

        private bool OnCheckForUpdatesCanExecute()
        {
            if (string.IsNullOrWhiteSpace(UpdateUrl))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExecutableFileName))
            {
                return false;
            }

            return true;
        }

        private async Task OnCheckForUpdatesExecuteAsync()
        {
            UpdateCustomChannels();

            var result = await _updateService.CheckForUpdatesAsync(new SquirrelContext());
            IsUpdateAvailable = result.IsUpdateInstalledOrAvailable;
        }

        public TaskCommand Update { get; private set; }

        private bool OnUpdateCanExecute()
        {
            if (string.IsNullOrWhiteSpace(UpdateUrl))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExecutableFileName))
            {
                return false;
            }

            return true;
        }

        private async Task OnUpdateExecuteAsync()
        {
            UpdateCustomChannels();

            try
            {
                IsInstallingUpdate = true;

                await _updateService.InstallAvailableUpdatesAsync(new SquirrelContext());
            }
            finally
            {
                Progress = 0;

                IsInstallingUpdate = false;
            }
        }

        public Command ShowInstalledDialog { get; private set; }

        private void OnShowInstalledDialogExecute()
        {
            // Dispatch since we close the vm
            _dispatcherService.BeginInvoke(async () =>
            {
                await _uiVisualizerService.ShowDialogAsync<AppInstalledViewModel>();
                await CloseViewModelAsync(null);
            });
        }
        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _updateService.UpdateProgress += OnUpdateServiceProgress;
        }

        protected override async Task CloseAsync()
        {
            _updateService.UpdateProgress -= OnUpdateServiceProgress;

            await base.CloseAsync();
        }

        private void OnUpdateServiceProgress(object sender, SquirrelProgressEventArgs e)
        {
            Progress = e.Percentage;
        }

        private void UpdateCustomChannels()
        {
            var channels = new UpdateChannel[]
            {
                new UpdateChannel("Custom", UpdateUrl)
                {
                    IsPrerelease = true
                }
            };

            _updateService.Initialize(channels, channels[0], true);
        }

        private void OnExecutableFileNameChanged()
        {
            ((ExampleUpdateExecutableLocationService)_updateExecutableLocationService).ExecutableFileName = ExecutableFileName;
        }
    }
}
