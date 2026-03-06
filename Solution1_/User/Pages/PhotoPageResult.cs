using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

public class PhotoPageResult : PageResultBase//добавляем другие классы (переносим в отдельный класс)
    {
        public InputFile Photo { get; set; }//InputFile класс с using Telegram.Bot.Types

        /*public PhotoPageResult(InputFile photo, string text, InlineKeyboardMarkup replyMarkup) : base(text, replyMarkup)//попробовать поменять ReplyMarkupBase на IReplyMarkup
        {
            Photo = photo;
        }*/

        public PhotoPageResult(InputFile photo, string text, ReplyMarkup replyMarkup) : base(text, replyMarkup)//попробовать поменять ReplyMarkupBase на IReplyMarkup
        {
            Photo = photo;
        }
}