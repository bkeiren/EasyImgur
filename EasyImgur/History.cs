using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    class History
    {
        public static event HistoryItemAddedEventHandler historyItemAdded;
        public static event HistoryItemRemovedEventHandler historyItemRemoved;
        public delegate void HistoryItemAddedEventHandler( HistoryItem _Item );
        public delegate void HistoryItemRemovedEventHandler( HistoryItem _Item );

        private static List<HistoryItem> m_History = new List<HistoryItem>();

        public static int count
        {
            get { return m_History.Count; }
        }

        public static int anonymousCount
        {
            get 
            {
                int c = 0;
                foreach (HistoryItem item in m_History)
                {
                    if (item.anonymous)
                    {
                        ++c;
                    }
                }
                return c;
            }
        }

        public static int accountCount
        {
            get { return count - anonymousCount; }
        }

        public static void InitializeFromDisk()
        {
            List<HistoryItem> history = History.GetHistoryFromDisk();
            if (history != null)
            {
                m_History = history;
                foreach (HistoryItem item in m_History)
                {
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
                Log.Info("Couldn't find a history file.");
                return null;
            }
            catch (System.IO.IOException ex)
            {
                Log.Error("An I/O error occurred while opening the history file.");
                return null;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Log.Error("Not authorized to open the history file.");
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

            m_History.Add(_Item);
            historyItemAdded(_Item);

            StoreHistoryOnDisk();
        }

        public static void RemoveHistoryItem(HistoryItem _Item)
        {
            if (m_History.RemoveAll(item => item.id == _Item.id) <= 0)
            {
                Log.Warning("Failed to remove history item from list. Item is not present in list.");
                return;
            }

            StoreHistoryOnDisk();
            historyItemRemoved(_Item);
        }

        private static bool StoreHistoryOnDisk()
        {
            bool success = true;
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(m_History, Newtonsoft.Json.Formatting.None, new ImageConverter());
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
