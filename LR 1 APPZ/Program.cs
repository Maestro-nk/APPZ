using System;
using System.Collections.Generic;
using LR_1_APPZ.Entities;

namespace LR_1_APPZ
{
    class Program
    {
        static void Main(string[] args)
        {
            // --- 1. SEED DATA ---
            var teachers = new List<Teacher>
            {
                new Teacher("Ivanov I.I."),
                new Teacher("Petrov P.P."),
                new Teacher("Sidorov S.S.")
            };

            var groups = new List<StudentGroup>
            {
                new StudentGroup("Group 1-Normal", 1, 25),
                new StudentGroup("Group 2-Small", 1, 9),
                new StudentGroup("Group 3-Grads", 3, 20),
                new StudentGroup("Group 4-Algo", 2, 22)
            };

            var progBasics = new Discipline("Programming Basics", new List<int> { 1 }, 64, false, true);
            var oop = new Discipline("OOP", new List<int> { 1, 2 }, 72, true, false);
            var algo = new Discipline("Algorithms", new List<int> { 2 }, 64, true, false);

            var pbLec = new Activity("Prog Basics Lecture", ActivityType.Lecture, 2);
            var pbLab = new Activity("Prog Basics Lab", ActivityType.Laboratory, 4);
            pbLec.AssignTeacher(teachers[0], progBasics.Name);
            pbLab.AssignTeacher(teachers[0], progBasics.Name);
            progBasics.AddActivity(pbLec);
            progBasics.AddActivity(pbLab);

            var oopLec = new Activity("OOP Lecture", ActivityType.Lecture, 2);
            var oopLab = new Activity("OOP Lab", ActivityType.Laboratory, 4);
            oopLec.AssignTeacher(teachers[1], oop.Name);
            oopLab.AssignTeacher(teachers[1], oop.Name);
            oop.AddActivity(oopLec);
            oop.AddActivity(oopLab);

            var algoLec = new Activity("Algorithms Lecture", ActivityType.Lecture, 4);
            var algoLab = new Activity("Algorithms Lab", ActivityType.Laboratory, 4);
            algoLec.AssignTeacher(teachers[2], algo.Name);
            algoLab.AssignTeacher(teachers[2], algo.Name);
            algo.AddActivity(algoLec);
            algo.AddActivity(algoLab);

            var allDisciplines = new List<Discipline> { progBasics, oop, algo };

            // --- 2. INTERACTIVE SIMULATION MENU ---
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ACADEMIC SIMULATOR (Variant 8) ===");
                Console.WriteLine("STEP 1: Select a group for simulation");
                for (int i = 0; i < groups.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {groups[i].Name} (Year: {groups[i].Course}, Students: {groups[i].StudentsCount})");
                }
                Console.WriteLine("0. Exit");
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
                Console.WriteLine($"--- Group: {group.Name} (Year {group.Course}) ---");
                Console.WriteLine("STEP 2: Select a discipline to study");

                for (int i = 0; i < disciplines.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {disciplines[i].Name} (Target Hours: {disciplines[i].TargetHours})");
                }
                Console.WriteLine("0. Back to Group Selection");
                Console.Write("> ");

                string discChoice = Console.ReadLine();
                if (discChoice == "0") break;

                if (int.TryParse(discChoice, out int dIndex) && dIndex > 0 && dIndex <= disciplines.Count)
                {
                    var selectedDiscipline = disciplines[dIndex - 1];

                    // Відновлено перевірку курсу
                    if (!selectedDiscipline.CanBeStudiedBy(group))
                    {
                        Console.WriteLine($"\n[ACCESS DENIED] {group.Name} is on year {group.Course}. '{selectedDiscipline.Name}' is not allowed.");
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
                Console.WriteLine($"--- Studying: {discipline.Name} ---");
                Console.WriteLine($"Group: {group.Name}");
                Console.WriteLine($"Progress: {group.TotalStudiedHours} / {discipline.TargetHours} hours");
                Console.WriteLine($"Completed Labs: {group.CompletedPracticalTasks}");
                Console.WriteLine("----------------------------------");

                for (int i = 0; i < discipline.Activities.Count; i++)
                {
                    var act = discipline.Activities[i];
                    Console.WriteLine($"{i + 1}. Conduct {act.Title} ({act.Duration}h) - Teacher: {act.AssignedTeacher.Name} [{(act.AssignedTeacher.IsBusy ? "BUSY" : "FREE")}]");
                }
                Console.WriteLine("8. Finish all current classes (Free up teachers)");
                Console.WriteLine("9. Attempt Final Exam / Credit");
                Console.WriteLine("0. Back to Discipline Selection");
                Console.Write("> ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                // Звільнення викладачів після пари
                if (choice == "8")
                {
                    foreach (var act in discipline.Activities) act.AssignedTeacher?.FinishClass();
                    Console.WriteLine("\n[INFO] All teachers for this discipline are now FREE.");
                    Console.ReadLine();
                    continue;
                }

                if (choice == "9")
                {
                    bool success = discipline.ConductFinalAssessment(group, out string msg);
                    Console.WriteLine($"\n[FINAL ASSESSMENT]: {(success ? "PASSED" : "FAILED")} - {msg}");
                    Console.ReadLine();
                    continue;
                }

                if (int.TryParse(choice, out int actIndex) && actIndex > 0 && actIndex <= discipline.Activities.Count)
                {
                    var selectedActivity = discipline.Activities[actIndex - 1];

                    if (selectedActivity.CanStart(group, discipline, out string error))
                    {
                        selectedActivity.AssignedTeacher.StartClass(); // Займаємо викладача
                        selectedActivity.Conduct(group);
                        Console.WriteLine($"\n[SUCCESS] {selectedActivity.Title} started and conducted! Teacher {selectedActivity.AssignedTeacher.Name} is now BUSY.");
                    }
                    else
                    {
                        Console.WriteLine($"\n[ERROR] Cannot conduct {selectedActivity.Title}: {error}");
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}