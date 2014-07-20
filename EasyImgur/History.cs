using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    static class History
    {
        public static event HistoryItemAddedEventHandler historyItemAdded;
        public static event HistoryItemRemovedEventHandler historyItemRemoved;
        public delegate void HistoryItemAddedEventHandler( HistoryItem _Item );
        public delegate void HistoryItemRemovedEventHandler( HistoryItem _Item );

        private static BindingSource m_HistoryBinding;

        public static int count
        {
            get { return m_HistoryBinding.Count; }
        }

        public static int anonymousCount
        {
            get 
            {
                int c = 0;
                foreach(HistoryItem item in m_HistoryBinding)
                {
                    if (item.anonymous)
                    {
                        ++c;
                    }
                }
                return c;
            }
        }

        static History()
        {
            // add empty event handlers to avoid checking if the event is null
            historyItemAdded += v => { };
            historyItemRemoved += v => { };
        }

        public static int accountCount
        {
            get { return count - anonymousCount; }
        }

        // required to be the first method called
        public static void BindData(BindingSource source)
        {
            m_HistoryBinding = source;
        }

        public static void InitializeFromDisk()
        {
            List<HistoryItem> history = History.GetHistoryFromDisk();
            if (history != null)
            {

                foreach (HistoryItem item in history)
                {
                    m_HistoryBinding.Add(item);
                    historyItemAdded(item);
                }
            }
        }

        private static List<HistoryItem> GetHistoryFromDisk()
        {
            string jsonString = string.Empty;
            try
            {
                jsonString = System.IO.File.ReadAllText("history");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.Info("Couldn't find a history file:\n" + ex.Message);
                return null;
            }
            catch (System.IO.IOException ex)
            {
                Log.Error("An I/O error occurred while opening the history file:\n" + ex.Message);
                return null;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Log.Error("Not authorized to open the history file:\n" + ex.Message);
                return null;
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Error("A security exception occurred while trying to open the history file: " + ex.Message);
                return null;
            }

            if (jsonString == null || jsonString == string.Empty)
            {
                return null;
            }

            List<HistoryItem> history = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HistoryItem>>(jsonString, new ImageConverter());
            return history;
        }

        public static void StoreHistoryItem(HistoryItem _Item)
        {
            if (_Item == null)
            {
                Log.Warning("NULL object passed to History.StoreHistoryItem. No item stored.");
                return;
            }
            if(m_HistoryBinding.Contains(_Item))
            {
                Log.Warning("Object already in history passed to History.StoreHistoryItem. No item stored.");
                return;
            }

            m_HistoryBinding.Add(_Item);
            historyItemAdded(_Item);

            StoreHistoryOnDisk();
        }

        public static void RemoveHistoryItem(HistoryItem _Item)
        {
            // This was changed because BindingSource doesn't support RemoveAll.
            if(!m_HistoryBinding.Contains(_Item))
            {
                Log.Warning("Failed to remove history item from list. Item is not present in list.");
                return;
            }
            else
                m_HistoryBinding.Remove(_Item);

            StoreHistoryOnDisk();
            historyItemRemoved(_Item);
        }

        private static bool StoreHistoryOnDisk()
        {
            bool success = true;
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(m_HistoryBinding.List, Newtonsoft.Json.Formatting.None, new ImageConverter());
            try
            {
                System.IO.File.WriteAllText("history", jsonString);
            }
            catch (System.Exception ex)
            {
                Log.Error("Something went wrong while trying to store the history on disk. Exception: " + ex.ToString());
                success = false;
            }
            return success;
        }
    }
}
