using System;
using System.Collections.Generic;

namespace LR1._2.Entities
{
    public class StudentGroup
    {
        public string Name { get; private set; }
        public int Course { get; private set; }
        public int StudentsCount { get; private set; }
        public int CompletedPracticalTasks { get; private set; }
        public int TotalStudiedHours { get; private set; }

        // Я додав список для збереження предметів, які група вже успішно склала
        public List<string> CompletedDisciplines { get; private set; }

        public StudentGroup(string name, int course, int studentsCount, int initialHours = 0)
        {
            Name = name;
            Course = course;
            StudentsCount = studentsCount;
            CompletedPracticalTasks = 0;
            TotalStudiedHours = initialHours;
            CompletedDisciplines = new List<string>();
        }

        public void CompletePracticalTask() => CompletedPracticalTasks++;
        public void AddStudiedHours(int hours) => TotalStudiedHours += hours;

        // Фіксація успішного вивчення дисципліни
        public void MarkDisciplineAsCompleted(string disciplineName)
        {
            if (!CompletedDisciplines.Contains(disciplineName))
                CompletedDisciplines.Add(disciplineName);
        }

        // Моя логіка розрахунку підгруп: 1 група = 2 підгрупи по 50%, але не менше 10 осіб у кожній
        public int CalculateLabSubgroups()
        {
            if (StudentsCount < 10) return 0; // Якщо загалом менше 10, лаби проводити не можна

            int half = StudentsCount / 2; // Ділимо групу навпіл
            if (half >= 10)
            {
                return 2; // Якщо половина це 10 або більше студентів - робимо 2 підгрупи
            }

            return 1; // Якщо половина менше 10 (наприклад, група 18 осіб) - залишаємо 1 підгрупу
        }
    }
}