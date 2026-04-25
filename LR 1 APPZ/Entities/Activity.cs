using System;

namespace LR_1_APPZ.Entities
{
    public enum ActivityType
    {
        Lecture,
        Laboratory,
        Exam,
        Credit
    }

    public class Activity
    {
        public string Title { get; private set; }
        public ActivityType Type { get; private set; }
        public Teacher AssignedTeacher { get; private set; }
        public int Duration { get; private set; }

        public Activity(string title, ActivityType type, int duration)
        {
            Title = title;
            Type = type;
            Duration = duration;
        }

        public bool AssignTeacher(Teacher teacher, string disciplineName)
        {
            if (teacher.AssignDiscipline(disciplineName))
            {
                AssignedTeacher = teacher;
                return true;
            }
            return false;
        }

        // Guard Clause: Перевірка ВСІХ умов ПЕРЕД тим, як дозволити заняття
        public bool CanBeConductedFor(StudentGroup group, Discipline discipline, out string errorMessage)
        {
            if (AssignedTeacher == null)
            {
                errorMessage = "No teacher assigned to this activity.";
                return false;
            }

            // Перевірка кількості осіб для лабораторних (Варіант 8)
            if (Type == ActivityType.Laboratory && group.CalculateLabSubgroups() == 0)
            {
                errorMessage = "Group is too small for lab subgroups (min 10 students).";
                return false;
            }

            // Перевірка ліміту годин дисципліни
            if (group.TotalStudiedHours + Duration > discipline.TargetHours)
            {
                errorMessage = $"Exceeds total discipline hours ({discipline.TargetHours}).";
                return false;
            }

            errorMessage = "OK";
            return true;
        }

        // Сама симуляція процесу
        public void Conduct(StudentGroup group)
        {
            if (Type == ActivityType.Laboratory)
            {
                group.CompletePracticalTask();
            }

            group.AddStudiedHours(Duration);
        }
    }
}