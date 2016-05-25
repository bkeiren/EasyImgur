using System.Collections.Generic;
using System.Windows.Forms;

namespace EasyImgur
{
    static class Contributors
    {
        public class Contributor
        {
            public string Name { get; set; }
            public string Alias { get; set; }
            public string Url { get; set; }

            public override string ToString()
            {
                string str = this.Name;
                if (!string.IsNullOrEmpty(this.Alias))
                    str += " (" + this.Alias + ")";
                return str;
            }
        }

        public static List<Contributor> ContributorList { get; private set; }

        public static BindingSource BindingSource { get; private set; }

        static Contributors()
        {
            BindingSource = new BindingSource();
            ContributorList = new List<Contributor>
            {
                new Contributor
                {
                    Name = "Alex van Liew",
                    Alias = "snoozbuster",
                    Url = "https://github.com/snoozbuster"
                },
                new Contributor
                {
                    Name = "Joona Heikkilä",
                    Alias = "cubrr",
                    Url = "https://github.com/cubrr"
                },
                // Add new contributor here

                new Contributor // Leave this for last at all times.
                {
                    Name = "...and every user of EasyImgur",
                    Alias = "",
                    Url = ""
                }
            };
        }
    }
}
