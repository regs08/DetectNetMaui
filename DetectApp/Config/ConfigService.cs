using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleGson;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DetectApp
{
    public class ConfigService
    {
        public async Task<ServerConfig> LoadConfig(string config)
        {
            // for now just rerading inthe json file
            return JsonSerializer.Deserialize<ServerConfig>(config);

            //string configFileName = $"{configName}.json";
            //var stream = await FileSystem.OpenAppPackageFileAsync(configFileName);

            /*if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonSerializer.Deserialize<ServerConfig>(json);
                }
            }

            // Return a default config if the file doesn't exist
            //Console.WriteLine($"Loading default config for {configName}");
           // return new ServerConfig();
            */
        }

        public async Task<List<string>> AvailableConfigs()
        {
            List<string> availableConfigs = new List<string>();
            var stream = await FileSystem.OpenAppPackageFileAsync("DefaultConfig.json");
            using (var reader = new StreamReader(stream))
            {
                var jsonContent = await reader.ReadToEndAsync();
                availableConfigs.Add(jsonContent);
                return availableConfigs;

            }
        }
    }
}
