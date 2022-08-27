using System;

namespace EasyJob_ProDG.Data
{
    /// <summary>
    /// The reporter is used as an assistant to tranmit the progress
    /// to ProgressBar (or any other subscriber) in UI.
    /// </summary>
    public static class ProgressBarReporter
    {
        private static int reportPercentage;

        public static int ReportPercentage
        {
            get { return reportPercentage; }
            set
            {
                reportPercentage = value;
                ReportPercentageChanged?.Invoke();
            }
        }

        public static event Action ReportPercentageChanged;
    }
}
