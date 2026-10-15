using UnityEngine;
public class FileIOHelper : MonoBehaviour
{
    /// <summary>
    /// Right now this is just a wrapper for JsonUtility.FromJson.
    /// I've added this method so that I can handle all IO operations through here if I deem it necessary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T GetData<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
