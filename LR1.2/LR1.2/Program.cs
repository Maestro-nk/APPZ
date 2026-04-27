using System;
using System.Collections.Generic;
using LR1._2.Entities;

namespace LR1._2.Entities
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var teachers = new List<Teacher> { new Teacher("Іванов І.І."), new Teacher("Петров П.П."), new Teacher("Сидоров С.С.") };
            var group4 = new StudentGroup("Група 4 (Алгоритми)", 2, 22);
            group4.MarkDisciplineAsCompleted("ООП");

            var groups = new List<StudentGroup> {
                new StudentGroup("Група 1 (Ідеальна)", 1, 25), new StudentGroup("Група 2 (Мала)", 1, 9),
                new StudentGroup("Група 3 (Випускники)", 3, 20), group4, new StudentGroup("Група 5 (Середня)", 1, 18)
            };

            // true = Екзамен, false = Залік
            var progBasics = new Discipline("Основи програмування", new List<int> { 1 }, 64, false);
            var oop = new Discipline("ООП", new List<int> { 1, 2 }, 72, true);
            var algo = new Discipline("Алгоритми", new List<int> { 2 }, 64, true);

            var pbLec = ActivityFactory.CreateLecture("Лекція з Основ"); var pbLab = ActivityFactory.CreateLab("Лаба з Основ");
            pbLec.AssignTeacher(teachers[0], progBasics.Name); pbLab.AssignTeacher(teachers[0], progBasics.Name);
            progBasics.AddActivity(pbLec); progBasics.AddActivity(pbLab);

            var oopLec = ActivityFactory.CreateLecture("Лекція з ООП"); var oopLab = ActivityFactory.CreateLab("Лаба з ООП");
            oopLec.AssignTeacher(teachers[1], oop.Name); oopLab.AssignTeacher(teachers[1], oop.Name);
            oop.AddActivity(oopLec); oop.AddActivity(oopLab);

            var algoLec = ActivityFactory.CreateLecture("Лекція з Алгоритмів"); var algoLab = ActivityFactory.CreateLab("Лаба з Алгоритмів");
            algoLec.AssignTeacher(teachers[2], algo.Name); algoLab.AssignTeacher(teachers[2], algo.Name);
            algo.AddActivity(algoLec); algo.AddActivity(algoLab);

            var allDisciplines = new List<Discipline> { progBasics, oop, algo };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== АКАДЕМІЧНИЙ СИМУЛЯТОР (Варіант 8) ===");
                Console.WriteLine("КРОК 1: Оберіть групу для симуляції");
                for (int i = 0; i < groups.Count; i++) Console.WriteLine($"{i + 1}. {groups[i].Name} (Курс: {groups[i].Course}, Студентів: {groups[i].StudentsCount})");
                Console.WriteLine("0. Вихід");
                Console.Write("> ");

                string groupChoice = Console.ReadLine();
                if (groupChoice == "0") break;
                if (int.TryParse(groupChoice, out int gIndex) && gIndex > 0 && gIndex <= groups.Count)
                    RunDisciplineSelection(groups[gIndex - 1], allDisciplines);
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

                    if (!selectedDiscipline.CanBeStudiedBy(group))
                    {
                        if (group.CompletedDisciplines.Contains(selectedDiscipline.Name))
                            Console.WriteLine($"\n[ВІДМОВА] Група {group.Name} вже вивчала дисципліну '{selectedDiscipline.Name}'.");
                        else Console.WriteLine($"\n[ВІДМОВА] Дисципліна '{selectedDiscipline.Name}' недоступна для {group.Course} курсу.");
                        Console.ReadLine(); continue;
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
                Console.WriteLine("9. Відкрити меню оцінювання (МКР / Екзамен / Залік)");
                Console.WriteLine("0. Повернутися до вибору дисципліни");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                if (choice == "8")
                {
                    foreach (var act in discipline.Activities) act.AssignedTeacher?.FinishClass();
                    Console.WriteLine("\n[ІНФО] Усі викладачі цієї дисципліни тепер ВІЛЬНІ.");
                    Console.ReadLine(); continue;
                }

                if (choice == "9")
                {
                    // Відкриваємо нове меню! Якщо предмет здано успішно (true) - виходимо з цього циклу
                    bool isDisciplineFinished = RunAssessmentMenu(group, discipline);
                    if (isDisciplineFinished) break;
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
                    else Console.WriteLine($"\n[ПОМИЛКА] Неможливо провести {selectedActivity.Title}: {error}");
                    Console.ReadLine();
                }
            }
        }

        // НОВИЙ МЕТОД: Підменю вибору стратегії оцінювання
        static bool RunAssessmentMenu(StudentGroup group, Discipline discipline)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- Меню оцінювання: {discipline.Name} ---");
                Console.WriteLine("Оберіть тип контролю:");
                Console.WriteLine("1. Написати МКР (Модульна контрольна робота)");
                Console.WriteLine("2. Отримати Залік");
                Console.WriteLine("3. Здати Екзамен");
                Console.WriteLine("0. Повернутися до занять");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") return false; // Предмет не закрито

                IAssessmentStrategy strategy = null;
                bool isFinal = false;

                switch (choice)
                {
                    case "1": strategy = new MkrStrategy(); break;
                    case "2": strategy = new CreditStrategy(); isFinal = true; break;
                    case "3": strategy = new ExamStrategy(); isFinal = true; break;
                    default: continue;
                }

                // Передаємо обрану стратегію в дисципліну
                bool success = discipline.ConductAssessment(strategy, group, out string msg);
                Console.WriteLine($"\n[РЕЗУЛЬТАТ]: {(success ? "УСПІХ" : "ВІДМОВА")} - {msg}");

                if (success && isFinal)
                {
                    Console.WriteLine("Дисципліну закрито. Натисніть Enter для виходу...");
                    Console.ReadLine();
                    return true; // Предмет вивчено, кажемо батьківському меню закритися
                }

                Console.ReadLine();
            }
        }
    }
}