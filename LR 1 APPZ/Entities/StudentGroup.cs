using System;

namespace LR_1_APPZ.Entities
{
    public class StudentGroup
    {
        public string Name { get; private set; }
        public int Course { get; private set; }
        public int StudentsCount { get; private set; }

        // Стан об'єкта під час симуляції
        public int CompletedPracticalTasks { get; private set; }
        public int TotalStudiedHours { get; private set; }

        public StudentGroup(string name, int course, int studentsCount, int initialHours = 0)
        {
            Name = name;
            Course = course;
            StudentsCount = studentsCount;
            CompletedPracticalTasks = 0;
            TotalStudiedHours = initialHours; // Можливість задати години для демо
        }

        public void CompletePracticalTask()
        {
            CompletedPracticalTasks++;
        }

        public void AddStudiedHours(int hours)
        {
            TotalStudiedHours += hours;
        }

        // Перевірка обмеження підгруп (мінімум 10 осіб)
        public int CalculateLabSubgroups()
        {
            if (StudentsCount < 10) return 0;
            return StudentsCount >= 20 ? 2 : 1;
        }
    }
}