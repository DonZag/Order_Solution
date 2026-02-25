using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Solution1_;

public class StartPage : IPage
{
    string text = @"Привет, этот бот помогает подобрать специалиста, который сможет решить вашу задачу (оказать услугу) или мы можем предложить Вам сотрудничество если вы являетесь профессионалом своего дела.
Выберите пожалуйста направление";//текст страницы
    string button1 = "Найти специалиста или услугу";//кнопка 1
    string button2 = "Найти заказ (сотрудничество)";//кнопка 2
    string pathVideo = "Resourses\\Videos\\video_2026-02-26_00-07-16.mp4";
    ReplyMarkup replyMarkup;
    public StartPage() { replyMarkup = new KeyBoard().OneButtonRowBoard_Reply(button1, button2); }//кнопки которые идут на старте

    public PageResultBase View(Update update, UserState userState)
    {
        var resourse = ResoursesService.GetResource(pathVideo); //без оператора new так как вызываемый метод статический и нет необходимости создавать экземпляр класса  ResoursesService
        return new VideoPageResult(resourse, text, replyMarkup)//возвращаем данные стартовой страницы согласно шаблону IPage + заполняем поле UpdatedUserState (наша страница StartPage, состояние пользователя)
        {
            UpdatedUserState = new UserState(this, userState.UserData)
        };
    }

    public PageResultBase Handle(Update update, UserState userState)
    {
        if (update.Message?.Text == button1)
        {
            return new Order_FirstStep_Page().View(update, userState);
        }
        else if (update.Message?.Text == button2)
        {
            return new Partner_FirstPage().View(update, userState);
        }
        else
        {
            return new PageResultBase("Нажмите на одну из кнопок", replyMarkup);//если пользователь ничего не выбрал
        }
    }
}
