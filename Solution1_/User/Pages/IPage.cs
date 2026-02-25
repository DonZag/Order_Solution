using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Solution1_;

public interface IPage
{
    PageResultBase View(Update update, UserState userState);//отображение и далее по страницам 

    PageResultBase Handle(Update update, UserState userState);//обработка после нажатия какой либо кнопки
}
