using System;

namespace LR1._2.Entities
{
    // Інтерфейс стратегії
    public interface IAssessmentStrategy
    {
        bool Execute(StudentGroup group, Discipline discipline, out string message);
    }

    // Стратегія 1: МКР (Проміжний контроль, вимагає лаб, але не закриває предмет)
    public class MkrStrategy : IAssessmentStrategy
    {
        public bool Execute(StudentGroup group, Discipline discipline, out string message)
        {
            if (group.CompletedPracticalTasks < 1)
            {
                message = "Без зданих лабораторних робіт допуск до МКР заборонено!";
                return false;
            }
            message = "МКР успішно написано! (Проміжний контроль пройдено)";
            return true;
        }
    }

    // Стратегія 2: Залік (Автомат, але перевіряє чи цей предмет взагалі має залік)
    public class CreditStrategy : IAssessmentStrategy
    {
        public bool Execute(StudentGroup group, Discipline discipline, out string message)
        {
            if (discipline.IsExamBased)
            {
                message = "ПОМИЛКА: Ця дисципліна вимагає здачі екзамену, а не заліку!";
                return false;
            }

            message = "Залік виставлено автоматом!";
            group.MarkDisciplineAsCompleted(discipline.Name); // Закриваємо предмет
            return true;
        }
    }

    // Стратегія 3: Екзамен (Вимагає лаб і правильного типу предмета)
    public class ExamStrategy : IAssessmentStrategy
    {
        public bool Execute(StudentGroup group, Discipline discipline, out string message)
        {
            if (!discipline.IsExamBased)
            {
                message = "ПОМИЛКА: Ця дисципліна не передбачає екзамену (лише залік).";
                return false;
            }
            if (group.CompletedPracticalTasks < 1)
            {
                message = "ВІДМОВА: Без зданих лабораторних робіт допуск до Екзамену заборонено!";
                return false;
            }

            message = "Екзамен успішно складено!";
            group.MarkDisciplineAsCompleted(discipline.Name); // Закриваємо предмет
            return true;
        }
    }
}