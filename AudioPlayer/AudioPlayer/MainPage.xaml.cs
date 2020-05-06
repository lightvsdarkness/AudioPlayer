using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer.Services;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public List<Song> MySongs { get; }
        public List<string> MyList { get; } 
        public int MaxVolume { get; set; }

        private bool _play = false;
        private bool _repeat = false;
        private bool _rand = false;

        private readonly string _sourcePlay;
        private readonly string _sourcePause;

        private readonly string _sourceRepeat;
        private readonly string _sourceNRepeat;

        private readonly string _sourceRand;
        private readonly string _sourceNRand;

        private readonly AudioPlayerViewModel _player;

        
        public MainPage()
        {   
            InitializeComponent();

            MyList = (List<string>)DependencyService.Get<IMyFile>().GetFileLocation();
            MySongs = new List<Song>();

            _sourcePlay = "play.png";
            _sourcePause = "pause.png";

            _sourceRand = "rand.png";
            _sourceNRand = "n_rand.png";

            _sourceRepeat = "repeat.png";
            _sourceNRepeat = "n_repeat.png";

            _player = new AudioPlayerViewModel(DependencyService.Get<IAudio>());
            _player.SetLooping(_repeat);

            MaxVolume = _player.GetMaxVolume();
            Volume.Value = _player.GetVolume();

            AddSongsInMyListAsync();

            BindingContext = this;
        }

        private async void AddSongsInMyListAsync()
        {
            await Task.Run(() =>
            {
                foreach (var el in MyList)
                {
                    _player.SetDataSource(el);
                    _player.UpdateInfo();

                    MySongs.Add(new Song(_player.GetNameSong(), _player.GetArtist(), _player.GetDuration(), el));

                    //MySongs.Add(new Song("name", "artist", "14.5", el));
                }
            });
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            _player.SetVolume((int)e.NewValue);
        }

        private void MyListSongs_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Song mySong)) return;

            _player.Play(mySong.Location);

            if (_play == false)
            {
                Play.ImageSource = _sourcePause;
            }

            NameSong.Text = mySong.Name;
            Artist.Text = mySong.Author;
            Duration.Text = mySong.Duration;
        }

        private void Play_OnClicked(object sender, EventArgs e)
        {
            if (_play)
            {
                _player.Play();
                Play.ImageSource = _sourcePause;
            }
            else
            {
                _player.Stop();
                Play.ImageSource = _sourcePlay;
            }
                
            _play = !_play;
        }

        private void Rand_OnClicked(object sender, EventArgs e)
        {
            Rand.ImageSource = _rand ? _sourceRand : _sourceNRand;
            
            _rand = !_rand;
        }

        private void Repeat_OnClicked(object sender, EventArgs e)
        {
            Repeat.ImageSource = _repeat ? _sourceRepeat : _sourceNRepeat;
            _player.SetLooping(_repeat);
            
            _repeat = !_repeat;
        }
    }
}
