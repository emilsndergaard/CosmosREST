using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CosmosREST
{
    public partial class MainPage : ContentPage
    {
        private const string Url = "https://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient _client = new HttpClient();
        private ObservableCollection<Post> _posts;


        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            string content = await _client.GetStringAsync(Url);
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(content);
            _posts = new ObservableCollection<Post>(posts);
            MyListView.ItemsSource = _posts;
            base.OnAppearing();
        }


        private async void OnAdd(object sender, EventArgs e)
        {
            Post post = new Post { Title = $"Title: Timestamp {DateTime.UtcNow.Ticks}" }; //Creating a new instane of Post with a Title Property and its value in a Timestamp format
            string content = JsonConvert.SerializeObject(post); //Serializes or convert the created Post into a JSON String
            await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json")); //Send a POST request to the specified Uri as an asynchronous operation and with correct character encoding (utf9) and contenct type (application/json).
            _posts.Insert(0, post); //Updating the UI by inserting an element into the first index of the collection 
        }
        private async void OnUpdate(object sender, EventArgs e)
        {
            Post post = _posts[0]; //Assigning the first Post object of the Post Collection to a new instance of Post
            post.Title += " [updated]"; //Appending an [updated] string to the current value of the Title property
            string content = JsonConvert.SerializeObject(post); //Serializes or convert the created Post into a JSON String
            await _client.PutAsync(Url + "/" + post.Id, new StringContent(content, Encoding.UTF8, "application/json")); //Send a PUT request to the specified Uri as an asynchronous operation.
        }
        /// <summary>
        /// Click event of the Delete Button. It sends a DELETE request removing the first Post object in the server and in the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDelete(object sender, EventArgs e)
        {
            Post post = _posts[0]; //Assigning the first Post object of the Post Collection to a new instance of Post
            await _client.DeleteAsync(Url + "/" + post.Id); //Send a DELETE request to the specified Uri as an asynchronous 
            _posts.Remove(post); //Removes the first occurrence of a specific object from the Post collection. This will be visible on the UI instantly
        }
    }
}
