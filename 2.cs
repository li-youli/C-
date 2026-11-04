using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace AsyncExperiment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string[] urls = {
                "https://www.nasa.gov/",
                "https://dotnet.microsoft.com/zh-tw/download",
                "https://www.tcu.edu.tw/p/404-1033-65840.php"
            };

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("First Question\n");

                Console.WriteLine("--- Method 1A: GetStringAsync (WITHOUT await) ---");
                FetchWithoutAwait(client, urls);

                Console.WriteLine("\n--- Method 1B: GetStringAsync (WITH await) ---");
                await FetchWithAwait(client, urls);

                Console.WriteLine("\n--- Method 1C: Send Request (Synchronous) ---");
                FetchSynchronousSend(client, urls);


                Console.WriteLine("\n Second Question \n");

                Console.WriteLine("--- Method 2A: Task.WhenAll ---");
                await FetchTaskWhenAll(client, urls);

                Console.WriteLine("\n--- Method 2B: Task.WhenAny ---");
                await FetchTaskWhenAny(client, urls);
            }
        }
         
        static string GetSnippet(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "Empty response";
            }
             
            string cleanText = text.Replace("\r", "").Replace("\n", " ");
             
            if (cleanText.Length <= 100)
            {
                return cleanText;
            }
            else
            {
                return cleanText.Substring(0, 100) + "...";
            }
        }

        // First Question

        static void FetchWithoutAwait(HttpClient client, string[] urls)
        {
            foreach (var url in urls)
            {
                Task<string> task = client.GetStringAsync(url);
                string result = task.Result;
                Console.WriteLine($"[No Await] Fetched from {url}:\n -> \"{GetSnippet(result)}\"");
            }
        }

        static async Task FetchWithAwait(HttpClient client, string[] urls)
        {
            foreach (var url in urls)
            {
                string result = await client.GetStringAsync(url);
                Console.WriteLine($"[With Await] Fetched from {url}:\n -> \"{GetSnippet(result)}\"");
            }
        }

        static void FetchSynchronousSend(HttpClient client, string[] urls)
        {
            foreach (var url in urls)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpResponseMessage response = client.Send(request);

                using (Stream stream = response.Content.ReadAsStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine($"[Sync Send] Fetched from {url}:\n -> \"{GetSnippet(result)}\"");
                }
            }
        }

        // Second Question

        static async Task FetchTaskWhenAll(HttpClient client, string[] urls)
        {
            Task<string>[] tasks = new Task<string>[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                tasks[i] = client.GetStringAsync(urls[i]);
            }

            string[] results = await Task.WhenAll(tasks);

            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"[WhenAll] Task {i + 1} finished:\n -> \"{GetSnippet(results[i])}\"");
            }
        }

        static async Task FetchTaskWhenAny(HttpClient client, string[] urls)
        {
            Task<string>[] tasks = new Task<string>[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                tasks[i] = client.GetStringAsync(urls[i]);
            }

            Task<string> firstFinishedTask = await Task.WhenAny(tasks);
            string result = await firstFinishedTask;

            Console.WriteLine($"[WhenAny] The fastest website responded with:\n -> \"{GetSnippet(result)}\"");
        }
    }
}