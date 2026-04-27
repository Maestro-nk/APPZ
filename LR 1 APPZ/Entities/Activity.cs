using System;

namespace LR_1_APPZ.Entities
{
    public enum ActivityType { Lecture, Laboratory, Exam, Credit }

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

        // Перевірка ВСІХ умов (включно із зайнятістю викладача)
        public bool CanStart(StudentGroup group, Discipline discipline, out string error)
        {
            if (AssignedTeacher == null) { error = "No teacher assigned."; return false; }
            if (AssignedTeacher.IsBusy) { error = $"Teacher {AssignedTeacher.Name} is currently BUSY with another group!"; return false; }
            if (Type == ActivityType.Laboratory && group.CalculateLabSubgroups() == 0) { error = "Group is too small for labs (min 10)."; return false; }
            if (group.TotalStudiedHours + Duration > discipline.TargetHours) { error = $"Exceeds total discipline hours ({discipline.TargetHours})."; return false; }

            error = "OK";
            return true;
        }

        public void Conduct(StudentGroup group)
        {
            if (Type == ActivityType.Laboratory) group.CompletePracticalTask();
            group.AddStudiedHours(Duration);
        }
    }
}