using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ImmersiveDnDLib
{
    public class SFXClass
    {
        public List<string> soundList = new List<string>();
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        Dictionary<int, string> selectedButtons = new Dictionary<int, string>();
        private SoundPlayer soundPlayer;


        public string num1 { get; set; }
        public string num2 { get; set; }
        public string num3 { get; set; }
        public string num4 { get; set; }
        public string num5 { get; set; }
        public string num6 { get; set; }
        public string num7 { get; set; }
        public string num8 { get; set; }
        public string num9 { get; set; }


        public SFXClass()
        {
            LoadSounds();
        }

        public string GetSound(string sender)
        {
            //check CSV file which number coaralates to the sender and then play this sound
            //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", CbSFX.SelectedValue.ToString().Replace(' ', '_') + ".wav");
            //player = new System.Media.SoundPlayer(path);
            //player.Play();


            //Get the path to the audiofile
            string path = "";

            switch (sender)
            {
                case "1": sender = num1; break;
                case "2": sender = num2; break;
                case "3": sender = num3; break;
                case "4": sender = num4; break;
                case "5": sender = num5; break;
                case "6": sender = num6; break;
                case "7": sender = num7; break;
                case "8": sender = num8; break;
                case "9": sender = num9; break;

            }

            string soundName = "1";

            if (sender == "7")
            {
                soundName = num7?.Replace(" ", "_");
            }
            else if (sender == "8")
            {
                soundName = num8?.Replace(" ", "_");
            }
            else if (sender == "9")
            {
                soundName = num9?.Replace(" ", "_");
            }

            else if (sender == "4")
            {
                soundName = num4?.Replace(" ", "_");
            }
            else if (sender == "5")
            {
                soundName = num5?.Replace(" ", "_");
            }

            else if (sender == "6")
            {
                soundName = num6?.Replace(" ", "_");
            }
            else if (sender == "1")
            {
                soundName = num1?.Replace(" ", "_");
            }
            else if (sender == "2")
            {
                soundName = num2?.Replace(" ", "_");
            }
            else if (sender == "3")
            {
                soundName = num3?.Replace(" ", "_");
            }
            else if (sender == "0")
            {
                //player2.Stop();
            }

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SoundBoard", sender);

            return path;
        }



        public void LoadSounds()
        {
            string directoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SoundBoard");

            string[] sounds = Directory.GetFiles(directoryPath);
            foreach (var sound in sounds)
            {
                int characterIndex = sound.IndexOf("SoundBoard");
                if (characterIndex != -1)
                {
                    string filename = sound.Substring(characterIndex + "SoundBoard".Length);
                    soundList.Add(filename.Split('.')[0].Replace("_", " "));
                }

            }
            ReadCSV();
        }


        public void ReadCSV()
        {

            using (StreamReader sr = new StreamReader("buttonList.txt"))
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string[] values = sr.ReadLine().Split(';');
                    if (values.Length >= 0)
                    {
                        string value = values[1];
                        int key = Convert.ToInt32(values[0]);
                        
                        if (selectedButtons.Count > 8)
                        {
                            return;
                        }
                        else
                        {
                            selectedButtons.Add(key, value);
                        }
                    }
                    i++;
                }
            }


            int index = 1;
            foreach (var value in selectedButtons.Values)
            {
                string propertyName = $"num{index}";
                var propertyInfo = GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(this, value);
                }

                index++;
            }

        }
    }

}
