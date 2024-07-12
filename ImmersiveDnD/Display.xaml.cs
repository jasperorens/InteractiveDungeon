using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ImmersiveDnD
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display : Window
    {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        private bool isClosing = false;
        private int displayedAmount;
        private int checker;

        public void TheBasics()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Closing += Display_Closing;
        }




        public Display()
        {
                InitializeComponent();
                WindowState = WindowState.Maximized;
                Closing += Display_Closing;
                SetImageSource();
        }

        public Display(string character, string area, string party, string sfx, int checker, int displayedAmount) : this()
        {
            //SFX
            if (!string.IsNullOrEmpty(sfx))
            {
                player = new System.Media.SoundPlayer(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", sfx));
                player.Play();
            }
            else
            {
                player.Stop();
            }


            //CHARACTERS MULTIPLE
            if (displayedAmount > 0)
            {
                switch (displayedAmount)
                {
                    case 1: ImDisChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", character))); break;
                    case 2: ImDisChar2.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", character))); break;
                    case 3: ImDisChar3.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", character))); break;
                    case 4: ImDisChar4.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", character))); break;
                }
            }
            else
            {
                //CHARACTERS SINGULAIR
                if (!string.IsNullOrEmpty(character))
                {
                    ImDisChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", character)));
                }
                else
                {
                    ImDisChar.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blank.png")));
                }
            }

            //AREA
            if (!string.IsNullOrEmpty(area))
            {
                ImDisplay.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Area", area)));
            }
            else
            {
                ImDisplay.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo", "logo.png")));
            }

            //PARTY
            if (!string.IsNullOrEmpty(party))
            {
                ImParty.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Party", party)));
            }
            else
            {
                ImParty.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blank.png")));
            }

        }

        //UPDATE blank
        public void UpdateImageSources()
        {
            Dispatcher.Invoke(() =>
            {
                ImDisChar.Source = GetImageSource("blank.png");
                ImDisChar2.Source = GetImageSource("blank.png");
                ImDisChar3.Source = GetImageSource("blank.png");
                ImDisChar4.Source = GetImageSource("blank.png");
                ImDisplay.Source = GetImageSource(System.IO.Path.Combine("logo", "logo.png"));
                player.Stop();
            });
        }


        public void UpdateImageSources(string character, string area, string party, string sfx, int checker, int displayedAmount)
        {
            // SFX
            if (!string.IsNullOrEmpty(sfx))
            {
                string sfxPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", sfx);
                if (File.Exists(sfxPath))
                {
                    player = new System.Media.SoundPlayer(sfxPath);
                    player.Play();
                }
                else
                {
                    player.Stop();
                }
            }
            else
            {
                player.Stop();
            }

            // CHARACTERS MULTIPLE
            if (displayedAmount > 0)
            {
                switch (displayedAmount)
                {
                    case 1: ImDisChar.Source = GetImageSource(System.IO.Path.Combine("Characters", character)); break;
                    case 2: ImDisChar2.Source = GetImageSource(System.IO.Path.Combine("Characters", character)); break;
                    case 3: ImDisChar3.Source = GetImageSource(System.IO.Path.Combine("Characters", character)); break;
                    case 4: ImDisChar4.Source = GetImageSource(System.IO.Path.Combine("Characters", character)); break;
                }
            }
            else
            {
                ResetCharacterImages();
                if (!string.IsNullOrEmpty(character))
                {
                    ImDisChar.Source = GetImageSource(System.IO.Path.Combine("Characters", character));
                }
                else
                {
                    ImDisChar.Source = GetImageSource("blank.png");
                }
            }

            // AREA
            ImDisplay.Source = !string.IsNullOrEmpty(area)
                ? GetImageSource(System.IO.Path.Combine("Area", area))
                : GetImageSource(System.IO.Path.Combine("logo", "logo.png"));

            // PARTY
            ImParty.Source = !string.IsNullOrEmpty(party)
                ? GetImageSource(System.IO.Path.Combine("Party", party))
                : GetImageSource("blank.png");
        }

        private void ResetCharacterImages()
        {
            ImDisChar.Source = GetImageSource("blank.png");
            ImDisChar2.Source = GetImageSource("blank.png");
            ImDisChar3.Source = GetImageSource("blank.png");
            ImDisChar4.Source = GetImageSource("blank.png");
        }

        private BitmapImage GetImageSource(string relativePath)
        {
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (File.Exists(fullPath))
            {
                return new BitmapImage(new Uri(fullPath));
            }
            else
            {
                MessageBox.Show($"Image not found: {relativePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blank.png")));
            }
        }

        //closing events
        private void Display_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Prevent the Closing event from being called again if it's already processing
            if (isClosing)
            {
                return;
            }
            try
            {
                isClosing = true;
                player.Stop();
                player.Dispose(); // Release resources used by the sound player
                isClosing = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while closing the window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void CloseWindow()
        {
            isClosing = true;
            player.Stop();
            player.Dispose();
            Close();
        }

        public void SetImageSource()
        {
            // Get the path to the "logo.png" file in the "bin/Debug/logo" folder
            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo", "logo.png");

            // Create a BitmapImage object and set it as the source for the ImDisplay Image control
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            ImDisplay.Source = bitmapImage;
        }
    }
}
