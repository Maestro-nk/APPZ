using System;
using System.Collections.Generic;

namespace LR_1_APPZ.Entities
{
    public class Discipline
    {
        public string Name { get; private set; }
        public List<int> AllowedCourses { get; private set; }
        public int TargetHours { get; private set; }
        public List<Activity> Activities { get; private set; }

        // Я використовую цей прапорець, щоб визначити тип фінального контролю (Залік чи Екзамен/МКР)
        public bool IsCredit { get; private set; }

        public Discipline(string name, List<int> allowedCourses, int targetHours, bool isCredit)
        {
            Name = name;
            AllowedCourses = allowedCourses;
            TargetHours = targetHours < 64 ? 64 : targetHours; // Захист мінімальних годин за ТЗ
            Activities = new List<Activity>();
            IsCredit = isCredit;
        }

        public void AddActivity(Activity activity) => Activities.Add(activity);

        // Тут я додав перевірку: дисципліну можна вивчати тільки один раз!
        public bool CanBeStudiedBy(StudentGroup group)
        {
            bool isAllowedCourse = AllowedCourses.Contains(group.Course);
            bool isAlreadyCompleted = group.CompletedDisciplines.Contains(Name);

            return isAllowedCourse && !isAlreadyCompleted;
        }

        // Логіка допуску до МКР/Екзамену та Залік "автоматом"
        public bool ConductFinalAssessment(StudentGroup group, out string message)
        {
            // Якщо це не залік (екзамен або МКР), то потрібні здані роботи
            if (!IsCredit && group.CompletedPracticalTasks < 1)
            {
                message = "Без зданих лабораторних робіт допуск до МКР/Екзамену заборонено!";
                return false;
            }

            message = IsCredit ? "Залік виставлено автоматом!" : "МКР/Екзамен успішно складено!";
            group.MarkDisciplineAsCompleted(Name); // Фіксуємо, що група пройшла предмет
            return true;
        }
    }
}