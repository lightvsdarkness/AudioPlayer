using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.View;
using Android.Views;
using Android.Widget;
using AudioPlayer.Droid.Services;
using AudioPlayer.Services;
using Java.Lang;
using Java.Text;
using Xamarin.Forms;
using static Xamarin.Forms.Forms;
using Context = Android.Content.Context;
using Image = Xamarin.Forms.Image;
using Stream = Android.Media.Stream;
using String = Java.Lang.String;

[assembly: Xamarin.Forms.Dependency(typeof(AudioService))]
namespace AudioPlayer.Droid.Services
{
    internal class AudioService : IAudio

    {
        private MediaPlayer _mediaPlayer;
        private readonly MediaMetadataRetriever _reader;

        private readonly AudioManager _audioManager;
        private int _maxVolume;

        private InfoMP3 _infoMp3;

        public AudioService()
        {
            _reader = new MediaMetadataRetriever();


            if (Forms.Context != null)
                _audioManager = (AudioManager) Forms.Context.GetSystemService(Context.AudioService);
            _maxVolume = _audioManager.GetStreamMaxVolume(Stream.Music);
        }


        public void SetDataSource(string filePath)
        {
            _reader.SetDataSource(filePath);

            if (_reader != null)
            {
                _infoMp3.Artist = _reader.ExtractMetadata(MetadataKey.Artist)
                                  ?? _reader.ExtractMetadata(MetadataKey.Author)
                                  ?? _reader.ExtractMetadata(MetadataKey.Composer)
                                  ?? "unknown artist";


                var lastFilePath = filePath.LastIndexOf("/", StringComparison.Ordinal);
                var name = filePath.Remove(0, lastFilePath + 1).Replace(".mp3", "");

                _infoMp3.NameSong = _reader.ExtractMetadata(MetadataKey.Title)
                                    ?? name;

                long dur = Long.ParseLong(_reader.ExtractMetadata(MetadataKey.Duration));
                var seconds = String.ValueOf((dur % 60000) / 1000);
                var minutes = String.ValueOf((dur / 60000));
                _infoMp3.Duration = minutes + ":" + seconds;
            }
        }

        public void PlayAudioFile(string filePath)
        {
            SetDataSource(filePath);

            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Prepared += (s, e) => { _mediaPlayer.Start(); };
            }
            _mediaPlayer.Reset();
            _mediaPlayer.SetVolume(1.0f, 1.0f);

            _mediaPlayer.SetDataSource(filePath);
            _mediaPlayer.PrepareAsync();
        }

        public void Play()
        {
            _mediaPlayer?.Start();
        }

        public void Stop()
        {
            _mediaPlayer?.Pause();
        }

        public void SetLooping(bool loop)
        {
            if (_mediaPlayer == null) return;
            _mediaPlayer.Looping = loop;
        }

        public InfoMP3 GetInfo()
        {
            return _infoMp3;
        }

        // todo: Don't work. Rewrite.
        public void SetVolume(int volume)
        {
            if( volume <= _maxVolume)
                _audioManager.SetStreamVolume(Stream.Music, volume, 0);

            //_audioManager.GetStreamVolume(Stream.Music);

            //_mediaPlayer.SetVolume(leftVolume, rightVolume);
        }



        public int GetMaxVolume()
        {
            return _maxVolume;
        }

        public int GetVolume()
        {
            return _audioManager.GetStreamVolume(Stream.Music);
        }
    }
}