using System;
using System.Collections.Generic;

namespace LR_1_APPZ.Entities
{
    public class Discipline
    {
        public string Name { get; private set; }
        public List<int> AllowedCourses { get; private set; }
        public int TargetHours { get; private set; } // План годин (мінімум 64)
        public List<Activity> Activities { get; private set; }

        public bool HasExam { get; private set; }
        public bool HasCredit { get; private set; }

        public Discipline(string name, List<int> allowedCourses, int targetHours, bool hasExam, bool hasCredit)
        {
            Name = name;
            AllowedCourses = allowedCourses;
            // Валідація згідно ТЗ: не менше 64 годин
            TargetHours = targetHours < 64 ? 64 : targetHours;
            Activities = new List<Activity>();
            HasExam = hasExam;
            HasCredit = hasCredit;
        }

        public void AddActivity(Activity activity)
        {
            Activities.Add(activity);
        }

        // Перевірка відповідності курсу
        public bool CanBeStudiedBy(StudentGroup group)
        {
            return AllowedCourses.Contains(group.Course);
        }

        // Фінальне оцінювання
        public bool ConductFinalAssessment(StudentGroup group, out string message)
        {
            // Для екзамену обов'язкові здані лаби (мінімум 1 для демо)
            if (HasExam && group.CompletedPracticalTasks < 1)
            {
                message = "Need at least 1 completed lab for Exam.";
                return false;
            }

            // Залік автоматом або екзамен здано успішно
            message = HasCredit ? "Credit achieved automatically!" : "Exam passed successfully!";
            return true;
        }
    }
}