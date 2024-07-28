FindIDObject 사용법

컨셉 : 각 컴포넌트마다 가지고 있는 ID로 오브젝트를 찾는 방법

1. 찾으려는 컴포넌트의 InstanceID 또는 게임 오브젝트의 InstanceID를 확인한다.
컴포넌트의 ID			findTarget.GetInstanceID()
게임오브젝트의 ID		findTarget.gameObject.GetInstanceID()


2. Window - Select Object With Instance ID - InstanceID 입력 - 버튼 클릭으로 오브젝트를 찾을 수 있다.

-----------------------------------------------------------------
FinderScene 사용 법
1. Finder 오브젝트의 Finder 컴포넌트에 찾을 오브젝트를 연결한다.
2. 실행 시 오브젝트 아이디가 콘솔창에 출력된다.
3. 윈도우 창을 키고 아이디를 입력한 후 버튼을 누르면 오브젝트가 포커스된다.

-----------------------------------------------------------------
출처는 이곳입니다.
https://digitalopus.ca/site/free-unity3d-script-find-objects-by-instanceid/