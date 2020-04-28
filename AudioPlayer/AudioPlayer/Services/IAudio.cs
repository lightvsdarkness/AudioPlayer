using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AudioPlayer.Services
{
    public struct InfoMP3
    {
        public string Artist;
        public string NameSong;
        public string Duration;
    }

    public interface IAudio
    {
        InfoMP3 GetInfo();
        void PlayAudioFile(string filePath);
        void Play();
        void Stop();
        void SetLooping(bool loop);
        void SetVolume(int volume);
        int GetMaxVolume();
        int GetVolume();
        void SetDataSource(string filePath);
    }
}
