using System;
using UnityEngine;

namespace DefaultSetting
{
    public class TestManager : MonoBehaviour
    {
        public Action testAction = null;
        public enum TestType
        {
            None,
            TestA,
            TestB,
        };
        public TestType testType = (TestType)1;

        public void Init()
        {
            testAction = null;
            testAction -= TestFunction;
            testAction += TestFunction;
        }

        public void OnTest()
        {
            print("테스트 시작");
            testAction?.Invoke();
            print("테스트 종료\n");
        }

        private void TestFunction()
        {
            switch (testType)
            {
                case TestType.None:
                    break;
                case TestType.TestA:
                    print("TestA, 원하는 테스트를 입력하세요");
                    break;
                case TestType.TestB:
                    print("TestB, 원하는 테스트를 입력하세요");
                    break;
                default:
                    break;
            }
        }
    }
}
