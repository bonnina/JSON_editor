using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace JSON_editor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to source file: ");
            string filePath = Console.ReadLine();

            Console.WriteLine("\nEnter path to translations file: ");
            string translationsPath = Console.ReadLine();

            UpdateJsonFile(filePath, translationsPath);
            Console.ReadLine();
        }

        public static void UpdateJsonFile(string filePath, string translationsPath)
        {

            string result = string.Empty;

            using (StreamReader fr = new StreamReader(filePath))
            {
                var file_json = fr.ReadToEnd();
                var file_contents = JObject.Parse(file_json);

                using (StreamReader tr = new StreamReader(translationsPath))
                {
                    var transl_json = tr.ReadToEnd();
                    var transl_contents = JObject.Parse(transl_json);

                    foreach (var translation in transl_contents.Properties())
                    {
                        string english = translation.Value.SelectToken("ENG").Value<string>();
                        string german = translation.Value.SelectToken("GER").Value<string>();

                        foreach (var item in file_contents.Properties())
                        {
                            if (!String.IsNullOrEmpty(english) && !String.IsNullOrEmpty(german))
                            {
                                item.Value = item.Value.ToString().Replace(english, german);
                            }
                        }
                    }
                }

                result = file_contents.ToString();
            }

            File.WriteAllText(filePath, result);
        }
    }
}

//var file_contents = JObject.Parse(
//    @"{
//        '0': 'English',
//        '1': 'user'
//    }"
//);

//var transl_contents = JObject.Parse(
//    @"{
//        0: {
//            'id': '000'
//            'ENG': 'language',
//            'GER': 'Sprache'
//            },
//        1: {
//            'id': '001'
//            'ENG': 'user',
//            'GER': 'Nutzer'
//            }
//    }"
//);
