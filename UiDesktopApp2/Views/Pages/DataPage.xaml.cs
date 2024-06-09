﻿using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
