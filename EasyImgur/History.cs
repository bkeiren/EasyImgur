using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EasyImgur
{
    static class History
    {
        public delegate void HistoryItemAddedEventHandler(HistoryItem item);
        public delegate void HistoryItemRemovedEventHandler(HistoryItem item);
        public static event HistoryItemAddedEventHandler HistoryItemAdded;
        public static event HistoryItemRemovedEventHandler HistoryItemRemoved;

        private static BindingSource _historyBinding;

        private static string SaveLocation 
        { 
            get 
            {
                // In non-portable mode we want to save in AppData, otherwise the local folder.
                if (!Program.InPortableMode)
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur\\";
                else
                    return AppDomain.CurrentDomain.BaseDirectory;
            } 
        }

        public static int Count
        {
            get { return _historyBinding.Count; }
        }

        public static int AnonymousCount
        {
            get
            {
                return _historyBinding.Cast<HistoryItem>().Count(item => item.Anonymous);
            }
        }

        static History()
        {
            // add empty event handlers to avoid checking if the event is null
            HistoryItemAdded += v => { };
            HistoryItemRemoved += v => { };
        }

        public static int AccountCount
        {
            get { return Count - AnonymousCount; }
        }

        // required to be the first method called
        public static void BindData(BindingSource source)
        {
            _historyBinding = source;
        }

        public static void InitializeFromDisk()
        {
            List<HistoryItem> history = GetHistoryFromDisk();
            if (history == null) return;
            foreach (HistoryItem item in history)
            {
                _historyBinding.Add(item);
                HistoryItemAdded(item);
            }
        }

        private static List<HistoryItem> GetHistoryFromDisk()
        {
            string jsonString;
            try
            {
                jsonString = FileHelper.GZipReadFile(SaveLocation + "history");
            }
            catch (FileNotFoundException ex)
            {
                Log.Info("Couldn't find a history file: \n" + ex.Message);
                return null;
            }
            catch (IOException ex)
            {
                Log.Error("An I/O error occurred while opening the history file: \n" + ex);
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error("Not authorized to open the history file: \n" + ex);
                return null;
            }
            catch (SecurityException ex)
            {
                Log.Error("A security exception occurred while trying to open the history file: " + ex);
                return null;
            }

            if (string.IsNullOrEmpty(jsonString))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<HistoryItem>>(jsonString, new ImageConverter());
        }

        public static void StoreHistoryItem(HistoryItem item)
        {
            if (item == null)
            {
                Log.Warning("NULL object passed to History.StoreHistoryItem. No item stored.");
                return;
            }
            if(_historyBinding.Contains(item))
            {
                Log.Warning("Object already in history passed to History.StoreHistoryItem. No item stored.");
                return;
            }

            _historyBinding.Add(item);
            HistoryItemAdded(item);

            StoreHistoryOnDisk();
        }

        public static void RemoveHistoryItem(HistoryItem item)
        {
            // This was changed because BindingSource doesn't support RemoveAll.
            if(!_historyBinding.Contains(item))
            {
                Log.Warning("Failed to remove history item from list. Item is not present in list.");
                return;
            }
            _historyBinding.Remove(item);

            StoreHistoryOnDisk();
            HistoryItemRemoved(item);
        }

        private static bool StoreHistoryOnDisk()
        {
            bool success = true;
            string jsonString = JsonConvert.SerializeObject(_historyBinding.List, Formatting.None, new ImageConverter());
            try
            {
                FileHelper.GZipWriteFile(SaveLocation + "history", jsonString);
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong while trying to store the history on disk. Exception: " + ex);
                success = false;
            }
            return success;
        }
    }
}
