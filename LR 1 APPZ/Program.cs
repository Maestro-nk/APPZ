using System;
using System.Collections.Generic;
using LR_1_APPZ.Entities;

namespace LR_1_APPZ
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // --- 1. ПІДГОТОВКА ДАНИХ (SEED DATA) ---
            var teachers = new List<Teacher>
{
    new Teacher("Іванов І.І."),
    new Teacher("Петров П.П."),
    new Teacher("Сидоров С.С.")
};

            // Створюємо групу 2-го курсу окремо, щоб додати їй історію
            var group4 = new StudentGroup("Група 4 (Алгоритми)", 2, 22);

            // ІМІТАЦІЯ: Група 4 вже вивчила ООП на першому курсі
            group4.MarkDisciplineAsCompleted("ООП");

            var groups = new List<StudentGroup>
{
    new StudentGroup("Група 1 (Ідеальна)", 1, 25),
    new StudentGroup("Група 2 (Мала)", 1, 9),
    new StudentGroup("Група 3 (Випускники)", 3, 20),
    group4,                                          // Наша оновлена група з історією
    new StudentGroup("Група 5 (Середня)", 1, 18)
};

            // isCredit: true = Залік, false = Екзамен/МКР
            var progBasics = new Discipline("Основи програмування", new List<int> { 1 }, 64, true);
            var oop = new Discipline("ООП", new List<int> { 1, 2 }, 72, false);
            var algo = new Discipline("Алгоритми", new List<int> { 2 }, 64, false);

            var pbLec = new Activity("Лекція з Основ", ActivityType.Lecture, 2);
            var pbLab = new Activity("Лаба з Основ", ActivityType.Laboratory, 4);
            pbLec.AssignTeacher(teachers[0], progBasics.Name);
            pbLab.AssignTeacher(teachers[0], progBasics.Name);
            progBasics.AddActivity(pbLec);
            progBasics.AddActivity(pbLab);

            var oopLec = new Activity("Лекція з ООП", ActivityType.Lecture, 2);
            var oopLab = new Activity("Лаба з ООП", ActivityType.Laboratory, 4);
            oopLec.AssignTeacher(teachers[1], oop.Name);
            oopLab.AssignTeacher(teachers[1], oop.Name);
            oop.AddActivity(oopLec);
            oop.AddActivity(oopLab);

            var algoLec = new Activity("Лекція з Алгоритмів", ActivityType.Lecture, 4);
            var algoLab = new Activity("Лаба з Алгоритмів", ActivityType.Laboratory, 4);
            algoLec.AssignTeacher(teachers[2], algo.Name);
            algoLab.AssignTeacher(teachers[2], algo.Name);
            algo.AddActivity(algoLec);
            algo.AddActivity(algoLab);

            var allDisciplines = new List<Discipline> { progBasics, oop, algo };

            // --- 2. ІНТЕРАКТИВНЕ МЕНЮ СИМУЛЯЦІЇ ---
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== АКАДЕМІЧНИЙ СИМУЛЯТОР (Варіант 8) ===");
                Console.WriteLine("КРОК 1: Оберіть групу для симуляції");
                for (int i = 0; i < groups.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {groups[i].Name} (Курс: {groups[i].Course}, Студентів: {groups[i].StudentsCount})");
                }
                Console.WriteLine("0. Вихід");
                Console.Write("> ");

                string groupChoice = Console.ReadLine();
                if (groupChoice == "0") break;

                if (int.TryParse(groupChoice, out int gIndex) && gIndex > 0 && gIndex <= groups.Count)
                {
                    RunDisciplineSelection(groups[gIndex - 1], allDisciplines);
                }
            }
        }

        static void RunDisciplineSelection(StudentGroup group, List<Discipline> disciplines)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- Група: {group.Name} (Курс {group.Course}) ---");
                Console.WriteLine("КРОК 2: Оберіть дисципліну для вивчення");

                for (int i = 0; i < disciplines.Count; i++)
                {
                    string status = group.CompletedDisciplines.Contains(disciplines[i].Name) ? "[ВИВЧЕНО]" : "";
                    Console.WriteLine($"{i + 1}. {disciplines[i].Name} {status}");
                }
                Console.WriteLine("0. Повернутися до вибору групи");
                Console.Write("> ");

                string discChoice = Console.ReadLine();
                if (discChoice == "0") break;

                if (int.TryParse(discChoice, out int dIndex) && dIndex > 0 && dIndex <= disciplines.Count)
                {
                    var selectedDiscipline = disciplines[dIndex - 1];

                    // Перевірка: чи підходить курс і чи не вивчала група це раніше
                    if (!selectedDiscipline.CanBeStudiedBy(group))
                    {
                        if (group.CompletedDisciplines.Contains(selectedDiscipline.Name))
                            Console.WriteLine($"\n[ВІДМОВА] Група {group.Name} вже вивчала дисципліну '{selectedDiscipline.Name}'.");
                        else
                            Console.WriteLine($"\n[ВІДМОВА] Дисципліна '{selectedDiscipline.Name}' недоступна для {group.Course} курсу.");

                        Console.ReadLine();
                        continue;
                    }

                    RunActivityMenu(group, selectedDiscipline);
                }
            }
        }

        static void RunActivityMenu(StudentGroup group, Discipline discipline)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- Вивчення: {discipline.Name} ---");
                Console.WriteLine($"Група: {group.Name} | Підгруп для лаб: {group.CalculateLabSubgroups()}");
                Console.WriteLine($"Прогрес годин: {group.TotalStudiedHours} / {discipline.TargetHours}");
                Console.WriteLine($"Здано лабораторних: {group.CompletedPracticalTasks}");
                Console.WriteLine("----------------------------------");

                for (int i = 0; i < discipline.Activities.Count; i++)
                {
                    var act = discipline.Activities[i];
                    Console.WriteLine($"{i + 1}. Провести: {act.Title} ({act.Duration} год) - Викладач: {act.AssignedTeacher.Name} [{(act.AssignedTeacher.IsBusy ? "ЗАЙНЯТИЙ" : "ВІЛЬНИЙ")}]");
                }
                Console.WriteLine("8. Завершити всі поточні пари (Звільнити викладачів)");
                Console.WriteLine("9. Спроба здати МКР / Екзамен / Залік");
                Console.WriteLine("0. Повернутися до вибору дисципліни");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                if (choice == "8")
                {
                    foreach (var act in discipline.Activities) act.AssignedTeacher?.FinishClass();
                    Console.WriteLine("\n[ІНФО] Усі викладачі цієї дисципліни тепер ВІЛЬНІ.");
                    Console.ReadLine();
                    continue;
                }

                if (choice == "9")
                {
                    bool success = discipline.ConductFinalAssessment(group, out string msg);
                    Console.WriteLine($"\n[ФІНАЛЬНИЙ КОНТРОЛЬ]: {(success ? "УСПІХ" : "ВІДМОВА")} - {msg}");
                    if (success)
                    {
                        Console.WriteLine("Дисципліну закрито. Натисніть Enter для виходу...");
                        Console.ReadLine();
                        break; // Виходимо з меню предмета, бо він зданий
                    }
                    Console.ReadLine();
                    continue;
                }

                if (int.TryParse(choice, out int actIndex) && actIndex > 0 && actIndex <= discipline.Activities.Count)
                {
                    var selectedActivity = discipline.Activities[actIndex - 1];

                    if (selectedActivity.CanStart(group, discipline, out string error))
                    {
                        selectedActivity.AssignedTeacher.StartClass();
                        selectedActivity.Conduct(group);
                        Console.WriteLine($"\n[УСПІХ] {selectedActivity.Title} розпочато! Викладач {selectedActivity.AssignedTeacher.Name} тепер ЗАЙНЯТИЙ.");
                    }
                    else
                    {
                        Console.WriteLine($"\n[ПОМИЛКА] Неможливо провести {selectedActivity.Title}: {error}");
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}