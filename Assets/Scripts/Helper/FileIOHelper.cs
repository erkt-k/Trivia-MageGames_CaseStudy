using UnityEngine;
public class FileIOHelper : MonoBehaviour
{
    public static T GetData<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
