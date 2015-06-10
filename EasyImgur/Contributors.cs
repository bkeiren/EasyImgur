using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    static class Contributors
    {
        public class Contributor
        {
            public String name { get; set; }
            public String alias { get; set; }
            public String url { get; set; }

            public override string ToString()
            {
                String str = name;
                if (!String.IsNullOrEmpty(alias))
                    str += " (" + alias + ")";
                return str;
            }
        }

        static public List<Contributor> contributors = new List<Contributor> {
            new Contributor { name = "Alex van Liew",           alias = "snoozbuster",              url = "https://github.com/snoozbuster"      },
            new Contributor { name = "Joona Heikkilä",          alias = "cubrr",                    url = "https://github.com/cubrr"            },


            // Leave this for last at all times.
            new Contributor { name = "and every user of EasyImgur",  alias = "",                    url = ""                                    }
        };

        static public BindingSource bindingSource = new BindingSource();
    }
}
