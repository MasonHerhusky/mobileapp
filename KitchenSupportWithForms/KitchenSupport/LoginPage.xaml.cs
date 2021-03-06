﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KitchenSupport
{
	public partial class LoginPage : ContentPage //, INotifyPropertyChanged
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

		async private void login(object sender, EventArgs args){
			var client = new System.Net.Http.HttpClient ();
			string url = "http://api.kitchen.support/accounts/login";
			string data = "{\n    \"email\" : \"" + email.Text + "\",\n    \"password\" : \"" + password.Text + "\"\n}";
			var httpContent = new StringContent (data);
			httpContent.Headers.ContentType = new MediaTypeHeaderValue ("application/json");
			var response = client.PostAsync(new Uri(url), httpContent);

			if (response.Result.StatusCode.ToString() == "OK") {
				var message = response.Result.Content.ReadAsStringAsync().Result;
				var json = JObject.Parse (message);
				var token = json ["api_token"];
				//await storeToken(token.ToString());
				await Navigation.PushModalAsync (new NavigationPage(new HomePage()));
			}
			else {
				await DisplayAlert ("Alert", "Invalid Username or Password.","OK");
			}
		}

		public async Task storeToken(string token)
		{
			//Writes a New Token upon authentication in the directory
			DependencyService.Get<ISaveAndLoad>().SaveTextAsync("token.txt", token);
			App.StoredToken = DependencyService.Get<ISaveAndLoad> ().LoadTextAsync ("token.txt").Result;
		}

		public interface ISaveAndLoad {
			//Needed to pull and save tokens
			Task SaveTextAsync (string filename, string text);
			Task<string> LoadTextAsync (string filename);
		}

		private void register(object sender, EventArgs args){
			Navigation.PushModalAsync (new NavigationPage(new RegisterPage()));
		}

		private void forgotPassword(object sender, EventArgs args){
			Navigation.PushModalAsync (new NavigationPage(new ResetPage()));

		}
	}
}

