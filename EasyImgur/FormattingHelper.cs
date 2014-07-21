using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    static class FormattingHelper
    {
        public class FormattingScheme
        {
            public FormattingScheme( string _Symbol, string _Description, ReplacementFactory _Factory )
            {
                m_Symbol = _Symbol;
                m_HumanReadableDescription = _Description;
                m_ReplacementFactory = _Factory;
            }

            private string m_Symbol;
            private string m_HumanReadableDescription;
            private ReplacementFactory m_ReplacementFactory;

            public string symbol
            {
                get
                {
                    return m_Symbol;
                }
            }
            public string description
            {
                get
                {
                    return m_HumanReadableDescription;
                }
            }
            public ReplacementFactory factory
            {
                get
                {
                    return m_ReplacementFactory;
                }
            }
        }

        public delegate string ReplacementFactory();

        static private FormattingScheme[] m_FormattingSchemes = {
                                                                    new FormattingScheme("%n%", "The index of the current upload. Ex.: First uploaded image is 0, fifth 4, tenth is 9, etc.", delegate{ return ImgurAPI.numSuccessfulUploads.ToString(); }),
                                                                    new FormattingScheme("%date%", "Current date in DD-MM-YYYY format.", delegate{ return "%day%-%month%-%year%"; }),
                                                                    new FormattingScheme("%time%", "Current time in HH:MM:SS format.", delegate{ return "%hour%:%minute%:%second%"; }),
                                                                    new FormattingScheme("%day%", "Current day of the month (1-31)", delegate{ return System.DateTime.Now.Day.ToString(); }),
                                                                    new FormattingScheme("%month%", "Current month (1-12)", delegate{ return System.DateTime.Now.Month.ToString(); }),
                                                                    new FormattingScheme("%year%", "Current year", delegate{ return System.DateTime.Now.Year.ToString(); }),
                                                                    new FormattingScheme("%hour%", "Current hour (0-23)", delegate{ return System.DateTime.Now.Hour.ToString(); }),
                                                                    new FormattingScheme("%minute%", "Current minute (0-59)", delegate{ return System.DateTime.Now.Minute.ToString(); }),
                                                                    new FormattingScheme("%second%", "Current second (0-59)", delegate{ return System.DateTime.Now.Second.ToString(); }),
                                                                };
        
        static public string Format( string _Input )
        {
            string Output = _Input;
            foreach (FormattingScheme scheme in m_FormattingSchemes)
            {
                Output = Output.Replace(scheme.symbol, scheme.factory());
            }
            return Output;
        }

        static public FormattingScheme[] GetSchemes()
        {
            return m_FormattingSchemes;
        }
    }
}
