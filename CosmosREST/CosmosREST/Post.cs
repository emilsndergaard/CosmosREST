using System.ComponentModel;
using System.Runtime.CompilerServices;
using CosmosREST.Annotations;
using Newtonsoft.Json;

namespace CosmosREST
{
    internal class Post : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _title;

        [JsonProperty("title")]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}