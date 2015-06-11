using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    static class FormattingHelper
    {
        public class FormattingScheme
        {
            public string Symbol { get; private set; }
            public string Description { get; private set; }
            public ReplacementFactory Factory { get; private set; }

            public FormattingScheme(string symbol, string description, ReplacementFactory factory)
            {
                this.Symbol = symbol;
                this.Description = description;
                this.Factory = factory;
            }
        }

        public class FormattingContext
        {
            public string FilePath { get; set; }
            public int AlbumIndex { get; set; }

            public FormattingContext()
            {
                FilePath = "";
            }
        }

        public delegate string ReplacementFactory(FormattingContext context);

        private readonly static FormattingScheme[] FormattingSchemes =
        {
            new FormattingScheme(
                "%n%",
                "The position of the current upload. Ex.: First uploaded image is 1, fifth 5, tenth is 10, etc.",
                context => (ImgurAPI.numSuccessfulUploads + 1).ToString()),
            new FormattingScheme(
                "%date%",
                "Current date in DD-MM-YYYY format.",
                context => "%day%-%month%-%year%"),
            new FormattingScheme(
                "%time%",
                "Current time in HH:MM:SS format.",
                context => "%hour%:%minute%:%second%"),
            new FormattingScheme(
                "%day%",
                "Current day of the month (1-31)",
                context => DateTime.Now.Day.ToString()),
            new FormattingScheme(
                "%month%",
                "Current month (1-12)",
                context => DateTime.Now.Month.ToString()),
            new FormattingScheme(
                "%year%",
                "Current year",
                context => DateTime.Now.Year.ToString()),
            new FormattingScheme(
                "%hour%",
                "Current hour (0-23)",
                context => DateTime.Now.Hour.ToString()),
            new FormattingScheme(
                "%minute%",
                "Current minute (0-59)",
                context => DateTime.Now.Minute.ToString()),
            new FormattingScheme(
                "%second%",
                "Current second (0-59)",
                context => DateTime.Now.Second.ToString()),
            new FormattingScheme(
                "%filename%",
                "The file name of the file that is being uploaded (if the source is a file)",
                context => Path.GetFileName(context.FilePath)),
            new FormattingScheme(
                "%filepath%",
                "The file path of the file that is being uploaded (if the source is a file)",
                context => context.FilePath),
            new FormattingScheme(
                "%albumindex%",
                "The position of the image in the album (0-n, where n = the number of images. Only applies if an album is being uploaded)",
                context => context.AlbumIndex.ToString())
        };
        
        static public string Format(string input, FormattingContext ctx)
        {
            FormattingContext context = ctx ?? new FormattingContext();
            // Aggregate is essentially a foreach loop through the FormattingSchemes, doing
            // current = current.Replace(...);
            return FormattingSchemes
                .Aggregate(input, (current, scheme) => current.Replace(scheme.Symbol, scheme.Factory(context)));
        }

        static public FormattingScheme[] GetSchemes()
        {
            return FormattingSchemes;
        }
    }
}
