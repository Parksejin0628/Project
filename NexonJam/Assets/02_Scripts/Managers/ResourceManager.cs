using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class ResourceManager : MonoBehaviour
    {
        private T LoadOriginal<T>(string path) where T : Object
        {
            T original = Load<T>($"Prefabs/{path}");
            if (original == null)
            {
                string prevFuncName = new System.Diagnostics.StackFrame(1, true).GetMethod().Name;
                string prevClassName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
                Debug.Log($"Failed to load prefab : {path}\nprevFunc : {prevFuncName}\nprefClassName : {prevClassName}\n");
                return null;
            }
            return original;
        }

        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = Managers.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }

            //TODO: Instantiate를 거치지 않고 바로 Load하는 경우 못 찾았을 때 체크가 불가능

            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = LoadOriginal<GameObject>(path);

            GameObject go;
            if (original.GetComponent<Poolable>() != null)
                go = Managers.Pool.Pop(original, parent).gameObject;
            else
                go = Object.Instantiate(original, parent);

            go.name = original.name;
            return go;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
        {
            GameObject original = LoadOriginal<GameObject>(path);

            GameObject go;
            if (original.GetComponent<Poolable>() != null)
                go = Managers.Pool.Pop(original, null).gameObject;
            else
                go = Object.Instantiate(original);

            go.transform.position = position;
            go.transform.rotation = rotation;

            go.name = original.name;
            return go;
        }

        public void Destroy(GameObject go, float destroyTime = 0)
        {
            StartCoroutine(CoDestroy(go, destroyTime));
        }

        IEnumerator CoDestroy(GameObject go, float destroyTime)
        {
            if (go == null)
                yield break;

            yield return new WaitForSeconds(destroyTime);

            if (go == null)
                yield break;

            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable != null)
            {
                Managers.Pool.Push(poolable);
                yield break;
            }


            Object.Destroy(go);
        }

    }
}
