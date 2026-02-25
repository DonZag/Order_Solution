using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

public class KeyBoard
{
    public ReplyKeyboardMarkup OneButtonRowBoard_Reply(params string[] texts)//каждый текст на отдельной строке как кнопка 
    {
        var rows = new List<KeyboardButton[]>();
        foreach (var text in texts)
        {
            rows.Add(new[] { new KeyboardButton(text) });
        }

        return new ReplyKeyboardMarkup(rows)
        {
            ResizeKeyboard = true
        };
    }

    public InlineKeyboardMarkup OneButtonRowBoard_Inline(params string[] texts)//текст под изображением
    {
        var rows = new List<InlineKeyboardButton[]>();
        foreach (var text in texts)
        {
            rows.Add(new[] { InlineKeyboardButton.WithCallbackData(text, text) });//пример с так называемой WithCallbackData ("Купить iPhone", "product_123");
        }

        return new InlineKeyboardMarkup(rows);
    }

    public ReplyKeyboardMarkup NumsButtonRowBoard_Reply(int buttonsPerRow, params string[] texts)//первым параметром указываем кол-во кнопок на строке
    {
        var rows = new List<KeyboardButton[]>();

        for (int i = 0; i < texts.Length; i += buttonsPerRow)
        {
            var rowTexts = texts.Skip(i).Take(buttonsPerRow).ToArray();
            var rowButtons = rowTexts.Select(t => new KeyboardButton(t)).ToArray();
            rows.Add(rowButtons);
        }

        return new ReplyKeyboardMarkup(rows)
        {
            ResizeKeyboard = true
        };
    }

    public InlineKeyboardMarkup NumsButtonRowBoard_Inline(int buttonsPerRow, params string[] texts)
    {
        var rows = new List<InlineKeyboardButton[]>();

        for (int i = 0; i < texts.Length; i += buttonsPerRow)
        {
            var rowTexts = texts.Skip(i).Take(buttonsPerRow).ToArray();
            var rowButtons = rowTexts.Select(t => InlineKeyboardButton.WithCallbackData(t, t)).ToArray();
            rows.Add(rowButtons);
        }

        return new InlineKeyboardMarkup(rows);
    }
}
