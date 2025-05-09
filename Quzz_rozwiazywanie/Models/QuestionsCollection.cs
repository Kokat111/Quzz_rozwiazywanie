using System;

namespace Quzz_rozwiazywanie.Models
{
    public class QuestionsCollection
    {
        // Właściwości pytania
        public string Question { get; set; } = string.Empty;
        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;
        public string Answer4 { get; set; } = string.Empty;

        // Czy odpowiedzi są poprawne
        public bool IsCorrectAnswer1 { get; set; } = false;
        public bool IsCorrectAnswer2 { get; set; } = false;
        public bool IsCorrectAnswer3 { get; set; } = false;
        public bool IsCorrectAnswer4 { get; set; } = false;

        // Czas odpowiedzi
        public string AnswerTime { get; set; } = string.Empty;

        // Konstruktor do łatwego tworzenia pytań
        public QuestionsCollection(string question, string answer1, string answer2, string answer3, string answer4,
            bool isCorrectAnswer1, bool isCorrectAnswer2, bool isCorrectAnswer3, bool isCorrectAnswer4, string answerTime)
        {
            Question = question;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
            Answer4 = answer4;
            IsCorrectAnswer1 = isCorrectAnswer1;
            IsCorrectAnswer2 = isCorrectAnswer2;
            IsCorrectAnswer3 = isCorrectAnswer3;
            IsCorrectAnswer4 = isCorrectAnswer4;
            AnswerTime = answerTime;
        }

        // Konstruktor bez parametrów dla łatwego dodawania pustych pytań
        public QuestionsCollection() { }
    }
}
