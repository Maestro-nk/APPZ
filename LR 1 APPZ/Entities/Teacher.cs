using System;

namespace LR_1_APPZ.Entities
{
    public class Teacher
    {
        public string Name { get; private set; }
        public string CurrentDiscipline { get; private set; }

        // Я додав цей прапорець, щоб викладач не міг вести дві пари одночасно (захист від стану гонки)
        public bool IsBusy { get; private set; }

        public Teacher(string name)
        {
            Name = name;
            CurrentDiscipline = null;
            IsBusy = false;
        }

        // Тут я перевіряю, чи не призначений викладач на іншу дисципліну (вимога варіанту 8)
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