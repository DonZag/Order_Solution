using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class UserData
    {
        public string? StepikId { get; set; }//информация о пользователи, возможно нужно будет переименовать на UserId

        public override string ToString()
        {
            return $"StepikID={StepikId}";
        }
    }
