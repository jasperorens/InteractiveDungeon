using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using ImmersiveDnDLib;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace ImmersiveDnD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /////////////////////////////////////////////////////////////////////////Class Variables/////////////////////////////////////////////////////
        SFXClass sFXClass = new SFXClass();
        int checker;
        bool isOpen = false;
        bool isSound = false;
        private Display displayWindow;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        System.Media.SoundPlayer player2 = new System.Media.SoundPlayer();
        bool notification = false;
        int charCurrentPage = 1;
        int selectedNumber;
        string []CharacterSelected = new string[4];
        int characterCounterAdded;
        bool charWipe = false;

        /////////////////////////////////////////////////////////////////////////Initialiser/////////////////////////////////////////////////////

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            //LoadInListBox("Area");
            LoadInListBox("Characters");
            LoadInListBox("SFX");
            LoadInListBox("Party");
            LoadSFX();
            CbCharLTLoader();
        }

        /////////////////////////////////////////////////////////////////////////Listbox Loader/////////////////////////////////////////////////////

        public void LoadInListBox(string sender)
        {
            string directoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sender);

            string[] pictures = Directory.GetFiles(directoryPath);
            foreach (string picture in pictures)
            {
                int characterIndex = picture.IndexOf($"\\{sender}\\");
                if (characterIndex != -1)
                {
                    string filename = picture.Substring(characterIndex + $"\\{sender}\\".Length);
                    if (sender == "Characters")
                    {
                        CbCharacters.Items.Add(filename.Split('.')[0].Replace("_", " "));
                    }
                    else if (sender == "Area")
                    {
                        CbArea.Items.Add(filename.Split('.')[0].Replace("_", " "));
                    }
                    else if (sender == "SFX")
                    {
                        CbSFX.Items.Add(filename.Split('.')[0].Replace('_', ' '));
                    }
                    else if (sender == "Party")
                    {
                        CbParty.Items.Add(filename.Split('.')[0].Replace('_', ' '));
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////Selection/Modifier/LoadIn/////////////////////////////////////////////////////
        
        private void CbCharLTLoader()
        {
            string charactersFolderPath = (System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters"));

            // Check if the 'Characters' folder exists
            if (!Directory.Exists(charactersFolderPath))
            {
                MessageBox.Show("Characters folder not found!");
                return;
            }

            // Use a HashSet to store unique race names
            HashSet<string> raceNames = new HashSet<string>();

            // Get all files in the 'Characters' folder
            string[] characterFiles = Directory.GetFiles(charactersFolderPath);

            foreach (string characterFile in characterFiles)
            {
                // Extract the filename without extension
                string fileName = System.IO.Path.GetFileNameWithoutExtension(characterFile);

                // Split the filename by '_' to get the race name
                string[] nameParts = fileName.Split('_');

                if (nameParts.Length >= 1)
                {
                    // Add the race name to the HashSet
                    raceNames.Add(nameParts[0]);
                }
            }

            // Add unique race names to the ComboBox
            foreach (string raceName in raceNames)
            {
                CbCharLT.Items.Add(raceName);
            }
        }

        /////////////////////////////////////////////////////////////////////////Selection/Modifier/////////////////////////////////////////////////////
        private void CbAreaLT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Indien de selectie van het area type veranderd word word de listbox van de images eronder aangepast aan de hand van de AreaListClass
            string areaType = CbAreaLT.SelectedValue.ToString().Split(':')[1].Split(' ')[1];
            AreaListClass areaListClass = new AreaListClass();
            switch (areaType)
            {
                case "Forests":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Forest"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Caves":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Cave"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Dungeons":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Dungeon"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Mansions":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Mansion"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Towns":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Town"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Plains":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Plain"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Mountains":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Mountain"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Islands":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Island"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Swamps":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Swamp"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "Specials":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("Special"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;

                case "All":
                    CbArea.Items.Clear();
                    foreach (string area in areaListClass.Fillbox("All"))
                    {
                        CbArea.Items.Add(area);
                    }
                    break;
            }
        }

        private void CbCharLT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCharLT.SelectedValue == null || CbCharLT.SelectedIndex == -1)
            {
                // Load all characters if no specific species is selected
                LoadAllCharacters();
            }
            else
            {
                string characterSpecies = CbCharLT.SelectedValue.ToString();
                CharacterListClass characterListClass = new CharacterListClass();
                CbCharacters.Items.Clear();

                foreach (string character in characterListClass.Fillbox(characterSpecies))
                {
                    CbCharacters.Items.Add(character);
                }
            }
        }

        private void LoadAllCharacters()
        {
            CharacterListClass characterListClass = new CharacterListClass();
            CbCharacters.Items.Clear();

            foreach (string character in characterListClass.Fillbox("All"))
            {
                CbCharacters.Items.Add(character);
            }
        }



        /////////////////////////////////////////////////////////////////////////Selection/Selector/////////////////////////////////////////////////////

        private void CbCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCharacters.SelectedIndex == -1)
            {
                ImChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectimage.png")));
            }
            else
            {
                ImChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", CbCharacters.SelectedValue.ToString().Replace(" ", "_") + ".png")));
            }
        }

        private void CbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbArea.SelectedValue == null || CbArea.SelectedIndex == -1)
            {
                ImAr.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectarea.png")));
            }
            else
            {
                ImAr.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Area", CbArea.SelectedValue.ToString().Replace(" ", "_") + ".png")));
            }
        }

        private void CbSFX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbSFX.SelectedValue == null || CbSFX.SelectedIndex == -1)
            {
                isSound = true;
                ImSFXplay.Visibility = Visibility.Collapsed;
                ImSFXstop.Visibility = Visibility.Collapsed;
                ImSFX.Visibility = Visibility.Visible;
            }
            else
            {
                isSound = false;
                ImSFXplay.Visibility = Visibility.Visible;
                ImSFXstop.Visibility = Visibility.Visible;
                ImSFX.Visibility = Visibility.Collapsed;
            }
        }


        private void CbParty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbParty.SelectedValue == null || CbParty.SelectedIndex == -1)
            {
                ImParty.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectParty.png")));
            }
            else
            {
                ImParty.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Party", CbParty.SelectedValue.ToString().Replace(" ", "_") + ".png")));
            }
        }



        /////////////////////////////////////////////////////////////////////////Main Buttons/////////////////////////////////////////////////////

        private void BtnMap_Click(object sender, RoutedEventArgs e)
        {
            string sfx;
            if (CbSFX.SelectedValue != null)
            {
                sfx = CbSFX.SelectedValue.ToString().Replace(" ", "_") + ".wav";
            }
            else
            {
                sfx = "";
            }



                if (isOpen == false)
                {
                    displayWindow = new Display("", "", "", sfx, 0, 0);
                    displayWindow.Show();
                    isOpen = true;
                }
            else
            {
                if (CbSFX.SelectedValue != null)
                {
                    displayWindow?.UpdateImageSources("", "", "", sfx, 0, 0);
                }
                else
                {
                    displayWindow?.UpdateImageSources();
                }
            }
        }

        private void BtnDisplay_Click(object sender, RoutedEventArgs e)
        {
            string character;
            string area;
            string party;
            string sfx;
            int displayedAmount;

            sfx = (CbSFX.SelectedValue != null) ? CbSFX.SelectedValue.ToString().Replace(" ", "_") + ".wav" : "";
            character = (CbCharacters.SelectedValue != null) ? CbCharacters.SelectedValue.ToString().Replace(" ", "_") + ".png" : "";
            area = (CbArea.SelectedValue != null) ? CbArea.SelectedValue.ToString().Replace(" ", "_") + ".png" : "";
            party = (CbParty.SelectedValue != null) ? CbParty.SelectedValue.ToString().Replace(" ", "_") + ".png" : "";
            
            //only music
            if ((CbCharacters.SelectedValue == null) && (CbArea.SelectedValue == null))
            {
                //only music
                if (isOpen == false)
                {
                    displayWindow = new Display(character, area, party, sfx, 0, 0);
                    displayWindow.Show();
                    isOpen = true;
                }
                else
                {
                    if (CbSFX.SelectedValue != null)
                    {
                        displayWindow?.UpdateImageSources(character, area, party, sfx, 0, 0);
                    }
                    else
                    {
                        displayWindow?.UpdateImageSources();
                    }
                }
            }

            //only area
            else if (CbCharacters.SelectedValue == null)
            {
                checker = 1;
                if (isOpen == false)
                {
                    displayWindow = new Display(character, area, party, sfx, 0, 0);
                    displayWindow.Show();
                    isOpen = true;
                }
                else
                {
                    if (CbSFX.SelectedValue != null)
                    {
                        if (CbParty.SelectedValue != null)
                        {
                            displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);
                        }
                        else
                        {
                            displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);
                        }
                    }
                    else
                    {
                        if (CbParty.SelectedValue != null)
                        {
                            displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);
                        }
                        else
                        {
                            displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);
                        }
                    }
                }
            }

            //only character
            else if (CbArea.SelectedValue == null)
            {
                if ((CharacterSelected[0] == null) || (charWipe == true)) ///////////////////////////////////////
                {
                    checker = 2;

                    if (isOpen == false)
                    {
                        displayWindow = new Display(character, area, party, sfx, 0, 0);
                        displayWindow.Show();
                        isOpen = true;
                    }
                    else
                    {
                        displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);
                    }
                    charWipe = false;
                }

                else ///////////////////////////////////
                {
                    displayedAmount = 1;
                    foreach(string characterX in CharacterSelected)
                    {
                        if (characterX != null)
                        {
                            checker = 2;
                            string value = characterX.Replace(" ", "_") + ".png";
                            if (isOpen == false)
                            {
                                displayWindow = new Display(value, area, party, sfx, 0, displayedAmount);
                                displayWindow.Show();
                                isOpen = true;
                                displayedAmount++;
                            }
                            else
                            {
                                displayWindow?.UpdateImageSources(value, area, party, sfx, checker, displayedAmount);
                                displayedAmount++;
                            }
                        }
                        //clear value
                        ClearChar();
                    }
                }
            }

            //character & area /////////////////////////////////////////////////////////////
            if ((CbCharacters.SelectedValue != null) && (CbArea.SelectedValue != null)) 
            {
                if ((CharacterSelected[0] == null) || (charWipe == true))
                {
                    checker = 3;
                    if (isOpen == false)
                    {
                        displayWindow = new Display(character, area, party, sfx, 0, 0);
                        displayWindow.Show();
                        isOpen = true;

                    }
                    else
                    {
                        displayWindow?.UpdateImageSources(character, area, party, sfx, checker, 0);

                    }
                    charWipe = false;
                }


                else ///////////////////////////////////
                {

                    checker = 3;
                    displayedAmount = 1;
                    foreach (string characterX in CharacterSelected)
                    {
                        if (characterX != null)
                        {
                            string value = characterX.Replace(" ", "_") + ".png";
                            if (isOpen == false)
                            {
                                displayWindow = new Display(value, area, party, sfx, 0, displayedAmount);
                                displayWindow.Show();
                                isOpen = true;
                                displayedAmount++;
                            }
                            else
                            {
                                displayWindow?.UpdateImageSources(value, area, party, sfx, checker, displayedAmount);
                                displayedAmount++;
                            }
                        }

                        //clear value
                        ClearChar();
                    }
                }
            }
        }

        private void BtnUploadMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFile("logo");
        }

        private void BtnUploaldParty_Click(object sender, RoutedEventArgs e)
        {
            OpenFile("Party");
        }

        /////////////////////////////////////////////////////////////////////////Clear Value Buttons/////////////////////////////////////////////////////

        private void BtnClearChar_Click(object sender, RoutedEventArgs e)
        {
            ClearChar();
        }

        private void ClearChar()
        {
            charWipe = true;
            charCurrentPage = 1;
            CbCharacters.SelectedIndex = -1;
            CbCharLT.SelectedIndex = -1;
            string[] CharacterSelected = new string[4];
            CharacterSelected[0] = null;
            CharacterSelected[1] = null;
            CharacterSelected[2] = null;
            CharacterSelected[3] = null;
            characterCounterAdded = 0;
            CbCharNum.SelectedIndex = 0;
            LbCurrent.Content = charCurrentPage;
        }

        private void BtnClearArea_Click(object sender, RoutedEventArgs e)
        {
            CbArea.SelectedIndex = -1;
        }

        private void BtnClearSFX_Click(object sender, RoutedEventArgs e)
        {
            CbSFX.SelectedIndex = -1;
            player.Stop();
        }


        /////////////////////////////////////////////////////////////////////////Audio Buttons/////////////////////////////////////////////////////

        private void ImSFXplay_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", CbSFX.SelectedValue.ToString().Replace(' ', '_') + ".wav");
            player = new System.Media.SoundPlayer(path);
            player.Play();
        }

        private void ImSFXstop_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.Stop();
        }


        /////////////////////////////////////////////////////////////////////////Add Buttons/////////////////////////////////////////////////////

        private void OpenFile(string sender)
        {
            if (notification == false)
            {
                MessageBox.Show("Please make sure to read the README file for images");
                notification = true;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            if ((sender == "SFX") || (sender == "SoundBoard"))
            {
                ofd = new OpenFileDialog
                {
                    Filter = "All files (*.*)|*.*|Imagefiles (*.wav) |*.wav",
                    FilterIndex = 2,
                    FileName = "",
                    Multiselect = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                };
            }
            else
            {
                if (sender != "logo")
                {
                    ofd = new OpenFileDialog
                    {
                        Filter = "All files (*.*)|*.*|Imagefiles (*.png) |*.png",
                        FilterIndex = 2,
                        FileName = "",
                        Multiselect = true,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    };
                }
                else
                {
                    ofd = new OpenFileDialog
                    {
                        Filter = "All files (*.*)|*.*|Imagefiles (*.png) |*.png",
                        FilterIndex = 2,
                        FileName = "",
                        Multiselect = false,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    };
                }

            }



            if (ofd.ShowDialog() == true)
            {
                string destinationFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sender); //SFX, Characters, SoundBoard, Area


                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string file in ofd.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(file);

                    string destinationPath;
                    if (sender == "logo")
                    {
                        destinationPath = System.IO.Path.Combine(destinationFolder, "logo.png");
                    }
                    else
                    {
                        destinationPath = System.IO.Path.Combine(destinationFolder, fileName);
                    }
                    System.IO.File.Copy(file, destinationPath, true);

                    if (sender == "Characters")
                    {
                        CbCharacters.Items.Add(fileName.Replace("_", " ").Split('.')[0]);
                    }
                    else if (sender == "Area")
                    {
                        CbArea.Items.Add(fileName.Replace("_", " ").Split('.')[0]);
                    }
                    else if (sender == "SFX")
                    {
                        CbSFX.Items.Add(fileName.Replace("_", " ").Split('.')[0]);
                    }
                    else if (sender == "Party")
                    {
                        CbParty.Items.Add(fileName.Replace('_', ' ').Split('.')[0]);
                    }
                    else if (sender == "logo")
                    {
                        System.IO.File.Copy(file, destinationPath, true);
                        Display display = new Display();
                    }

                }
                
                MessageBox.Show("File copied successfully.");
            }

        }

        private void BtnUploadChar_Click(object sender, RoutedEventArgs e)
        {
            OpenFile("Characters");
            CbCharacters.Items.Clear();
            LoadInListBox("Characters");
        }

        private void BtnUploadArea_Click(object sender, RoutedEventArgs e)
        {
            OpenFile("Area");
            CbArea.Items.Clear();
            LoadInListBox("Area");
        }

        private void BtnUploadSFX_Click(object sender, RoutedEventArgs e)
        {
            OpenFile("SFX");
            CbSFX.Items.Clear();
            LoadInListBox("SFX");
        }

        /////////////////////////////////////////////////////////////////////////Soundboard/////////////////////////////////////////////////////

        private void BtnSound_Click(object sender, RoutedEventArgs e)
        {
            if (sender.ToString().Split(':')[1].Split(' ')[1] != "0")
            {
                string path = sFXClass.GetSound(sender.ToString().Split(':')[1].Split(' ')[1]);
                player2 = new System.Media.SoundPlayer(path);
                player2.Play();
            }
            else
            {
                player2 = new System.Media.SoundPlayer();
                player2.Stop();
            }

        }

        private void LoadSFX()
        {
            sFXClass.LoadSounds();


            Lb1.Content = sFXClass.num1?.Replace("_", " ").Split('.')[0];
            Lb2.Content = sFXClass.num2?.Replace("_", " ").Split('.')[0];
            Lb3.Content = sFXClass.num3?.Replace("_", " ").Split('.')[0];
            Lb4.Content = sFXClass.num4?.Replace("_", " ").Split('.')[0];
            Lb5.Content = sFXClass.num5?.Replace("_", " ").Split('.')[0];
            Lb6.Content = sFXClass.num6?.Replace("_", " ").Split('.')[0];
            Lb7.Content = sFXClass.num7?.Replace("_", " ").Split('.')[0];
            Lb8.Content = sFXClass.num8?.Replace("_", " ").Split('.')[0];
            Lb9.Content = sFXClass.num9?.Replace("_", " ").Split('.')[0];
        }

        private void setUpBoard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSound_KeyDown(object sender, KeyEventArgs e)
        {
            //door middel van de SFXClass word de numpad uitgelezen en correct geplaatst
            string soundName = "1";


            if (e.Key == Key.NumPad7)
            {
                soundName = sFXClass.num7?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad8)
            {
                soundName = sFXClass.num8?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad9)
            {
                soundName = sFXClass.num9?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad4)
            {
                soundName = sFXClass.num4?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad5)
            {
                soundName = sFXClass.num5?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad6)
            {
                soundName = sFXClass.num6?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad1)
            {
                soundName = sFXClass.num1?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad2)
            {
                soundName = sFXClass.num2?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad3)
            {
                soundName = sFXClass.num3?.Replace(" ", "_");
            }
            else if (e.Key == Key.NumPad0)
            {
                player.Stop();
            }
            else
            {
                return;
            }
            if (!string.IsNullOrEmpty(soundName))
            {
                if (e.Key != Key.NumPad0)
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SoundBoard", soundName.Split('.')[0] + ".wav");
                    player2 = new System.Media.SoundPlayer(path);
                    player2.Play();
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////Character Number/////////////////////////////////////////////////////

        private void CbCharNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateControlVisibility();
            selectedNumber = Convert.ToInt32(CbCharNum.SelectedIndex) + 1;
            LbTotal.Content = selectedNumber.ToString();

            if (selectedNumber < charCurrentPage)
            {
                charCurrentPage = selectedNumber;
                ArrowLeft();
                arrowRight.Visibility = Visibility.Hidden;
                Display_Character();
            }
        }

        private void Display_Character()
        {
            if (charWipe == false) 
            {
                if (CharacterSelected[charCurrentPage - 1] != null)
                {
                    ImChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", CharacterSelected[charCurrentPage - 1].Replace(" ", "_") + ".png")));
                }
                else
                {
                    ImChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectimage.png")));
                }
            }
            else
            {
                ImChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectimage.png")));
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateControlVisibility();
        }

        private void BtnAddChar_Click(object sender, RoutedEventArgs e)
        {
            if (CbCharacters.SelectedValue != null)
            {
                string characterName = CbCharacters.SelectedValue.ToString();
                int maximum = Convert.ToInt32(CbCharNum.SelectedValue.ToString().Split(' ')[1]);

                if (characterCounterAdded <= (maximum - 1))
                {
                    CharacterSelected[characterCounterAdded] = characterName;
                    characterCounterAdded++;
                    MessageBox.Show("Character added");

                    if (characterCounterAdded <= (maximum - 1))
                    {
                        ArrowRight();
                    }
                }
                else
                {
                    BtnAddChar.IsEnabled = false;
                    BtnAddChar.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("Please select a character first.");
            }
        }


        private void UpdateControlVisibility()
        {

            if (CbCharNum.SelectedIndex > 0)
            {
                BtnAddChar.IsEnabled = true;
                BtnAddChar.Visibility = Visibility.Visible;
                if (arrowRight != null)
                {
                    arrowRight.Visibility = Visibility.Visible;
                }
                if (arrowLeft != null && charCurrentPage > 1)
                {
                    arrowLeft.Visibility = Visibility.Visible;
                }

            }
            else
            {
                BtnAddChar.Visibility = Visibility.Hidden;
                if (arrowRight != null)
                {
                    arrowRight.Visibility = Visibility.Hidden;
                }
                if (arrowLeft != null)
                {
                    arrowLeft.Visibility = Visibility.Hidden;
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////Arrow Buttons/////////////////////////////////////////////////////

        private void arrowLeft_Click(object sender, RoutedEventArgs e)
        {
            charCurrentPage--;
            selectedNumber = Convert.ToInt32(CbCharNum.SelectedIndex) + 1;
            ArrowLeft();
            Display_Character();
        }

        private void ArrowLeft()
        {
            LbCurrent.Content = charCurrentPage.ToString();
            if (selectedNumber != charCurrentPage)
            {
                arrowRight.Visibility = Visibility.Visible;
            }
            if (charCurrentPage > 1)
            {
                arrowLeft.Visibility = Visibility.Visible;
            }

            if (charCurrentPage == 1)
            {
                arrowLeft.Visibility = Visibility.Hidden;
            }
        }

        private void ArrowRight()
        {
            charCurrentPage++;
            selectedNumber = Convert.ToInt32(CbCharNum.SelectedIndex) + 1;
            LbCurrent.Content = charCurrentPage.ToString();
            if (selectedNumber == charCurrentPage)
            {
                arrowRight.Visibility = Visibility.Hidden;
                arrowLeft.Visibility = Visibility.Visible;
            }
            if (charCurrentPage > 1)
            {
                arrowLeft.Visibility = Visibility.Visible;
            }

            if (charCurrentPage == 1)
            {
                arrowLeft.Visibility = Visibility.Hidden;
            }
            Display_Character();
        }

        private void arrowRight_Click(object sender, RoutedEventArgs e)
        {
            ArrowRight();
        }

        private void BtnClearParty_Click(object sender, RoutedEventArgs e)
        {
            CbParty.SelectedIndex = -1;
            Party("blank");
        }

        public void Party(string sender)
        {
            if (sender == "blank")
            {
                ImParty.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectParty.png")));
            }
        }
    }
}
