﻿using EasyJob_ProDG.UI.Utility;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace EasyJob_ProDG.UI.ViewModel
{
    /// <summary>
    /// Logic for progress bar status visual and text
    /// </summary>
    public class StatusBarViewModel : Observable
    {
        private static int _dataReporterValue => EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage;

        private static StatusBarViewModel _staticReportViewModelInstance = null;
        private BackgroundWorker _worker;

        private int _Max = 100;
        private int _setValue;
        private bool IsInProgress;

        /// <summary>
        /// Delay in ms used for increment
        /// </summary>
        private const int delay = 25;

        private int _progressPercentage;
        public int ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                if (value <= _Max)
                    _progressPercentage = value;
                else _Max = value;
                OnPropertyChanged();
            }
        }

        private Visibility _progressBarVisibile;
        public Visibility ProgressBarVisible
        {
            get { return _progressBarVisibile; }
            private set
            {
                _progressBarVisibile = value;
                OnPropertyChanged();
            }
        }

        private string _statusBarText;
        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                _statusBarText = value;
                OnPropertyChanged();
            }
        }

        public StatusBarViewModel()
        {
            if(_staticReportViewModelInstance == null)
                _staticReportViewModelInstance = this;
            ProgressBarVisible = Visibility.Collapsed;
            ProgressPercentage = 0;
            StatusBarText = "";

            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentageChanged += OnStaticReportValueChanged;
        }

        private void Increment()
        {
            if (IsInProgress) return;
            if (ProgressPercentage == _Max) return;
            ProgressPercentage++;
        }

        private void IncrementProgressBackgroundWorker()
        {
            if (IsInProgress) return;

            Reset();
            IsInProgress = true;

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            _worker.WorkerSupportsCancellation = true;

            _worker.RunWorkerAsync();
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Completed();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (ProgressPercentage < _Max)
            {
                if (ProgressPercentage < _setValue)
                    ProgressPercentage++;
                (sender as BackgroundWorker).ReportProgress(ProgressPercentage);
                Thread.Sleep(delay);
            }
        }

        private void Reset()
        {
            ProgressPercentage = 0;
        }

        internal void StartProgressBar(int barSet)
        {
            ProgressBarVisible = Visibility.Visible;
            _setValue = barSet;
            IncrementProgressBackgroundWorker();
        }

        internal void StartProgressBar(int barSet, string text)
        {
            SetBarText(text);
            StartProgressBar(barSet);
        }

        internal void Completed()
        {
            IsInProgress = false;
            SetBarText(string.Empty);
            ProgressBarVisible = Visibility.Collapsed;
        }

        internal void ChangeBarSet(int newBarSet)
        {
            _setValue = newBarSet;
        }

        internal void SetBarText(string text)
        {
            StatusBarText = text;
        }

        internal void Cancel()
        {
            _worker?.CancelAsync();
            Completed();
        }

        /// <summary>
        /// Sets ProgressBar Percentage on StaticReportValue changed.
        /// </summary>
        private static void OnStaticReportValueChanged()
        {
            _staticReportViewModelInstance.ProgressPercentage = _dataReporterValue;
        }
    }
}
