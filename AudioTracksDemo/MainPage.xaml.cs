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
using Windows.UI.Core;
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
        int audioTrackCount = -1;
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

        private void createAudioTrackMenu()
        {
            Debug.WriteLine($"creating AudioTrackMenu ");
            if (audioTrackCount > 1)
            {

                for (int i = 0; i < audioTrackCount; i++)
                {
                    Debug.WriteLine($"{playbackItem.AudioTracks[i].SupportInfo.DecoderStatus} {playbackItem.AudioTracks[i].Name}");
                    ToggleMenuFlyoutItem item = new ToggleMenuFlyoutItem();
                    item.Text = $"Audio Track {i+1} ({playbackItem.AudioTracks[i].Language}) ";
                    item.Tag = i;
                    item.Click += Item_Click;
                    //if (i == 0) item.IsChecked = true;
                    audioTrack.Items.Add(item);
                   
                }
                Debug.WriteLine($"created AudioTrackMenu ");
            }
            else
            {
                Debug.WriteLine($"Cant create audioTrackMenu, ats not loaded  ");
                menuBarItem.Items.RemoveAt(1);
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Item Click Event");
            int index = (int)(sender as ToggleMenuFlyoutItem).Tag;
            
            for (int i = 0; i < audioTrack.Items.Count; i++)
            {
                ((ToggleMenuFlyoutItem)(audioTrack.Items[i])).IsChecked = false;
                Debug.WriteLine($"{i} is set to false");
                // (((ToggleMenuFlyoutItem)menuBarItem.Items[i])).IsChecked = false; 
            }
            
            (sender as ToggleMenuFlyoutItem).IsChecked = true;
            
            changeAudioTrack(index);
        }
        private void changeAudioTrack(int index)
        {
            Debug.WriteLine($"changing videoTrack ");
            if (playbackItem != null && playbackItem.AudioTracks != null)
            {
                playbackItem.AudioTracks.SelectedIndex = index;

                Debug.WriteLine($"index set to : {index} ");

            }
        }
      async private void AudioTracks_SelectedIndexChanged(ISingleSelectMediaTrackList sender, object args)
        {
          

                Debug.WriteLine("AudioTracks_SelectedIndexChanged");
            var audioTrackIndex = sender.SelectedIndex;
            Debug.WriteLine($"index : {audioTrackIndex} ");
            audioTrackCount = playbackItem.AudioTracks.Count;
            Debug.WriteLine($"AudioTrackCount : {audioTrackCount} ");
            Debug.WriteLine($"label {audioTrackIndex} : { playbackItem.AudioTracks[audioTrackIndex].Label}");
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //Make sure to call this method on the UI thread:
                createAudioTrackMenu();
                
                
            });
           
            
        }

       
       
    }
}
