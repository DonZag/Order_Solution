using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class NotStartedPage : IPage
{
    public PageResultBase View(Update update, UserState userState)
    {
        return null;
    }

    public PageResultBase Handle(Update update, UserState userState)
    {
        return new StartPage().View(update, userState);
    }
}
