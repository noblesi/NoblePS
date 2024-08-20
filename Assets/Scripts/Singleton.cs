using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) // ���� �ν��Ͻ��� null�̶��
            {
                instance = (T)FindObjectOfType(typeof(T)); // ��򰡿� �ν��Ͻ��� ���� �� ������ �ν��Ͻ��� FindObejctofType���� Ž��

                if (instance == null) // ���ٸ�, ���� ������Ʈ�� ����
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Start()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance.gameObject != this.gameObject)
        {
            DestroyImmediate(instance.transform.root.gameObject);
            instance = this as T;
        }

        // DontSestroyOnLoad�� �ٸ� ������Ʈ�� ������ �ִٸ� �۵� X, �Ŵ����� �θ��̰ų�, �ڽ��� �� �۵�
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject); // ���� ��ȯ�ǵ� ������Ʈ�� �ı����� �ʴ´�.
        }
    }
}