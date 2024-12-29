using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Transform FindTarget(string tag)
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(tag);
        return targetObject != null ? targetObject.transform : null;
    }
}
