using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static bool isDebug;
    // Start is called before the first frame update
    void Awake()
    {
        if (Application.version.StartsWith('d'))
        {
            isDebug = true;
        }
        else
        {
            var debug = GameObject.FindGameObjectsWithTag("Debug");
            foreach(var _object in debug){
                _object.SetActive(false);
            }
        }
    }
}
