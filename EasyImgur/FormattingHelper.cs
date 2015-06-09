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

        public class FormattingContext
        {
            public string   m_Filepath      = string.Empty;
            public int      m_AlbumIndex    = 0;
        }

        public delegate string ReplacementFactory( FormattingContext _Context );

        static private FormattingScheme[] m_FormattingSchemes = {
                                                                    new FormattingScheme("%n%", "The position of the current upload. Ex.: First uploaded image is 1, fifth 5, tenth is 10, etc.", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return (ImgurAPI.numSuccessfulUploads + 1).ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%date%", "Current date in DD-MM-YYYY format.", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return "%day%-%month%-%year%"; 
                                                                                         }),
                                                                    new FormattingScheme("%time%", "Current time in HH:MM:SS format.", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return "%hour%:%minute%:%second%"; 
                                                                                         }),
                                                                    new FormattingScheme("%day%", "Current day of the month (1-31)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Day.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%month%", "Current month (1-12)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Month.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%year%", "Current year", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Year.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%hour%", "Current hour (0-23)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Hour.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%minute%", "Current minute (0-59)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Minute.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%second%", "Current second (0-59)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         { 
                                                                                             return System.DateTime.Now.Second.ToString(); 
                                                                                         }),
                                                                    new FormattingScheme("%filename%", "The file name of the file that is being uploaded (if the source is a file)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         {
                                                                                             return System.IO.Path.GetFileName(_Context.m_Filepath);
                                                                                         }),
                                                                    new FormattingScheme("%filepath%", "The file path of the file that is being uploaded (if the source is a file)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         {
                                                                                             return _Context.m_Filepath;
                                                                                         }),
                                                                    new FormattingScheme("%albumindex%", "The position of the image in the album (0-n, where n = the number of images. Only applies if an album is being uploaded)", 
                                                                                         delegate (FormattingContext _Context)
                                                                                         {
                                                                                             return _Context.m_AlbumIndex.ToString();
                                                                                         })
                                                                };
        
        static public string Format( string _Input, FormattingContext _Context )
        {
            string Output = _Input;
            
            FormattingContext context = _Context ?? new FormattingContext();

            foreach (FormattingScheme scheme in m_FormattingSchemes)
            {
                Output = Output.Replace(scheme.symbol, scheme.factory(context));
            }
            return Output;
        }

        static public FormattingScheme[] GetSchemes()
        {
            return m_FormattingSchemes;
        }
    }
}
