using Solution1_;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Linq;

public class Order_FirstStep_Page : IPage
{

    string text = @"Тематика запросов для выбора: (или напечатать свое)";
    string button1 = "Строительство / Ремонт";
    string button2 = "Репетитор";
    string button3 = "Юридическая консультация";
    string button4 = "Клининговая служба";
    string button5 = "Трезвый водитель";
    string backButton = "Назад (вернуться на предыдущий шаг)";
    string pathPhoto = "Resourses\\Photos\\IMG_20231023_141620_0311.jpg";

    List<string> buttons = new List<string>();
    //ReplyMarkup replyMarkup;
    ReplyMarkup replyMarkup;
    public Order_FirstStep_Page() 
    {
        //inlinekeyboardMarkup = new KeyBoard().OneButtonRowBoard_Inline(button1, button2, button3, button4, button5, backButton);
        replyMarkup = new KeyBoard().OneButtonRowBoard_Inline(button1, button2);
        buttons = new[] { button1, button2 }.ToList();
    }
    public PageResultBase View(Update update, UserState userState)
    {
        var resourse = ResoursesService.GetResource(pathPhoto);
        return new PhotoPageResult(resourse, text, replyMarkup)
        {
            UpdatedUserState = new UserState(this, userState.UserData)
        };
    }

    public PageResultBase Handle(Update update, UserState userState)
    {
        foreach (var button in buttons)
        {
            if (update.Message?.Text == button && update.Message?.Text != backButton)
            {
                return new StartPage().View(update, userState);
            }
        }
        return new StartPage().View(update, userState);
    }

}