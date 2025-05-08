using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quzz_rozwiazywanie.Models
{
    public class Answer
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }
}
