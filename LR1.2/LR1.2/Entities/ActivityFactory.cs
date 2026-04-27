using LR1._2.Entities;
using System;

namespace LR1._2.Entities
{
    // ПАТЕРН ФАБРИКА: Інкапсулює логіку створення об'єктів
    public static class ActivityFactory
    {
        // Фабричний метод для лекцій (завжди 2 години)
        public static Activity CreateLecture(string title)
        {
            return new Activity(title, ActivityType.Lecture, 2);
        }

        // Фабричний метод для лабораторних (завжди 4 години)
        public static Activity CreateLab(string title)
        {
            return new Activity(title, ActivityType.Laboratory, 4);
        }
    }
}