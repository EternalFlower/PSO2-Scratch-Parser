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

namespace PSO2_Scratch_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string AppStatus { get; set; }
        public string ContentStatus { get; set; }
        private ScratchList ScratchList;

        public bool Enabled { 
            get {
                return ScratchList != null && ScratchList.Count != 0;
            } 
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            ContentStatus = "Status: Nothing";
            AppStatus = "PSO2 Scratch Parser";
            

            //ScratchList scratchData = ScratchList.parseFromHTMLFile(@"C:\Users\Jimmy\Downloads\AC・SG・FUNスクラッチ _ 『PSO2』アイテムカタログ.html");
            //ScratchList scratchData = ScratchList.parseFromWebsiteURL(@"http://pso2.jp/players/catalog/scratch/ac/20210519/");
            //ScratchList = scratchData;
        }
    }
}
