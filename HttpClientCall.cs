using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DesktopApp.Model;

namespace DesktopApp
{
	public class HttpClientCall
	{
		private readonly string url;
		private readonly HttpClient client;
		public string token { get; set; }
		public int lastHash { get; set; }

        public HttpClientCall(string url)
        {
			this.url = url;
			this.client =  new HttpClient();
		}
		
		public  async Task<string> Login(string userName, string password)
		{
			var loginModel = new LoginModel { username = userName, password = password };
			var json = JsonSerializer.Serialize(loginModel);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var endpoint = url+"/Auth/login";

			var response = await client.PostAsync(endpoint, data);
			//var result = await response.Content.ReadAsStringAsync();
			var loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(response.Content.ReadAsStream());
			return loginResponse.token;
		}
		public async Task<bool> IsNewExist()
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", token);
			var endpoint = url+ "/File/CheckNewFileExist?id="+ lastHash;

			var stringTask = await client.GetAsync(endpoint);
			var resp = await stringTask.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<bool>(resp);

		}
		public async Task<FileModel> DownloadFile()
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", token);
			var endpoint = url + "/File/GetNewFileExist";

			var stringTask = await client.GetAsync(endpoint);
			var resp = await stringTask.Content.ReadAsStringAsync();
			var fileModel = JsonSerializer.Deserialize<FileModel>(resp, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
			lastHash = fileModel.FileHash;
			var filepath = Path.Combine(Directory.GetCurrentDirectory(), fileModel.FileName);


			// Create the file, or overwrite if the file exists.
			using (FileStream fs = File.Create(filepath))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(fileModel.FileContent);
				fs.Write(info, 0, info.Length);
			}		
			
			return fileModel;

		}
	}
}

