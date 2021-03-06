﻿using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace JSON_editor
{
    class Program
    {
        static void Main()
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

            if (File.Exists(filePath) && File.Exists(translationsPath))
            {
                var fileJson = File.ReadAllText(filePath);
                var fileContents = JObject.Parse(fileJson);

                
                var translJson = File.ReadAllText(translationsPath);
                var translContents = JObject.Parse(translJson);

                foreach (var translation in translContents.Properties())
                {
                    string english = translation.Value.SelectToken("ENG").Value<string>();
                    string german = translation.Value.SelectToken("GER").Value<string>();

                    foreach (var item in fileContents.Properties())
                    {
                        var currentValue = item.Value.ToString();
                        bool canUpdate = CanUpdate(english, german, currentValue);

                        if (canUpdate)
                        {
                            item.Value = currentValue.Replace(english, german);
                        }
                    }
                }

                result = fileContents.ToString();
            }

            File.WriteAllText(filePath, result);
        }

        public static bool CanUpdate(string english, string german, string val)
        {
            return !String.IsNullOrEmpty(english)
                && !String.IsNullOrEmpty(german)
                && val.ToString() == english;
        }
    }
}

//var file_contents = JObject.Parse(
//    @"{
//        '0': 'language',
//        '1': 'user',
//        '2': 'save'
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
