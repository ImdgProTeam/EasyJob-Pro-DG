using System;
using System.Windows;

namespace EasyJob_ProDG.UI.Classes
{
    internal static class Output1
    {
        internal static void ProgramInitiatedView()
        {
            Style1.InitialFormatConsole();
        }

        internal static void DisplayLine(string text)
        {
            MessageBox.Show(text);
            
        }

        internal static void DisplayLine(int number)
        {
            MessageBox.Show(number.ToString());
        }

        internal static void DisplayLine(string text, string param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }
        internal static void DisplayLine(string text, int param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }

        internal static void DisplayLine(string text, object[] param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }

        internal static void DisplayLine(string text, int i, string str)
        {
            string msg = String.Format(text, i, str);
            MessageBox.Show(msg);
        }

        internal static void Display(string text)
        {
            MessageBox.Show(text);
        }

        internal static void Display(int number)
        {
            MessageBox.Show(number.ToString());
        }

        internal static void Display(string text, string param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }
        internal static void Display(string text, int param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }

        internal static void Display(string text, object[] param)
        {
            string msg = String.Format(text, param);
            MessageBox.Show(msg);
        }

        internal static void Display(string text, int i, string str)
        {
            string msg = String.Format(text, i, str);
            MessageBox.Show(msg);
        }

    }
}
