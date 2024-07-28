using System;

namespace DefaultSetting
{
    public enum Category
    {
        Red,
        Green,
        Blue,
    }

    [Serializable]
    public class Example1Entity
    {
        public uint id;
        public string name;
        public int price;
        public bool isNotForSale;
        public float rate;
        public Category category;

        public Example1Entity DeepCopy()
        {
            Example1Entity newCopy = new Example1Entity();

            //newCopy.id = this.id;
            //newCopy.stage = this.stage;
            //newCopy.coinCount = this.coinCount;
            //...데이터 넣기

            return newCopy;
        }
    }
}