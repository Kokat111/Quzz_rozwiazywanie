using System.Collections.Generic;

namespace Quzz_rozwiazywanie.Models
{
    public class Quiz
    {
        public string QuizName { get; set; }
        public List<Question> Questions { get; set; } = new();
    }
}