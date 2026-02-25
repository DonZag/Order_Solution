using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

    public class ResoursesService
    {
        public static InputFileStream GetResource(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return InputFile.FromStream(fileStream);
        }
    }

