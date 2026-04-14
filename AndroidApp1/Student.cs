

namespace AndroidApp1
{
    internal class Student
    {
        //属性
        public string name { get; set;}
        public int health { get; set;}
        public int energy { get; set;}
        public int happiness { get; set;}
        public int charm { get; set;}
        public int laziness { get; set;}
        public int confusion { get; set;}

        public int chinese { get; set;}
        public int math { get; set;}
        public int english { get; set;}
        public string crouse1Name { get; set;}
        public int crouse1Grade { get; set;}
        public string crouse2Name { get; set;}
        public int crouse2Grade { get; set;}
        public string crouse3Name { get; set;}
        public int crouse3Grade { get; set;}


        public Student(string name, int health, int energy, int happiness, int charm, int laziness, int confusion, int chinese, int math, int english, string crouse1Name, int crouse1Grade, string crouse2Name, int crouse2Grade, string crouse3Name, int crouse3Grade)
        { 
            this.name = name;
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
    }
}
