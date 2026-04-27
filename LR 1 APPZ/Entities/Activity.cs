namespace LR1_APPZ.Entities
{
    // [ВТОРИННИЙ ВУЗОЛ]: Категорії активностей для правильного призначення викладачів
    public enum ActivityCategory { Lecture, Practice, Control }

    // [ВТОРИННИЙ ВУЗОЛ]: Клас активності, який дозволяє легко додавати нові типи занять
    public class Activity
    {
        public string Name { get; private set; }
        public ActivityCategory Category { get; private set; }
        public int DurationHours { get; private set; }

        // Тут конструктор активності
        public Activity(string name, ActivityCategory category, int durationHours)
        {
            Name = name;
            Category = category;
            DurationHours = durationHours;
        }
    }
}