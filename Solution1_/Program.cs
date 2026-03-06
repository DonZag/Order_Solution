using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace Solution1_
{
     
    class Program
    {
        static UserStateStorage storage = new UserStateStorage();
        static async Task Main(string[] args)
        {

            /*IWebProxy systemProxy = WebRequest.GetSystemWebProxy();
            systemProxy.Credentials = CredentialCache.DefaultCredentials;

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = systemProxy,
                UseProxy = true // Явно указываем, что нужно использовать прокси
            };

            var httpClient = new HttpClient(httpClientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(30);*/

            //var tgBotClient = new TelegramBotClient("8544403341:AAGFqSwS8ee_Df3PuajykVw-7YQcvhNxjNc", httpClient);//создание объекта тг бот API

            var tgBotClient = new TelegramBotClient("8544403341:AAGFqSwS8ee_Df3PuajykVw-7YQcvhNxjNc");

            var user = await tgBotClient.GetMe();//получение информации (данных) от какого-либо пользователя чз метод GetMeAsync()
                                                 //использование async и await позволяет вашему приложению свободно обрабатывать другие операции, пока они ожидают завершения сетевых вызовов

            Console.WriteLine($"Бот @{user.Username} успешно запущен и готов к работе!");

            var receiveroptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(), // получение обновлений
                DropPendingUpdates = true
            };
            tgBotClient.StartReceiving(updateHandler: HandleUpdate, errorHandler: HandlePoolingError, receiverOptions: receiveroptions);//начало получения обновлений (updates) от Telegram
                                                                                                      //передача в параметр updateHandler ссылку на метод который будет вызываться прикаждом update 
                                                                                                      //передача в параметр pollingErrorHandler: ссылку на метод который будет вызываться прикаждом update для обработки ошибок
            Console.ReadLine();
        }

        private static async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
        {
            /*var Upd_Message = update.Message?.Text;
            if (Upd_Message == null)//если в update появилось / есть Message и Text равен null
            {
                return;
            }*/
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

            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message && update.Type != Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)//если ответ не сообщение и не ответ от CallbackQuery (нажатие на кнопку InlineKeyBoard)
            {
                return;
            }
            long telegramUserId;
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                telegramUserId = update!.Message!.From!.Id;//чз наш update находим UserId который начал взаимодействовать с ботом
            }
            else
            {
                telegramUserId = update!.CallbackQuery!.From!.Id;
            }


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

            var lastMessage = await SendResult(client, update, telegramUserId, IsExistUserState, result);

            result.UpdatedUserState.UserData.LastMessage = new Message(lastMessage.MessageId, result.IsMedia);

            storage.AddOrUpdate(telegramUserId, result.UpdatedUserState);//сохраняем состояние нашего пользователя


            //throw new NotImplementedException();
        }

        private static async Task<Telegram.Bot.Types.Message> SendResult(ITelegramBotClient client, Update update, long telegramUserId, bool IsExistUserState, PageResultBase result)
        {
            switch (result)//обработка result на случай если он будет не PageResultBase а другого типа и автоматически конвертируем в нужный класс, в данном случае PhotoPageResult 
            {
                case PhotoPageResult photopageresult:
                    return await SendPhoto(client, update, telegramUserId, result, photopageresult);

                /*case VideoPageResult videoPageResult:
                    return await client.SendVideo(
                        chatId: telegramUserId,
                        video: videoPageResult.Video,
                        caption: videoPageResult.Text,
                        replyMarkup: videoPageResult.ReplyMarkup);*/

                default:
                   return await SendText(client, update, telegramUserId, IsExistUserState, result);
            }
        }

        private static async Task<Telegram.Bot.Types.Message> SendPhoto(ITelegramBotClient client, Update update, long telegramUserId, PageResultBase result, PhotoPageResult photopageresult)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageMedia(
                    chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                    media: new InputMediaPhoto(photopageresult.Photo)
                    {
                        Caption = result.Text,
                        ParseMode = result.ParseMode
                    },
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup);
            }

            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessage(
                    chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }
            return await client.SendPhoto(
                chatId: telegramUserId,//чат с пользователем
                photo: photopageresult.Photo,//свойство Photo(тип InputFile) у PhotoPageResult 
                caption: photopageresult.Text,//текст
                replyMarkup: photopageresult.ReplyMarkup,
                parseMode: result.ParseMode);//клавиатура
        }

        private static async Task<Telegram.Bot.Types.Message> SendText(ITelegramBotClient client, Update update, long telegramUserId, bool IsExistUserState, PageResultBase result)
        {
            if (update.CallbackQuery != null && (!result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))//если кнопка нажата, т.е. типа CallbackQuery и последнее сообщение не медиа то true
            {
                return await client.EditMessageText(
                    chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                    text: result.Text,
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup,
                    parseMode: result.ParseMode);
            }

            if(result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessage(
                    chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }

            return await client.SendMessage(chatId: telegramUserId,
                                            text: result.Text,
                                            replyMarkup: result.ReplyMarkup,
                                            parseMode: result.ParseMode);
        }

        private static Task HandlePoolingError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            //throw new NotImplementedException();
            //Console.WriteLine(exception.Message);//определение ошибки текстом
            var errorMessage = exception.ToString(); // Посмотрите полный стек ошибки здесь
            Console.WriteLine($"Ошибка в polling: {errorMessage}");
            return Task.CompletedTask;
        }
    }
}
