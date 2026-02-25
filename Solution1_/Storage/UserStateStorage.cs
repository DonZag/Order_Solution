using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

    internal class UserStateStorage//хранение состояния пользователя
    {
        private readonly ConcurrentDictionary<long, UserState> cache = new ConcurrentDictionary<long, UserState>();//long - TelegramUserId

        public void AddOrUpdate(long telegramUserId, UserState userstate)
        {
            cache.AddOrUpdate(telegramUserId, userstate, (k, v) => userstate);//замена если будет такой же ключ, метод для добавления ключа TelegramUserId
        }

        public bool TryGet(long telegramUserId, out UserState? userstate)//если есть TelegramUserId то добавляем его, ? - значение можеть быть null
        {
            return cache.TryGetValue(telegramUserId, out userstate);
        }
    }
