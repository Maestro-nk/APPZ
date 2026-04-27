using System;

namespace LR_1_APPZ.Entities
{
    public class Teacher
    {
        public string Name { get; private set; }
        public string CurrentDiscipline { get; private set; }
        public bool IsBusy { get; private set; } // Захист від подвійного бронювання

        public Teacher(string name)
        {
            Name = name;
            CurrentDiscipline = null;
            IsBusy = false;
        }

        public bool AssignDiscipline(string disciplineName)
        {
            if (CurrentDiscipline != null && CurrentDiscipline != disciplineName) return false;
            CurrentDiscipline = disciplineName;
            return true;
        }

        public bool StartClass()
        {
            if (IsBusy) return false;
            IsBusy = true;
            return true;
        }

        public void FinishClass() => IsBusy = false;
    }
}