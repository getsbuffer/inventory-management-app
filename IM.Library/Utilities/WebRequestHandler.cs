﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IM.Library.Utilities
{
    public class WebRequestHandler
    {
        private string host = "localhost";
        private string port = "5100";
        private HttpClient Client { get; }

        public WebRequestHandler()
        {
            Client = new HttpClient();
        }

        public async Task<string> Get(string url)
        {
            var fullUrl = $"http://{host}:{port}{url}";
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(fullUrl).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public async Task<string> Delete(string url)
        {
            var fullUrl = $"http://{host}:{port}{url}";
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Delete, fullUrl))
                    {
                        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                            return "ERROR";
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public async Task<string> Post(string url, object obj)
        {
            var fullUrl = $"http://{host}:{port}{url}";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, fullUrl))
                {
                    var json = JsonConvert.SerializeObject(obj);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                            return "ERROR";
                        }
                    }
                }
            }
        }

        public async Task<string> Put(string url, object obj)
        {
            var fullUrl = $"http://{host}:{port}{url}";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Put, fullUrl))
                {
                    var json = JsonConvert.SerializeObject(obj);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                            return "ERROR";
                        }
                    }
                }
            }
        }
    }
}
