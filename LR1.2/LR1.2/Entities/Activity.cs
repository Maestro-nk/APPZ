using System;

namespace LR1._2.Entities
{
    // Я додав МКР (Модульну контрольну роботу) до переліку активностей
    public enum ActivityType { Lecture, Laboratory, Exam, Credit, ModularControl }

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

        // Guard Clause: я перевіряю всі умови до того, як дозволити проведення заняття
        public bool CanStart(StudentGroup group, Discipline discipline, out string error)
        {
            if (AssignedTeacher == null) { error = "Викладача не призначено."; return false; }
            if (AssignedTeacher.IsBusy) { error = $"Викладач {AssignedTeacher.Name} зараз веде пару в іншої групи!"; return false; }
            if (Type == ActivityType.Laboratory && group.CalculateLabSubgroups() == 0) { error = "Група занадто мала для лабораторних (менше 10 осіб)."; return false; }
            if (group.TotalStudiedHours + Duration > discipline.TargetHours) { error = $"Перевищено ліміт годин дисципліни ({discipline.TargetHours} год)."; return false; }

            error = "ОК";
            return true;
        }

        public void Conduct(StudentGroup group)
        {
            if (Type == ActivityType.Laboratory) group.CompletePracticalTask();
            group.AddStudiedHours(Duration);
        }
    }
}