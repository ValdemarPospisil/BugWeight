using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInputHandler : MonoBehaviour
{
    void Update()
    {
        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Get the currently selected GameObject in the EventSystem
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject != null)
            {
                // Try to get the Button component from the selected GameObject
                Button button = selectedObject.GetComponent<Button>();

                if (button != null)
                {
                    // Invoke the button's click event
                    button.onClick.Invoke();
                }
            }
        }
    }
}
