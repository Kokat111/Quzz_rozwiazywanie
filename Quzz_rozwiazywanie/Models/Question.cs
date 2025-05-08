using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Quzz_rozwiazywanie.Models
{
    public class Question
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public int Time { get; set; }
        public List<Answer> Answers { get; set; } = new();
    }
}
