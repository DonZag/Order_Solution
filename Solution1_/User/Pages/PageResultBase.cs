using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class PageResultBase
{
        public string Text { get; } //возвращаем текст от пользователя

        public ReplyMarkup ReplyMarkup { get; }//клавиатура в зависимости от страницы где находится пользователь

        public InlineKeyboardMarkup InlineKeyboardMarkup { get; }//другой тип клавиатуры 

        public UserState UpdatedUserState { get; set; }// состояние пользователя

        public ParseMode ParseMode { get; set; } = ParseMode.Html;

        /*public PageResultBase(string text, InlineKeyboardMarkup replyMarkup)
        {
            Text = text;
            InlineKeyboardMarkup = replyMarkup;
        }*/

        public PageResultBase(string text, ReplyMarkup replyMarkup)
        {
            Text = text;
            ReplyMarkup = replyMarkup;
        }

    public bool IsMedia => this is PhotoPageResult ||
                           this is VideoPageResult;

}
