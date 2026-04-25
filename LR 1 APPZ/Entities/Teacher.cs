using System;

namespace LR_1_APPZ.Entities
{
    public class Teacher
    {
        public string Name { get; private set; }
        public string CurrentDiscipline { get; private set; }

        public Teacher(string name)
        {
            Name = name;
            CurrentDiscipline = null;
        }

        // Перевірка обмеження: один викладач = одна дисципліна в момент часу
        public bool AssignDiscipline(string disciplineName)
        {
            if (CurrentDiscipline != null && CurrentDiscipline != disciplineName)
            {
                return false;
            }
            CurrentDiscipline = disciplineName;
            return true;
        }

        public void FinishTeaching()
        {
            CurrentDiscipline = null;
        }
    }
}