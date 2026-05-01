using System;
using System.Collections.Generic;
using System.Linq;
using LR1_APPZ.Entities;

namespace LR1_APPZ
{
    // [ПЕРВИННИЙ ВУЗОЛ]: Точка входу в програму та управління консольним інтерфейсом
    class Program
    {
        static List<Teacher> allTeachers = new List<Teacher>();
        static List<StudentGroup> allGroups = new List<StudentGroup>();
        static List<Discipline> allDisciplines = new List<Discipline>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            SeedData();

            // Тут головний цикл програми (Шар 1)
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ГОЛОВНЕ МЕНЮ СИСТЕМИ ===");
                Console.WriteLine("1. Меню Адміністратора (Управління предметами та викладачами)");
                Console.WriteLine("2. Симуляція навчання (Студенти та Пари)");
                Console.WriteLine("0. Вихід");
                Console.Write("\nОберіть режим > ");

                string choice = Console.ReadLine();
                if (choice == "0") break;
                else if (choice == "1") AdminLayer2_SubjectsList();
                else if (choice == "2") SimLayer2_GroupsList();
            }
        }

        // ==========================================
        // РЕЖИМ 1: АДМІНІСТРАТОР (Налаштування)
        // ==========================================

        // ШАР 2 (Адмін): Список предметів
        static void AdminLayer2_SubjectsList()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- [АДМІН] СПИСОК ДИСЦИПЛІН ---");
                for (int i = 0; i < allDisciplines.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {allDisciplines[i].Name} (Фінал: {allDisciplines[i].FinalControl})");
                }
                Console.WriteLine("0. Назад");
                Console.Write("> ");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx == 0) return;
                if (idx > 0 && idx <= allDisciplines.Count) AdminLayer3_EditSubject(allDisciplines[idx - 1]);
            }
        }

        // ШАР 3 (Адмін): Меню редагування конкретного предмета
        static void AdminLayer3_EditSubject(Discipline d)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- [АДМІН] НАЛАШТУВАННЯ: {d.Name} ---");
                Console.WriteLine($"Поточний фінал: {d.FinalControl}");
                Console.WriteLine($"Лектор: {d.MainLecturer?.Name ?? "Не призначено"}");
                Console.WriteLine($"Практики: {d.Practice1?.Name ?? "-"}, {d.Practice2?.Name ?? "-"}");
                Console.WriteLine($"Кількість активностей у курсі: {d.Activities.Count}");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("1. Змінити фінальну активність");
                Console.WriteLine("2. Призначити/Змінити викладачів");
                Console.WriteLine("0. Назад");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") return;
                if (choice == "1") AdminLayer4_ChangeFinalActivity(d);
                if (choice == "2") AdminLayer4_AssignTeachers(d);
            }
        }

        // ШАР 4 (Адмін): Зміна фіналу
        static void AdminLayer4_ChangeFinalActivity(Discipline d)
        {
            Console.Clear();
            if (d.Name == "Основи програмування")
            {
                Console.WriteLine("Для 'Основ програмування' жорстко зафіксовано Залік. Зміна заборонена.");
                Console.ReadLine(); return;
            }

            Console.WriteLine("Оберіть нову фінальну активність:");
            Console.WriteLine("1. Тільки Екзамен");
            Console.WriteLine("2. Екзамен + Курсова робота");
            Console.Write("> ");
            string choice = Console.ReadLine();

            if (choice == "1") d.SetFinalControl(FinalControlType.Exam);
            else if (choice == "2") d.SetFinalControl(FinalControlType.ExamAndCoursework);
        }

        // ШАР 4 (Адмін): Призначення викладачів
        static void AdminLayer4_AssignTeachers(Discipline d)
        {
            Console.Clear();
            var lecturers = allTeachers.Where(t => t.Role == TeacherRole.Lecturer).ToList();
            var practices = allTeachers.Where(t => t.Role == TeacherRole.Practice).ToList();

            Console.WriteLine("Оберіть Лектора (введіть номер):");
            for (int i = 0; i < lecturers.Count; i++) Console.WriteLine($"{i + 1}. {lecturers[i].Name}");
            int lecIdx = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.WriteLine("\nОберіть Практика 1 (введіть номер):");
            for (int i = 0; i < practices.Count; i++) Console.WriteLine($"{i + 1}. {practices[i].Name}");
            int pr1Idx = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.WriteLine("\nОберіть Практика 2 (введіть номер):");
            for (int i = 0; i < practices.Count; i++) Console.WriteLine($"{i + 1}. {practices[i].Name}");
            int pr2Idx = int.Parse(Console.ReadLine() ?? "2") - 1;

            d.AssignTeachers(lecturers[lecIdx], practices[pr1Idx], practices[pr2Idx]);
            Console.WriteLine("\nВикладачів успішно призначено!");
            Console.ReadLine();
        }

        // ==========================================
        // РЕЖИМ 2: СИМУЛЯЦІЯ (Навчання)
        // ==========================================

        // ШАР 2 (Симуляція): Список груп
        static void SimLayer2_GroupsList()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- [СИМУЛЯЦІЯ] СПИСОК ГРУП ---");
                for (int i = 0; i < allGroups.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {allGroups[i].Name} (Студентів: {allGroups[i].StudentsCount})");
                }
                Console.WriteLine("0. Назад");
                Console.Write("> ");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx == 0) return;
                if (idx > 0 && idx <= allGroups.Count) SimLayer3_SubjectsForGroup(allGroups[idx - 1]);
            }
        }

        // ШАР 3 (Симуляція): Вибір предмета
        static void SimLayer3_SubjectsForGroup(StudentGroup group)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- ДИСЦИПЛІНИ ДЛЯ: {group.Name} ---");
                for (int i = 0; i < allDisciplines.Count; i++)
                {
                    string status = group.CompletedDisciplines.Contains(allDisciplines[i].Name) ? "[ВИВЧЕНО]" : "";
                    Console.WriteLine($"{i + 1}. {allDisciplines[i].Name} {status}");
                }
                Console.WriteLine("0. Назад");
                Console.Write("> ");

                if (int.TryParse(Console.ReadLine(), out int dIndex) && dIndex == 0) return;
                if (dIndex > 0 && dIndex <= allDisciplines.Count)
                {
                    var disc = allDisciplines[dIndex - 1];
                    if (!disc.CanBeStudiedBy(group))
                    {
                        Console.WriteLine("\n[ВІДМОВА] Група не може вивчати цей предмет (не той курс або вже вивчено).");
                        Console.ReadLine(); continue;
                    }
                    SimLayer4_Dashboard(group, disc);
                }
            }
        }

        // ШАР 4 (Симуляція): Панель проведення пар та здачі фіналу
        static void SimLayer4_Dashboard(StudentGroup group, Discipline d)
        {
            while (true)
            {
                Console.Clear();
                int subgroups = group.CalculateLabSubgroups();
                Console.WriteLine($"=== ПАНЕЛЬ КЕРУВАННЯ: {group.Name} -> {d.Name} ===");
                Console.WriteLine($"Аудиторних годин: {group.GetHours(d.Name)}/64 | Здано ЛР: {group.GetLabs(d.Name)} (мін 8) | Написано МКР: {group.GetMkr(d.Name)}/2");
                Console.WriteLine($"Підгруп для ЛР: {subgroups} | Статус екзамену: {(group.PassedExams.Contains(d.Name) ? "Складено" : "Не складено")}");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"Лектор: {d.MainLecturer?.Name} [{(d.MainLecturer?.IsBusy == true ? "ЗАЙНЯТИЙ" : "Вільний")}]");
                Console.WriteLine($"Практики: {d.Practice1?.Name} [{(d.Practice1?.IsBusy == true ? "ЗАЙНЯТИЙ" : "Вільний")}], {d.Practice2?.Name} [{(d.Practice2?.IsBusy == true ? "ЗАЙНЯТИЙ" : "Вільний")}]");
                Console.WriteLine("---------------------------------------------------");

                // Динамічна генерація меню з об'єктів Activity
                for (int i = 0; i < d.Activities.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Провести: {d.Activities[i].Name} (+{d.Activities[i].DurationHours} год)");
                }
                Console.WriteLine("8. Звільнити викладачів (завершити всі пари)");
                Console.WriteLine("9. СДАТИ ФІНАЛ (Екзамен/Залік/Курсова)");
                Console.WriteLine("0. Назад");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") return;

                if (choice == "8")
                {
                    d.MainLecturer?.SetBusy(false); d.Practice1?.SetBusy(false); d.Practice2?.SetBusy(false);
                    continue;
                }

                if (choice == "9")
                {
                    bool success = d.TryPassFinal(group, out string msg);
                    Console.WriteLine($"\n[РЕЗУЛЬТАТ]: {msg}");
                    Console.ReadLine();
                    if (group.CompletedDisciplines.Contains(d.Name)) return;
                    continue;
                }

                // Тут алгоритм проведення динамічної активності з перевіркою викладачів
                if (int.TryParse(choice, out int actIdx) && actIdx > 0 && actIdx <= d.Activities.Count)
                {
                    var act = d.Activities[actIdx - 1];

                    if (act.Category == ActivityCategory.Lecture)
                    {
                        if (d.MainLecturer == null || d.MainLecturer.IsBusy) { Console.WriteLine("\nПомилка: Лектор зайнятий."); Console.ReadLine(); continue; }
                        d.MainLecturer.SetBusy(true);
                        group.AddHours(d.Name, act.DurationHours);
                    }
                    else if (act.Category == ActivityCategory.Practice)
                    {
                        if (subgroups == 0) { Console.WriteLine("\nПомилка: Група замала (<10)."); Console.ReadLine(); continue; }
                        if (d.Practice1 == null || d.Practice1.IsBusy || (subgroups == 2 && (d.Practice2 == null || d.Practice2.IsBusy)))
                        { Console.WriteLine("\nПомилка: Немає вільних практиків."); Console.ReadLine(); continue; }

                        d.Practice1.SetBusy(true); if (subgroups == 2) d.Practice2.SetBusy(true);
                        group.AddHours(d.Name, act.DurationHours);
                        group.AddLab(d.Name);
                    }
                    else if (act.Category == ActivityCategory.Control)
                    {
                        if (d.Practice1 == null || d.Practice1.IsBusy || (subgroups == 2 && (d.Practice2 == null || d.Practice2.IsBusy)))
                        { Console.WriteLine("\nПомилка: Немає вільних практиків для контролю."); Console.ReadLine(); continue; }

                        d.Practice1.SetBusy(true); if (subgroups == 2) d.Practice2.SetBusy(true);
                        group.AddHours(d.Name, act.DurationHours);
                        group.AddMkr(d.Name);
                    }

                    Console.WriteLine($"\n'{act.Name}' успішно проведено!"); Console.ReadLine();
                }
            }
        }

        // ==========================================
        // ІНІЦІАЛІЗАЦІЯ ДАНИХ 
        // ==========================================
        static void SeedData()
        {
            allTeachers.Add(new Teacher("Проф. Коваленко (Лектор)", TeacherRole.Lecturer));
            for (int i = 1; i <= 6; i++) allTeachers.Add(new Teacher($"Практик {i}", TeacherRole.Practice));

            allGroups.Add(new StudentGroup("Група 1 (25 осіб)", 1, 25));
            allGroups.Add(new StudentGroup("Група 2 (18 осіб)", 1, 18));
            allGroups.Add(new StudentGroup("Група 3 (9 осіб)", 1, 9));

            var progBasics = new Discipline("Основи програмування", new List<int> { 1 }, FinalControlType.Credit);
            var oop = new Discipline("ООП", new List<int> { 1, 2 }, FinalControlType.Exam);
            var algo = new Discipline("Алгоритми", new List<int> { 2 }, FinalControlType.ExamAndCoursework);

            // [ПРИКЛАД ПРОСТОГО РОЗШИРЕННЯ В КОДІ]: 
            // Якщо завтра знадобиться додати "Семінар" або "Колоквіум", розробнику
            // потрібно лише додати новий рядок `new Activity(...)` у цей список.
            // Меню студента (Шар 4) автоматично згенерує під нього кнопку без зміни логіки інтерфейсу.
            var defaultActivities = new List<Activity> {
                new Activity("Лекція", ActivityCategory.Lecture, 2),
                new Activity("Лабораторна робота", ActivityCategory.Practice, 4),
                new Activity("Модульна Контрольна (МКР)", ActivityCategory.Control, 2)
            };

            progBasics.Activities.AddRange(defaultActivities);
            oop.Activities.AddRange(defaultActivities);
            algo.Activities.AddRange(defaultActivities);

            progBasics.AssignTeachers(allTeachers[0], allTeachers[1], allTeachers[2]);
            oop.AssignTeachers(allTeachers[0], allTeachers[3], allTeachers[4]);
            algo.AssignTeachers(allTeachers[0], allTeachers[5], allTeachers[6]);

            allDisciplines.Add(progBasics); allDisciplines.Add(oop); allDisciplines.Add(algo);

            // ==========================================
            // ДЕМО-ГРУПА ДЛЯ ШВИДКОГО ЗАХИСТУ
            // ==========================================
            var demoGroup = new StudentGroup("Група 0 (Демо-Спідран)", 2, 20); 

            demoGroup.AddHours("Алгоритми", 56); 

            for (int i = 0; i < 7; i++) demoGroup.AddLab("Алгоритми"); 
            demoGroup.AddMkr("Алгоритми"); 

            allGroups.Add(demoGroup);
        }
    }
}