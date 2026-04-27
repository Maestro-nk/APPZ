using System.Collections.Generic;

namespace LR1_APPZ.Entities
{
    // [ВТОРИННИЙ ВУЗОЛ]: Типи фінального контролю
    public enum FinalControlType { Credit, Exam, ExamAndCoursework }

    // [ПЕРВИННИЙ ВУЗОЛ]: Головний клас Дисципліни (акумулює викладачів, активності та логіку допусків)
    public class Discipline
    {
        public string Name { get; private set; }
        public List<int> AllowedCourses { get; private set; }
        public FinalControlType FinalControl { get; private set; }

        // Тут список об'єктів активностей (Лекції, Лаби тощо)
        public List<Activity> Activities { get; private set; } = new List<Activity>();

        // Тут жорстко закріплені ролі викладачів (ТЗ)
        public Teacher MainLecturer { get; private set; }
        public Teacher Practice1 { get; private set; }
        public Teacher Practice2 { get; private set; }

        // Тут конструктор дисципліни
        public Discipline(string name, List<int> allowedCourses, FinalControlType defaultControl)
        {
            Name = name;
            AllowedCourses = allowedCourses;
            FinalControl = defaultControl;
        }

        public void AddActivity(Activity activity) => Activities.Add(activity);
        public void SetFinalControl(FinalControlType controlType) => FinalControl = controlType;

        // Тут метод закріплення викладачів за предметом
        public void AssignTeachers(Teacher lecturer, Teacher pr1, Teacher pr2)
        {
            MainLecturer = lecturer;
            Practice1 = pr1;
            Practice2 = pr2;
        }

        public bool CanBeStudiedBy(StudentGroup group)
        {
            return AllowedCourses.Contains(group.Course) && !group.CompletedDisciplines.Contains(Name);
        }

        // Тут алгоритм перевірки допусків до фінального контролю (64 год, 8 ЛР, 2 МКР)
        public bool TryPassFinal(StudentGroup group, out string message)
        {
            int hours = group.GetHours(Name);
            int mkr = group.GetMkr(Name);
            int labs = group.GetLabs(Name);

            // Базові перевірки, обов'язкові для всіх (навіть для Заліку)
            if (hours < 64) { message = $"Недостатньо аудиторних годин ({hours}/64)."; return false; }
            if (mkr < 2) { message = $"Необхідно скласти 2 МКР. Складено: {mkr}."; return false; }

            // Логіка Заліку
            if (FinalControl == FinalControlType.Credit)
            {
                message = "Залік успішно отримано! Дисципліну закрито.";
                group.CompletedDisciplines.Add(Name);
                return true;
            }

            // Блокування доступу до Екзамену без лабораторних
            if (labs < 8) { message = $"Необхідно мінімум 8 ЛР для екзамену. Здано: {labs}."; return false; }

            // Логіка простого Екзамену
            if (FinalControl == FinalControlType.Exam)
            {
                message = "Екзамен успішно складено! Дисципліну закрито.";
                group.CompletedDisciplines.Add(Name);
                return true;
            }

            // Логіка багатоетапного фіналу (Екзамен + Курсова)
            if (FinalControl == FinalControlType.ExamAndCoursework)
            {
                if (!group.PassedExams.Contains(Name))
                {
                    message = "Екзамен успішно складено! Тепер необхідно захистити Курсову роботу.";
                    group.PassedExams.Add(Name);
                    return true;
                }
                else
                {
                    message = "Курсову роботу успішно захищено! Дисципліну закрито.";
                    group.CompletedDisciplines.Add(Name);
                    return true;
                }
            }

            message = "Невідома конфігурація предмета.";
            return false;
        }
    }
}