using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AudioTracksDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaPlaybackItem playbackItem;
        public MainPage()
        {
            this.InitializeComponent();
        }
         async private void Open_File(object sender, RoutedEventArgs e)
        {
            try { await PickSingleVideoFile(); } catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
            
        }
        async private System.Threading.Tasks.Task PickSingleVideoFile()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".mkv");

            var file = await openPicker.PickSingleFileAsync();
            //Debug.WriteLine(file);
            // mediaPlayer is a MediaPlayerElement defined in XAML
            if (file != null)
            {
                var source = MediaSource.CreateFromStorageFile(file);
                playbackItem = new MediaPlaybackItem(source);

                playbackItem.AudioTracks.SelectedIndexChanged += AudioTracks_SelectedIndexChanged;
                mediaplayerElement.Source = playbackItem;
           
            }
        }

        private void AudioTracks_SelectedIndexChanged(ISingleSelectMediaTrackList sender, object args)
        {
          

                Debug.WriteLine("AudioTracks_SelectedIndexChanged");
            var audioTrackIndex = sender.SelectedIndex;
            Debug.WriteLine($"index : {audioTrackIndex} ");
            if (playbackItem != null && playbackItem.AudioTracks != null)
            {
                playbackItem.AudioTracks.SelectedIndex = audioTrackIndex;
                Debug.WriteLine($"index set to : {playbackItem.AudioTracks.SelectedIndex} ");
            }
            
        }

        

       
    }
}
