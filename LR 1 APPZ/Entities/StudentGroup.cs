using System.Collections.Generic;

namespace LR1_APPZ.Entities
{
    // [ПЕРВИННИЙ ВУЗОЛ]: Сутність студентської групи та її "Залікова книжка"
    public class StudentGroup
    {
        public string Name { get; private set; }
        public int Course { get; private set; }
        public int StudentsCount { get; private set; }

        // Тут словники для зберігання прогресу ізольовано для кожної дисципліни
        public Dictionary<string, int> HoursPerDiscipline { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> LabsPerDiscipline { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MkrPerDiscipline { get; private set; } = new Dictionary<string, int>();

        public List<string> PassedExams { get; private set; } = new List<string>();
        public List<string> CompletedDisciplines { get; private set; } = new List<string>();

        // Тут конструктор студентської групи
        public StudentGroup(string name, int course, int studentsCount)
        {
            Name = name;
            Course = course;
            StudentsCount = studentsCount;
        }

        // Тут алгоритм розрахунку підгруп (якщо менше 10 людей - лаби не проводяться)
        public int CalculateLabSubgroups()
        {
            if (StudentsCount < 10) return 0;
            return (StudentsCount / 2) >= 10 ? 2 : 1;
        }

        // Тут методи безпечного нарахування прогресу (Студент відвідав заняття)
        public void AddHours(string discipline, int hours)
        {
            if (!HoursPerDiscipline.ContainsKey(discipline)) HoursPerDiscipline[discipline] = 0;
            HoursPerDiscipline[discipline] += hours;
        }

        public void AddLab(string discipline)
        {
            if (!LabsPerDiscipline.ContainsKey(discipline)) LabsPerDiscipline[discipline] = 0;
            LabsPerDiscipline[discipline]++;
        }

        public void AddMkr(string discipline)
        {
            if (!MkrPerDiscipline.ContainsKey(discipline)) MkrPerDiscipline[discipline] = 0;
            MkrPerDiscipline[discipline]++;
        }

        // Тут геттери для отримання статистики групи по конкретному предмету
        public int GetHours(string d) => HoursPerDiscipline.ContainsKey(d) ? HoursPerDiscipline[d] : 0;
        public int GetLabs(string d) => LabsPerDiscipline.ContainsKey(d) ? LabsPerDiscipline[d] : 0;
        public int GetMkr(string d) => MkrPerDiscipline.ContainsKey(d) ? MkrPerDiscipline[d] : 0;
    }
}