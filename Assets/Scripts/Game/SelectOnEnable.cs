using UnityEngine;

public class SelectOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().Select();
    }

}
