using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

    public class VideoPageResult : PageResultBase//добавляем другие классы (переносим в отдельный класс)
    {
        public InputFile Video { get; set; }//InputFile класс с using Telegram.Bot.Types

        public VideoPageResult(InputFile video, string text, ReplyMarkup replyMarkup) : base(text, replyMarkup)//попробовать поменять ReplyMarkupBase на IReplyMarkup
        {
            Video = video;
        }
    }
