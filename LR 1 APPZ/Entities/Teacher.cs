namespace LR1_APPZ.Entities
{
    // [ВТОРИННИЙ ВУЗОЛ]: Перелік ролей викладачів
    public enum TeacherRole { Lecturer, Practice }

    // [ВТОРИННИЙ ВУЗОЛ]: Сутність викладача
    public class Teacher
    {
        public string Name { get; private set; }
        public TeacherRole Role { get; private set; }
        public bool IsBusy { get; private set; }

        // Тут конструктор викладача
        public Teacher(string name, TeacherRole role)
        {
            Name = name;
            Role = role;
            IsBusy = false;
        }

        // Тут метод управління зайнятістю викладача
        public void SetBusy(bool busy) => IsBusy = busy;
    }
}