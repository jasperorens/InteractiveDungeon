using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmersiveDnDLib
{
    public class CharacterListClass
    {
        public CharacterListClass() 
        { 

        }

        public List<string> Fillbox(string sender)
        {
            List<string> selectedPictures = new List<string>();
            string directoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters");
            string[] pictures = Directory.GetFiles(directoryPath);


            if (sender == "All")
            {
                selectedPictures.Clear();
                foreach (string picture in pictures)
                {
                    int characterIndex = picture.IndexOf("Characters");
                    string filename = picture.Substring(characterIndex + "Characters".Length);

                    string pictureChecker = picture.Substring(characterIndex + "Characters".Length).Split('\\')[1].Split('_')[0];
                    string correctFileName = filename.Split('\\')[1].Split('.')[0].Replace('_', ' ');
                    selectedPictures.Add(correctFileName);
                }
            }
            else
            {
                selectedPictures.Clear();
                foreach (string picture in pictures)
                {
                    int characterIndex = picture.IndexOf("Characters");
                    string filename = picture.Substring(characterIndex + "Characters".Length);

                    string pictureChecker = picture.Substring(characterIndex + "Characters".Length).Split('\\')[1].Split('_')[0];
                    if (pictureChecker == sender)
                    {
                        selectedPictures.Add(filename.Split('\\')[1].Split('.')[0].Replace('_', ' '));
                    }
                }
            }
            return selectedPictures;

        }


    }
}
