﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowershellGUI
{
    /// <summary>
    /// Interaction logic for DynamicUserControl.xaml
    /// </summary>
    public partial class DynamicUserControl : UserControl
    {
        public DynamicUserControl()
        {
            InitializeComponent();
        }

        private void DynamicButton_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DynamicButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
