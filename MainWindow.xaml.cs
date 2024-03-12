
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;


namespace practice_3
{
    public partial class MainWindow : Window
    {
        private string[] audioFiles;
        private int currentTrackIndex = 0;
        private bool isRepeating = false;
        private bool isShuffle = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                audioFiles = Directory.GetFiles(dialog.SelectedPath, "*.mp3");
                Array.Sort(audioFiles);
                if (audioFiles.Length > 0)
                {
                    PlayTrack(audioFiles[currentTrackIndex]);
                }
            }
        }

        private void PlayTrack(string filePath)
        {
            Video.Source = new Uri(filePath);
            Video.Play();
        }

        private void PlayPause_Click(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Video.CanPause)
            {
                if (Video.IsEnabled)
                {
                    Video.Pause();
                }
                else
                {
                    Video.Play();
                }
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrackIndex > 0)
            {
                currentTrackIndex--;
                PlayTrack(audioFiles[currentTrackIndex]);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrackIndex < audioFiles.Length - 1)
            {
                currentTrackIndex++;
                PlayTrack(audioFiles[currentTrackIndex]);
            }
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            isRepeating = !isRepeating;
            if (isRepeating)
            {
                isShuffle = false; // отключаем случайное воспроизведение при включении повтора
            }
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            isShuffle = !isShuffle;
            if (isShuffle)
            {
                var rnd = new Random();
                audioFiles = audioFiles.OrderBy(x => rnd.Next()).ToArray();
                currentTrackIndex = 0;
                PlayTrack(audioFiles[currentTrackIndex]);
                isRepeating = false; // отключаем повтор при включении случайного воспроизведения
            }
            else
            {
                Array.Sort(audioFiles);
                currentTrackIndex = 0;
                PlayTrack(audioFiles[currentTrackIndex]);
            }
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Video.NaturalDuration.HasTimeSpan)
            {
                TimeSpan time = TimeSpan.FromSeconds(positionSlider.Value * Video.NaturalDuration.TimeSpan.TotalSeconds / 100);
                Video.Position = time;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Video.Volume = volumeSlider.Value;
        }

    }
}