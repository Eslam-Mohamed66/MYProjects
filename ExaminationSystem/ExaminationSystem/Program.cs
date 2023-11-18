
using System;
using System.Collections.Generic;

namespace Examinationsystem
{
    class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
    }

    class Question
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Mark { get; set; }
        public List<Answer> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public int UserAnswerIndex { get; set; } // Added for user's answer
    }

    public class Exam
    {
        public TimeSpan TimeOfExam { get; set; }
        public int NumberOfQuestions { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual void ShowExam()
        {
            // Implementation will be different for each exam type
        }
       public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalMinutes:00}:{timeSpan.Seconds:00}";
        }

    }

    class FinalExam : Exam
    {
        public List<Question> TrueOrFalseQuestions { get; set; } = new List<Question>();
        public List<Question> MCQQuestions { get; set; }
        public double Grade { get; set; }

        public override void ShowExam()
        {
            Console.WriteLine("Final Exam - True or False Questions:");
            foreach (var question in TrueOrFalseQuestions)
            {
                Console.WriteLine("Question: " + question.Body);
                Console.WriteLine("Your Answer: " + (question.UserAnswerIndex == 0 ? "True" : "False"));
                Console.WriteLine("Correct Answer: " + (question.CorrectAnswerIndex == 0 ? "True" : "False"));
                Console.WriteLine();
            }

            Console.WriteLine("Final Exam - Multiple Choice Questions:");
            foreach (var question in MCQQuestions)
            {
                Console.WriteLine("Question: " + question.Body);
                Console.WriteLine("Options:");
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i].AnswerText}");
                }
                Console.WriteLine("Your Answer: " + (question.UserAnswerIndex + 1));
                Console.WriteLine("Correct Answer: " + (question.CorrectAnswerIndex + 1));
                Console.WriteLine();
            }

            CalculateGrade();
            Console.WriteLine("Final Exam Grade: " + Grade);
            Console.WriteLine("Time Taken: " + FormatTimeSpan(EndTime - StartTime));
        }

        private void CalculateGrade()
        {
            double totalQuestions = TrueOrFalseQuestions.Count + MCQQuestions.Count;
            double correctAnswers = 0;

            foreach (var question in TrueOrFalseQuestions)
            {
                if (question.UserAnswerIndex == question.CorrectAnswerIndex)
                {
                    correctAnswers++;
                }
            }

            foreach (var question in MCQQuestions)
            {
                if (question.UserAnswerIndex == question.CorrectAnswerIndex)
                {
                    correctAnswers++;
                }
            }

            Grade = (correctAnswers / totalQuestions) * 100;
        }
    }

    class PracticalExam : Exam
    {
        public List<Question> MCQQuestions { get; set; }
        public double Grade { get; set; }

        public override void ShowExam()
        {
            Console.WriteLine("Practical Exam - Multiple Choice Questions:");
            foreach (var question in MCQQuestions)
            {
                Console.WriteLine("Question: " + question.Body);
                Console.WriteLine("Options:");
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i].AnswerText}");
                }
                Console.WriteLine("Your Answer: " + (question.UserAnswerIndex + 1));
                Console.WriteLine("Correct Answer: " + (question.CorrectAnswerIndex + 1));
                Console.WriteLine();
            }

            CalculateGrade();
            Console.WriteLine("Practical Exam Grade: " + Grade);
            Console.WriteLine("Time Taken: " + FormatTimeSpan(EndTime - StartTime));
        }

        private void CalculateGrade()
        {
            double totalQuestions = MCQQuestions.Count;
            double correctAnswers = 0;

            foreach (var question in MCQQuestions)
            {
                if (question.UserAnswerIndex == question.CorrectAnswerIndex)
                {
                    correctAnswers++;
                }
            }

            Grade = (correctAnswers / totalQuestions) * 100;
        }
    }

    class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Exam ExamOfSubject { get; set; }

        public void CreateExam(Exam exam)
        {
            exam.StartTime = DateTime.Now;
            ExamOfSubject = exam;
        }
    }

    class Program
    {
        //static string FormatTimeSpan(TimeSpan timeSpan)
        //{
        //    return $"{timeSpan.Minutes} minutes and {timeSpan.Seconds} seconds";
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Examination System!");
            Console.WriteLine("Choose an Exam Type:");
            Console.WriteLine("1. Practical Exam");
            Console.WriteLine("2. Final Exam");

            int choice = int.Parse(Console.ReadLine());

            Exam selectedExam;

            if (choice == 1)
            {
                selectedExam = new PracticalExam();
            }
            else if (choice == 2)
            {
                selectedExam = new FinalExam();
            }
            else
            {
                Console.WriteLine("Invalid choice. Exiting...");
                return;
            }

            Console.Write("Enter the number of questions: ");
            int numberOfQuestions = int.Parse(Console.ReadLine());
            selectedExam.NumberOfQuestions = numberOfQuestions;

            List<Question> questions = new List<Question>();

            for (int i = 0; i < numberOfQuestions; i++)
            {
                Question question = new Question();

                Console.Write($"Enter the question header for Question {i + 1}: ");
                question.Header = Console.ReadLine();

                Console.Write($"Enter the question body for Question {i + 1}: ");
                question.Body = Console.ReadLine();

                Console.Write($"Enter the number of answer choices for Question {i + 1}: ");
                int numberOfChoices = int.Parse(Console.ReadLine());

                question.Answers = new List<Answer>();

                for (int j = 0; j < numberOfChoices; j++)
                {
                    Answer answer = new Answer();
                    Console.Write($"Enter answer choice {j + 1}: ");
                    answer.AnswerText = Console.ReadLine();
                    question.Answers.Add(answer);
                }

                Console.Write($"Enter the index of the correct answer (1/2/3/4) for Question {i + 1}: ");
                question.CorrectAnswerIndex = int.Parse(Console.ReadLine()) - 1;

                questions.Add(question);
            }

            if (selectedExam is PracticalExam practicalExam)
            {
                practicalExam.MCQQuestions = questions;
            }
            else if (selectedExam is FinalExam finalExam)
            {
                finalExam.MCQQuestions = questions;
            }

            Subject subject = new Subject
            {
                SubjectId = 1,
                SubjectName = "Mathematics",
                ExamOfSubject = selectedExam
            };

            // Interact with the user to answer questions
            if (selectedExam is PracticalExam || selectedExam is FinalExam)
            {
                foreach (var question in questions)
                {
                    Console.WriteLine(question.Body);
                    for (int i = 0; i < question.Answers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {question.Answers[i].AnswerText}");
                    }
                    Console.Write("Enter your answer (1/2/3/4): ");
                    int userAnswerIndex = int.Parse(Console.ReadLine()) - 1;
                    question.UserAnswerIndex = userAnswerIndex;
                    Console.WriteLine();
                }
            }

            // Display the exam results
            subject.ExamOfSubject.EndTime = DateTime.Now;
            subject.ExamOfSubject.ShowExam();
        }
    }
}




