using System;
using UnityEngine;

namespace DefaultSetting
{
    [Serializable]
    public class Example2Entity
    {
        public uint id;
        public string name;
        public Vector2 vector2;
        public Vector3 vector3;
        public Color color;
        public GameObject objPrefab;
        public Sprite objSprite;

        public Example2Entity DeepCopy()
        {
            Example2Entity newCopy = new Example2Entity();

            //newCopy.id = this.id;
            //newCopy.stage = this.stage;
            //newCopy.coinCount = this.coinCount;
            //...데이터 넣기

            return newCopy;
        }
    }
}