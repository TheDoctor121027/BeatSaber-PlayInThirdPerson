﻿using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace PlayInThirdPerson
{
    public static class ConfigHelper
    {
        public static Config Config { get; private set; }

        private const string ConfigFolder = "UserData";
        private const string ConfigFileName = "PlayInThirdPersonConfig.json";

        private static string ConfigFolderPath => Path.Combine(Environment.CurrentDirectory, ConfigFolder);
        private static string ConfigFilePath => Path.Combine(ConfigFolderPath, ConfigFileName);

        public static void LoadConfig()
        {
            Directory.CreateDirectory(ConfigFolderPath);

            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("[PlayInThirdPerson] Creating Default Config");
                Config = new Config();
            }
            else
            {
                Console.WriteLine("[PlayInThirdPerson] Loading Config");
                string data = File.ReadAllText(ConfigFilePath);
                Config = JsonConvert.DeserializeObject<Config>(data);
            }

            SaveConfig();
        }

        public static void SaveConfig()
        {
            Console.WriteLine("[PlayInThirdPerson] Saving Config");
            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);

            // Notify all CameraMover instances about the config update
            CameraMover[] cameraMovers = UnityEngine.Object.FindObjectsOfType<CameraMover>();
            foreach (var mover in cameraMovers)
            {
                mover.UpdateCameraPosition();
            }
        }
    }
}
