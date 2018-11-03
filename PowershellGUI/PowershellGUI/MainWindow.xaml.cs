using System.Windows;

namespace PowershellGUI
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /*
     * GUI tankemåte
     * AD
     *      psAdScript1.ps1
     *          Les kommentarer i PS-fil, Description, Header og deretter X antall kommandolinjeargumenter som skal inn til PS-filen
     *          Dette vil se slik ut: 
     *          <#
     *          Description = "beskrivelse"
     *          Header = "Funksjonsnavn"
     *          Output = "true"
     *          [string]Username = "beskrivelse av CLI"
     *          [int]SomeNumber = "beskrivelse av somenumber"
     *          [bool]SomeBool = "beskrivelse av someBool"
     *          #>
     *          
     *          Opprett antall input bokser tilsvarende antall kommando linje argumenter
     *          Les beskrivelsen til hver kommandolinjeargument
     *          Legg til beskrivelse tilhørende hver enkelt input boks
     *          
     *      psADScript2.ps1
     *          ...
     *          
            psADScript3.ps1
            ...
     *      
     * Exchange
     * 
     */
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            }
        }
    }
