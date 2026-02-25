using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace Solution1_
{
     
    class Program
    {
        static UserStateStorage storage = new UserStateStorage();
        static async Task Main(string[] args)
        {
            var tgBotClient = new TelegramBotClient("8544403341:AAGFqSwS8ee_Df3PuajykVw-7YQcvhNxjNc");//создание объекта тг бот API

            var user = await tgBotClient.GetMe();//получение информации (данных) от какого-либо пользователя чз метод GetMeAsync()
                                                 //использование async и await позволяет вашему приложению свободно обрабатывать другие операции, пока они ожидают завершения сетевых вызовов

            tgBotClient.StartReceiving(updateHandler: HandleUpdate, errorHandler: HandlePoolingError);//начало получения обновлений (updates) от Telegram
                                                                                                      //передача в параметр updateHandler ссылку на метод который будет вызываться прикаждом update 
                                                                                                      //передача в параметр pollingErrorHandler: ссылку на метод который будет вызываться прикаждом update для обработки ошибок
            Console.ReadLine();
        }

        private static async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var Upd_Message = update.Message?.Text;
            if(Upd_Message == null)//если в update появилось / есть Message и Text равен null
            {
                return;
            }
            /*var chatId = update.Message.Chat.Id;
            var messageTime = update.Message.Date; // Время сообщения (UTC)
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            if (messageTime <= fiveMinutesAgo)
            {
                return;
            }
            if (update.Message.Text != "/start" || update.Message.Text != "/menu")
            {
                await client.SendMessage(
                    chatId: chatId,
                    text: "Привет! Давай начнем пользоваться ботом"
                );
            }*/

            var telegramUserId = update.Message.From.Id;//чз наш update находим UserId который начал взаимодействовать с ботом

            var IsExistUserState = storage.TryGet(telegramUserId, out var userState);//получаем в каком стостоянии находится пользователь

            if (!IsExistUserState)
            {
                userState = new UserState(new NotStartedPage(), new UserData());//пользователь начал что то нажимать у нашего бота
            }

            Console.WriteLine($"update_Id: {update.Id}, Current_userState: {userState}");

            var result = userState!.Page.Handle(update, userState);//чз метод View отображаем на какой странице пользователь, ! - точно знаем что не null
            Console.WriteLine($"update_Id: {update.Id}, text: {result.Text}, UpdatedUserState: {result.UpdatedUserState}");

            /*
            var Upd_chatId = update.Message.Chat.Id;//чз наш update находим неохдимый чат
                                                    //var Upd_text = update.Message.Text;//чз наш update обрабатываем полученное сообщение
            var Upd_messageId = update.Message.MessageId;//чз наш update получаем Id отправленного сообщения
            */

            /*await client.SendMessage(chatId: Upd_chatId, //работа с ITelegramBotClient (Telegram Bot API) чз параметр client с возможностью вывода какого-либо метода
                              text: $"Вы прислали {Upd_Message}", //в нашем случае отправка сообщения чз метод SendTextMessageAsync
                             replyParameters: Upd_messageId);//ответ Upd_text на полученное сообщение в update*/

            //тоже что и предыдущее только в другом варианте

            switch(result)//обработка result на случай если он будет не PageResultBase а другого типа и автоматически конвертируем в нужный класс, в данном случае PhotoPageResult 
            {
                case PhotoPageResult photopageresult:
                    await client.SendPhoto(
                        chatId: telegramUserId,//чат с пользователем
                        photo: photopageresult.Photo,//свойство Photo(тип InputFile) у PhotoPageResult 
                        caption: photopageresult.Text,//текст
                        replyMarkup: photopageresult.InlineKeyboardMarkup);//клавиатура
                    break;

                case VideoPageResult videoPageResult:
                    await client.SendVideo(
                        chatId: telegramUserId,
                        video: videoPageResult.Video,
                        caption: videoPageResult.Text,
                        replyMarkup: videoPageResult.ReplyMarkup);
                    break;

                default:
                    await client.SendMessage(chatId: telegramUserId,
                        text: result.Text,
                        replyMarkup: result.ReplyMarkup);
                    break;
            }


            storage.AddOrUpdate(telegramUserId, result.UpdatedUserState);//сохраняем состояние нашего пользователя


            //throw new NotImplementedException();
        }

        private static async Task HandlePoolingError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            //throw new NotImplementedException();
            Console.WriteLine(exception.Message);//определение ошибки текстом
        }
    }
}
