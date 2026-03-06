using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserData
    {
        public string? StepikId { get; set; }//информация о пользователи, возможно нужно будет переименовать на UserId
        public Message? LastMessage { get; set; }// информация о последнем сообщени в ТГ для последующей его обработки

        public override string ToString()
        {
            return $"StepikID={StepikId}";
        }
    }
