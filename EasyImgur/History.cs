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

        private static BindingSource historyBinding;

        private static string SaveLocation => Program.RootFolder;

        public static int Count => historyBinding.Count;

        public static int AnonymousCount
        {
            get
            {
                return historyBinding.Cast<HistoryItem>().Count(item => item.Anonymous);
            }
        }

        public static int AccountCount => Count - AnonymousCount;

        // required to be the first method called
        public static void BindData(BindingSource source)
        {
            historyBinding = source;
        }

        public static void InitializeFromDisk()
        {
            var history = GetHistoryFromDisk();
            if (history == null) return;
            foreach (var item in history)
            {
                historyBinding.Add(item);
                HistoryItemAdded?.Invoke(item);
            }
        }

        private static List<HistoryItem> GetHistoryFromDisk()
        {
            string jsonString;
            try
            {
                jsonString = FileHelper.GZipReadFile(Path.Combine(SaveLocation, "history"));
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
            if (historyBinding.Contains(item))
            {
                Log.Warning("Object already in history passed to History.StoreHistoryItem. No item stored.");
                return;
            }

            historyBinding.Add(item);
            HistoryItemAdded?.Invoke(item);

            StoreHistoryOnDisk();
        }

        public static void RemoveHistoryItem(HistoryItem item)
        {
            // This was changed because BindingSource doesn't support RemoveAll.
            if (!historyBinding.Contains(item))
            {
                Log.Warning("Failed to remove history item from list. Item is not present in list.");
                return;
            }
            historyBinding.Remove(item);

            StoreHistoryOnDisk();
            HistoryItemRemoved?.Invoke(item);
        }

        private static bool StoreHistoryOnDisk()
        {
            var success = true;
            var jsonString = JsonConvert.SerializeObject(historyBinding.List, Formatting.None, new ImageConverter());
            try
            {
                FileHelper.GZipWriteFile(Path.Combine(SaveLocation, "history"), jsonString);
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
