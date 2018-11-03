using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PowershellGUI
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /*
     * Mappe-struktur filplassering lese fra en settings-fil
     * slik at vi kan flytte mappestrukturen 
     * 
     * GUI tankemåte
     * 
     * AD (hardkodet, mappe)
     *      MappevValg (Dynamisk antall mapper)
     *          psAdScript1.ps1
     *              Les kommentarer i PS-fil, Description, Header og deretter X antall kommandolinjeargumenter som skal inn til PS-filen
     *              Dette vil se slik ut: 
     *              <#
     *              Description = "beskrivelse"
     *              Header = "Funksjonsnavn"
     *              Output = "True"
     *              [string]Username = "beskrivelse av CLI"
     *              [int]SomeNumber = "beskrivelse av somenumber"
     *              [bool]SomeBool = "beskrivelse av someBool"
     *              #>
     *              
     *              Opprett antall input bokser tilsvarende antall kommando linje argumenter
     *              Les beskrivelsen til hver kommandolinjeargument
     *              Legg til beskrivelse tilhørende hver enkelt input boks
     *              
     *          psADScript2.ps1
     *              ...
     *              
                psADScript3.ps1

                ...
     *
     * 
     *      
     * Exchange (hardkodet, mappe)
     *      MappevValg (Dynamisk antall mapper)
     *          exchangeScript1.ps1
     *          exchangeScript2.ps1
     * Skype (hardkodet, mappe)
     *      Mappevalg (Dynamisk antall mapper)
     *          skypeScript1.ps1
     */
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            }

        /*
         * TEST-FUNKSJONER, SLETT SENERE
         * 
         */ 
        private void Button_Click(object sender, RoutedEventArgs e)
            {
            Test test = new Test();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder = test.GetPowershellScript();
            this.OutputTextBlock.AppendText(stringBuilder.ToString());
            }

        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
            {

            }

        // populates the dropdown box 
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
            {
            // relativ filsti til psScripts mappe
            string filePath = @"../../psScripts";
            string[] files = System.IO.Directory.GetFiles(filePath);
            var comboBox = (ComboBox)sender;
            comboBox.ItemsSource = files;
            int firstItem = 0;
            comboBox.SelectedIndex = firstItem;
            }
        }
    }
