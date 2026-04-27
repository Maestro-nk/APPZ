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
        public bool HasExam { get; private set; }
        public bool HasCredit { get; private set; }

        public Discipline(string name, List<int> allowedCourses, int targetHours, bool hasExam, bool hasCredit)
        {
            Name = name;
            AllowedCourses = allowedCourses;
            TargetHours = targetHours < 64 ? 64 : targetHours;
            Activities = new List<Activity>();
            HasExam = hasExam;
            HasCredit = hasCredit;
        }

        public void AddActivity(Activity activity) => Activities.Add(activity);
        public bool CanBeStudiedBy(StudentGroup group) => AllowedCourses.Contains(group.Course);

        public bool ConductFinalAssessment(StudentGroup group, out string message)
        {
            if (HasExam && group.CompletedPracticalTasks < 1)
            {
                message = "Need at least 1 completed lab for Exam.";
                return false;
            }
            message = HasCredit ? "Credit achieved automatically!" : "Exam passed successfully!";
            return true;
        }
    }
}