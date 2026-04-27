using System;
using System.Collections.Generic;

namespace LR1._2.Entities
{
    public class Discipline
    {
        public string Name { get; private set; }
        public List<int> AllowedCourses { get; private set; }
        public int TargetHours { get; private set; }
        public List<Activity> Activities { get; private set; }

        // Вказує, що вимагає предмет: true = Екзамен, false = Залік
        public bool IsExamBased { get; private set; }

        public Discipline(string name, List<int> allowedCourses, int targetHours, bool isExamBased)
        {
            Name = name;
            AllowedCourses = allowedCourses;
            TargetHours = targetHours < 64 ? 64 : targetHours;
            Activities = new List<Activity>();
            IsExamBased = isExamBased;
        }

        public void AddActivity(Activity activity) => Activities.Add(activity);

        public bool CanBeStudiedBy(StudentGroup group)
        {
            return AllowedCourses.Contains(group.Course) && !group.CompletedDisciplines.Contains(Name);
        }

        // ПАТЕРН СТРАТЕГІЯ: Метод приймає обрану користувачем стратегію і просто виконує її
        public bool ConductAssessment(IAssessmentStrategy strategy, StudentGroup group, out string message)
        {
            return strategy.Execute(group, this, out message);
        }
    }
}