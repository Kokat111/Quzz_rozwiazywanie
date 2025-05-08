using Quzz_rozwiazywanie.Helpers;
using Quzz_rozwiazywanie.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Quzz_rozwiazywanie.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Quiz _quiz;
        private int _currentQuestionIndex = 0;
        private System.Timers.Timer _quizTimer;
        private TimeSpan _elapsedTime;
        private string _selectedQuizName;

        public ObservableCollection<string> QuizNames { get; set; } = new();
        public string SelectedQuizName
        {
            get => _selectedQuizName;
            set { _selectedQuizName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Answer> CurrentAnswers =>
            new ObservableCollection<Answer>(_quiz?.Questions[_currentQuestionIndex].Answers ?? new());

        public string CurrentQuestion => _quiz?.Questions[_currentQuestionIndex].QuestionText ?? "";
        public string Title => _quiz?.QuizName ?? "";
        public string TimeDisplay => _elapsedTime.ToString(@"mm\:ss");

        public bool IsQuizStarted { get; private set; }
        public bool IsQuizFinished { get; private set; }

        public ICommand LoadQuizCommand { get; }
        public ICommand StartQuizCommand { get; }
        public ICommand NextQuestionCommand { get; }
        public ICommand FinishQuizCommand { get; }

        public QuizViewModel()
        {
            LoadQuizCommand = new RelayCommand(LoadQuiz);
            StartQuizCommand = new RelayCommand(StartQuiz, () => _quiz != null && !IsQuizStarted);
            FinishQuizCommand = new RelayCommand(FinishQuiz, () => IsQuizStarted);
            NextQuestionCommand = new RelayCommand(NextQuestion);
            _quizTimer = new System.Timers.Timer(1000);
            _quizTimer.Elapsed += (s, e) =>
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeDisplay));
            };
            LoadQuizNames();
        }

        private void LoadQuizNames()
        {
            QuizNames.Clear();
            foreach (var name in DatabaseHelper.GetQuizNames())
                QuizNames.Add(name);
        }

        private void LoadQuiz()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedQuizName))
                {
                    MessageBox.Show("Wybierz quiz z listy.");
                    return;
                }

                var encryptedText = DatabaseHelper.LoadEncryptedQuizJson(SelectedQuizName);
                var json = CaesarCipher.Decrypt(encryptedText, 2);
                var loadedQuiz = JsonSerializer.Deserialize<Quiz>(json);

                if (!ValidateQuiz(loadedQuiz, out string error))
                {
                    MessageBox.Show($"Błąd walidacji quizu:\n{error}");
                    return;
                }

                _quiz = loadedQuiz;
                _currentQuestionIndex = 0;
                _elapsedTime = TimeSpan.Zero;

                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(CurrentAnswers));
                OnPropertyChanged(nameof(Title));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania quizu:\n{ex.Message}");
            }
        }

        private bool ValidateQuiz(Quiz quiz, out string error)
        {
            error = "";
            if (quiz == null || quiz.Questions == null || quiz.Questions.Count == 0)
            {
                error = "Quiz jest pusty lub niepoprawnie zbudowany.";
                return false;
            }

            foreach (var q in quiz.Questions)
            {
                if (string.IsNullOrWhiteSpace(q.QuestionText))
                {
                    error = "Jedno z pytań ma pustą treść.";
                    return false;
                }

                if (q.Answers == null || q.Answers.Count != 4)
                {
                    error = $"Pytanie '{q.QuestionText}' nie ma dokładnie 4 odpowiedzi.";
                    return false;
                }

                if (q.Answers.Any(a => string.IsNullOrWhiteSpace(a.AnswerText)))
                {
                    error = $"Jedna z odpowiedzi w pytaniu '{q.QuestionText}' jest pusta.";
                    return false;
                }

                if (!q.Answers.Any(a => a.IsCorrect))
                {
                    error = $"Pytanie '{q.QuestionText}' nie ma żadnej poprawnej odpowiedzi.";
                    return false;
                }
            }

            return true;
        }

        private void StartQuiz()
        {
            IsQuizStarted = true;
            IsQuizFinished = false;
            _elapsedTime = TimeSpan.Zero;
            _quizTimer.Start();

            OnPropertyChanged(nameof(IsQuizStarted));
            OnPropertyChanged(nameof(IsQuizFinished));
        }
        private void NextQuestion()
        {
            if (_currentQuestionIndex < _quiz.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                _elapsedTime = TimeSpan.Zero;

                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(CurrentAnswers));
            }
            else
            {
                FinishQuiz();
            }
        }
        private void FinishQuiz()
        {
            IsQuizStarted = false;
            IsQuizFinished = true;
            _quizTimer.Stop();

            int correctCount = 0;
            int totalCount = _quiz.Questions.Count;

            string result = "Wyniki:\n\n";
            foreach (var question in _quiz.Questions)
            {
                result += $"Pytanie: {question.QuestionText}\n";
                bool userCorrect = true;

                foreach (var answer in question.Answers)
                {
                    string marker;

                    if (answer.IsCorrect && answer.IsSelected)
                    {
                        marker = "[✓]";
                    }
                    else if (!answer.IsCorrect && answer.IsSelected)
                    {
                        marker = "[✗]";
                        userCorrect = false;
                    }
                    else if (answer.IsCorrect && !answer.IsSelected)
                    {
                        marker = "[✓]"; 
                        userCorrect = false;
                    }
                    else
                    {
                        marker = "[ ]";
                    }

                    result += $"{marker} {answer.AnswerText}\n";
                }

                if (userCorrect) correctCount++;
                result += "\n";
            }

            result += $"Poprawnych odpowiedzi: {correctCount} / {totalCount}";

            MessageBox.Show(result, "Wyniki i poprawne odpowiedzi");
            OnPropertyChanged(nameof(IsQuizStarted));
            OnPropertyChanged(nameof(IsQuizFinished));
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}