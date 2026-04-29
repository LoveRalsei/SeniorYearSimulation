

namespace AndroidApp1
{
    public class Student
    {
        //属性
        public string name { get; set; }
        public int money { get; set; }
        public int health { get; set; }
        public int energy { get; set; }
        public int happiness { get; set; }
        public int charm { get; set; }
        public int laziness { get; set; }
        public int confusion { get; set; }

        public int chinese { get; set; }
        public int math { get; set; }
        public int english { get; set; }
        public string crouse1Name { get; set; }
        public int crouse1Grade { get; set; }
        public string crouse2Name { get; set; }
        public int crouse2Grade { get; set; }
        public string crouse3Name { get; set; }
        public int crouse3Grade { get; set; }

        protected const int _maxChinese = 150;
        protected const int _maxMath = 150;
        protected const int _maxEnglish = 150;
        protected const int _maxCrouse1Grade = 100;
        protected const int _maxCrouse2Grade = 100;
        protected const int _maxCrouse3Grade = 100;

        public Student()
        {
            this.name = "无参构造对象";
        }

        public Student(string name, int money, int health, int energy, int happiness, int charm, int laziness, int confusion, int chinese, int math, int english, string crouse1Name, int crouse1Grade, string crouse2Name, int crouse2Grade, string crouse3Name, int crouse3Grade)
        {
            this.name = name;
            this.money = money;

            this.health = health;
            this.energy = energy;
            this.happiness = happiness;
            this.charm = charm;
            this.laziness = laziness;
            this.confusion = confusion;

            this.chinese = chinese;
            this.math = math;
            this.english = english;
            this.crouse1Name = crouse1Name;
            this.crouse1Grade = crouse1Grade;
            this.crouse2Name = crouse2Name;
            this.crouse2Grade = crouse2Grade;
            this.crouse3Name = crouse3Name;
            this.crouse3Grade = crouse3Grade;
        }

        public Student(Student student)
        {
            if (student == null)
                return;
            this.name = student.name;
            this.money = student.money;
            this.health = student.health;
            this.energy = student.energy;
            this.happiness = student.happiness;
            this.charm = student.charm;
            this.laziness = student.laziness;
            this.confusion = student.confusion;

            this.chinese = student.chinese;
            this.math = student.math;
            this.english = student.english;
            this.crouse1Name = student.crouse1Name;
            this.crouse1Grade = student.crouse1Grade;
            this.crouse2Name = student.crouse2Name;
            this.crouse2Grade = student.crouse2Grade;
            this.crouse3Name = student.crouse3Name;
            this.crouse3Grade = student.crouse3Grade;
        }

        public void IncreaseProperty(StudentProperty property, int value)
        {
            switch (property)
            {
                case StudentProperty.Money:
                    this.money += value;
                    break;
                case StudentProperty.Health:
                    this.health += value;
                    break;
                case StudentProperty.Energy:
                    this.energy += value;
                    break;
                case StudentProperty.Happiness:
                    this.happiness += value;
                    break;
                case StudentProperty.Charm:
                    this.charm += value;
                    break;
                case StudentProperty.Laziness:
                    this.laziness += value;
                    break;
                case StudentProperty.Confusion:
                    this.confusion += value;
                    break;
                case StudentProperty.Chinese:
                    this.chinese += value;
                    break;
                case StudentProperty.Math:
                    this.math += value;
                    break;
                case StudentProperty.English:
                    this.english += value;
                    break;
                case StudentProperty.Crouse1Grade:
                    this.crouse1Grade += value;
                    break;
                case StudentProperty.Crouse2Grade:
                    this.crouse2Grade += value;
                    break;
                case StudentProperty.Crouse3Grade:
                    this.crouse3Grade += value;
                    break;
            }
        }

        public bool EnoughEnergy(int cost)
        {
            return this.energy >= cost;
        }

        public void ReduceEnergy(int value)
        {
            this.energy -= value;
        }

        public bool Lazy()
        {
            Random random = new Random();
            int randomValue = random.Next(1, 101);
            return randomValue <= this.laziness;
        }

        public bool Confusion()
        {
            Random random = new Random();
            int randomValue = random.Next(1, 101);
            return randomValue <= this.confusion;
        }
    }

    public enum StudentProperty
    {
        Money,
        Health,
        Energy,
        Happiness,
        Charm,
        Laziness,
        Confusion,
        Chinese,
        Math,
        English,
        Crouse1Grade,
        Crouse2Grade,
        Crouse3Grade
    }
}
