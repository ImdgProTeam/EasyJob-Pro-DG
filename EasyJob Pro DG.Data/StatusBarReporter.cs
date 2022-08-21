using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_ProDG.Data
{
    public static class StatusBarReporter
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
