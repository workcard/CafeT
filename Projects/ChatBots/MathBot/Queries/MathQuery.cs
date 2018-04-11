namespace MathBot.Queries
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class MathQuery
    {
        [Prompt("Nhập biểu thức bạn cần tính {&}")]
        public string Text { get; set; }
        
        //[Prompt("When do you want to {&}?")]
        //public DateTime CheckIn { get; set; }

        //[Numeric(1, int.MaxValue)]
        //[Prompt("How many {&} do you want to stay?")]
        //public int Nights { get; set; }
    }
}