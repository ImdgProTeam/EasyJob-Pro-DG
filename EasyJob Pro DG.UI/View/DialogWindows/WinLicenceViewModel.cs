﻿using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Services.DialogServices;
using System;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinLicenceViewModel : IDialogWindowRequestClose
    {
        public static DateTime LicenceValidity => Licence.EndLicence;

        public WinLicenceViewModel() 
        {

        }

        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;
    }
}