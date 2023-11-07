using EasyJob_ProDG.UI.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;

namespace EasyJob_ProDG.UI.Settings
{
    /// <summary>
    /// Handles saving and restoring of user settings from settings.settings to/from .xml file.
    /// </summary>
    public class UserSettingsFileHandler
    {
        /// <summary>
        /// List of properties that will be ignorred when generating xml file
        /// </summary>
        private static List<string> exceptedProperties = new()
        {
            "WindowPosition", "WindowStateMaximized", "FirstTimeStart", "WorkingCargoPlanFile"
        };

        /// <summary>
        /// Calls DialogSaveFile to save the settings.xml
        /// </summary>
        internal static void SaveSettings()
        {
            XDocument xDocument;
            if (!GenerateXDocumentFromSettings(out xDocument))
                return;

            string fileName;
            if (DialogSaveSettings.SaveSettingsToFileWithDialog(out fileName))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(fileName))
                {
                    xDocument.WriteTo(xmlWriter);
                }
            }
        }

        /// <summary>
        /// Calls DialogOpenFile to restore the settings from settings.xml file
        /// </summary>
        internal static bool RestoreSettings()
        {
            if (!DialogOpenSettings.OpenSettingsFileWithDialog(out var file))
            {
                return false;
            }
            XDocument xDocument = XDocument.Load(file);
            return RestoreSettingsFromXDocument(xDocument);
        }

        /// <summary>
        /// Generates xml Document containing properties from settings.settings, 
        /// except properties listed in <see cref="exceptedProperties"/>
        /// </summary>
        /// <returns>True if succesfully generated</returns>
        private static bool GenerateXDocumentFromSettings(out XDocument xDocument)
        {
            try
            {
                xDocument = new XDocument();
                xDocument.Add(new XComment("EasyJob ProDG properties"));
                xDocument.Add(new XElement("Properties"));
                xDocument.Root.Add(new XElement("version", "1.0"));
                foreach (SettingsProperty property in Properties.Settings.Default.Properties)
                {
                    //exclude
                    if (exceptedProperties.Contains(property.Name))
                        continue;

                    XElement xElement = new XElement(property.Name, Properties.Settings.Default[property.Name]);
                    xDocument.Root.Add(xElement);
                }
            }
            catch (Exception e)
            {
                EasyJob_ProDG.Data.LogWriter.Write($"Thrown an exception {e.Message} when generating settings.xml");
                xDocument = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates settings.settings from settings.xml file
        /// </summary>
        /// <param name="xDocument">settings.xml file</param>
        /// <returns>True if succesfully read</returns>
        private static bool RestoreSettingsFromXDocument(XDocument xDocument)
        {
            try
            {
                foreach (XElement element in xDocument.Root.Elements())
                {
                    try
                    {
                        if (element.Name.ToString().Equals("version")) continue;

                        var propertyType = Properties.Settings.Default.Properties[element.Name.ToString()].PropertyType;

                        if (propertyType.Equals(typeof(string)))
                            Properties.Settings.Default[element.Name.ToString()] = element.Value;
                        else if(propertyType.Equals(typeof(byte)))
                            Properties.Settings.Default[element.Name.ToString()] = byte.Parse(element.Value);
                        else if(propertyType.Equals(typeof(double)))
                            Properties.Settings.Default[element.Name.ToString()] = double.Parse(element.Value);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                EasyJob_ProDG.Data.LogWriter.Write($"Thrown an exception {e.Message} when reading settings.xml");
                return false;
            }
            Properties.Settings.Default.Save();
            return true;
        }
    }
}
