using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace DetectApp
{
    public class VideoStreamClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _videoStreamUrl;

        public VideoStreamClient(Server server)
        {
            _httpClient = new HttpClient();
            _videoStreamUrl = server.Config.VideoStreamUrl;
        }

        public async Task ConnectAsync()
        {
            try
            {
                // Send a GET request to the video stream URL
                HttpResponseMessage response = await _httpClient.GetAsync(_videoStreamUrl, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Connected to video stream successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to connect to video stream.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

        public async Task<Stream> GetVideoStreamAsync()
        {
            try
            {
                // Send a GET request to retrieve the video stream
                HttpResponseMessage response = await _httpClient.GetAsync(_videoStreamUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();  // Throw an exception if the response indicates an error

                // Return the content stream for the video
                return await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
        }
    }
}
