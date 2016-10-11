using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Extensions.Compression.Client;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Headers;
using System.Diagnostics;
using Polly;
using System;
using System.IO;
using Xamarin.Forms;

namespace PhillyBlotter.Models
{
	public class JsonWebClient
	{
		public async Task<System.IO.TextReader> DoRequestAsync(HttpResponseMessage req)
		{
			var stream = await req.Content.ReadAsStreamAsync();
			var sr = new System.IO.StreamReader(stream);
			return sr;
		}
		
		public async Task<System.IO.TextReader> DoRequestAsync(WebRequest req)
		{
			var reader = await Policy
			.Handle<Exception>()
			.WaitAndRetryAsync
			(
				retryCount: 5,
				sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
				onRetry: (Exception arg1, TimeSpan arg2, Context arg3) =>
				{
					// If this explodes it could be due to a DNS change.  We shall clear the DNS
					// cache entry and set it back to the default and clear the DNS cache.
					Data.ClearDNS();
					// Write why we're getting an exception
					Debug.WriteLine($"Error transmitting to server {arg1.Message}\r\n{arg1.StackTrace}");
				}
			)
			.ExecuteAsync(async () =>
			{
				var task = Task.Factory.FromAsync((cb, o) => ((HttpWebRequest)o).BeginGetResponse(cb, o), res =>
				((HttpWebRequest)res.AsyncState).EndGetResponse(res), req);
				var result = await task;
				var resp = result;
				var stream = resp.GetResponseStream();
				var sr = new System.IO.StreamReader(stream);
				return sr;
			});

			return reader;

		}

		public async Task<System.IO.TextReader> DoRequestAsync(string url)
		{
			var resp = await Policy
				.Handle<Exception>()
				.WaitAndRetryAsync
				(
					retryCount: 5,
					sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					onRetry: (Exception arg1, TimeSpan arg2, Context arg3) =>
					{
						// If this explodes it could be due to a DNS change.  We shall clear the DNS
						// cache entry and set it back to the default and clear the DNS cache.
						Data.ClearDNS();
						// Write why we're getting an exception
						Debug.WriteLine($"Error transmitting to server {arg1.Message}\r\n{arg1.StackTrace}");
					}
				)
				.ExecuteAsync(async () => {
					var client = new HttpClient(new ClientCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
					client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
					client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

					client.DefaultRequestHeaders.Add("Accept", "application/json");

					HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
					Debug.WriteLine($"Attempting to contact {url}");
					return await client.SendAsync(message);
				});
			
			var tr = await DoRequestAsync(resp);
			return tr;
		}

		// Transmits data but we don't care what happens next
		public async Task<bool> DoSilentPost(string url, string data)
		{
			await Policy
			.Handle<Exception>()
			.WaitAndRetryAsync
			(
				retryCount: 5,
				sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
				onRetry: (Exception arg1, TimeSpan arg2, Context arg3) =>
				{
					// If this explodes it could be due to a DNS change.  We shall clear the DNS
					// cache entry and set it back to the default and clear the DNS cache.
					Data.ClearDNS();
					// Write why we're getting an exception
					Debug.WriteLine($"Error posting to server {arg1.Message}\r\n{arg1.StackTrace}");
				}
			)
			.ExecuteAsync(async () =>
			{
				var client = new HttpClient(new ClientCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
				client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
				client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

				Debug.WriteLine("Posting to server...");
				await client.PostAsync(url, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
				Debug.WriteLine("Post complete.");
			});

			return true;
		}

		public async Task<T> DoRequestJsonAsync<T>(WebRequest req)
		{
			var ret = await DoRequestAsync(req);
			var response = await ret.ReadToEndAsync();
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
		}

		public async Task<T> DoRequestJsonAsync<T>(string uri)
		{
			var ret = await DoRequestAsync(uri);
			var response = await ret.ReadToEndAsync();
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
		}
	}
}