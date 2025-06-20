﻿using Phoenix.Common.Data;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Windows;

namespace GameLauncher
{
    enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string rootPath;
        private readonly string versionFile;
        private readonly string patchZip;
        private readonly string installZip;
        private readonly string gameExe;

        private LauncherStatus _status;
        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        PlayButton.Content = "Play";
                        break;
                    case LauncherStatus.failed:
                        PlayButton.Content = "Update Failed - Retry";
                        break;
                    case LauncherStatus.downloadingGame:
                        PlayButton.Content = "Downloading Game";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        PlayButton.Content = "Downloading Update";
                        break;
                    default:
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            GameName.Content = Constants.GAME_NAME;
            rootPath = Directory.GetCurrentDirectory();
            versionFile = Path.Combine(rootPath, "Version.txt");
            patchZip = Path.Combine(rootPath, "Phoenix-Patch.Zip");
            installZip = Path.Combine(rootPath, "Phoenix-Install.zip");
            gameExe = Path.Combine(rootPath, "Phoenix.Client.exe");
        }

        private void CheckForUpdates()
        {
            if (File.Exists(versionFile))
            {
                Version localVersion = new(File.ReadAllText(versionFile));
                VersionText.Text = localVersion.ToString();

                try
                {
                    WebClient webClient = new();
                    Version onlineVersion = new(webClient.DownloadString("https://drive.google.com/uc?export=download&id=1KC9l7SKXHHhvpAzSRHnk9cUi8jY2V44m"));

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        InstallPatchFiles(true, onlineVersion);
                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                InstallGameFiles(false, Version.zero);
            }
        }

        private void InstallGameFiles(bool _isUpdate, Version _onlineVersion)
        {
            try
            {
                WebClient webClient = new();
                if (_isUpdate)
                {
                    Status = LauncherStatus.downloadingUpdate;
                }
                else
                {
                    Status = LauncherStatus.downloadingGame;
                    _onlineVersion = new Version("0.0.1");
                }
                DownloadBar.Value = 0;
                DownloadBar.Visibility = Visibility.Visible;
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    DownloadBar.Value = e.ProgressPercentage;
                };
                webClient.DownloadFileAsync(new Uri("https://www.googleapis.com/drive/v3/files/1oqpgDYDsz6tle8USrIs-2tsGxcO3-1Gm?alt=media&key=AIzaSyAAatXFnJJx0xaKbl7jeY9UpZ40K72KvuY"), installZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }
        private void InstallPatchFiles(bool _isUpdate, Version _onlineVersion)
        {
            try
            {
                WebClient webClient = new();
                if (_isUpdate)
                {
                    Status = LauncherStatus.downloadingUpdate;
                }
                else
                {
                    Status = LauncherStatus.downloadingGame;
                    _onlineVersion = new Version(webClient.DownloadString("https://drive.google.com/uc?export=download&id=1KC9l7SKXHHhvpAzSRHnk9cUi8jY2V44m"));
                }
                DownloadBar.Visibility = Visibility.Visible;
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                webClient.DownloadFileAsync(new Uri("https://www.googleapis.com/drive/v3/files/1m_ygcqkYs3aNKBgNKR60OJhH46rdVQMh?alt=media&key=AIzaSyAAatXFnJJx0xaKbl7jeY9UpZ40K72KvuY"), patchZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }


        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                DownloadBar.Visibility = Visibility.Hidden;
                string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string installPath = Path.Combine(directory, installZip);
                string patchPath = Path.Combine(directory, patchZip);

                if (File.Exists(installPath))
                {
                    string onlineVersion = ((Version)e.UserState).ToString();
                    ZipFile.ExtractToDirectory(installZip, rootPath, true);
                    File.Delete(installZip);

                    File.WriteAllText(versionFile, onlineVersion);

                    VersionText.Text = onlineVersion;
                    CheckForUpdates();
                }
                else if (File.Exists(patchPath))
                {
                    string onlineVersion = ((Version)e.UserState).ToString();
                    ZipFile.ExtractToDirectory(patchZip, rootPath, true);
                    File.Delete(patchZip);

                    File.WriteAllText(versionFile, onlineVersion);

                    VersionText.Text = onlineVersion;
                    Status = LauncherStatus.ready;
                }
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(gameExe) && Status == LauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new(gameExe);
                startInfo.WorkingDirectory = Path.Combine(rootPath);
                Process.Start(startInfo);

                Close();
            }
            else if (Status == LauncherStatus.failed)
            {
                CheckForUpdates();
            }
        }
    }

    struct Version
    {
        internal static Version zero = new(0, 0, 0);

        private readonly short major;
        private readonly short minor;
        private readonly short subMinor;

        internal Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        internal Version(string _version)
        {
            string[] versionStrings = _version.Split('.');
            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            subMinor = short.Parse(versionStrings[2]);
        }

        internal bool IsDifferentThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != _otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }
}
