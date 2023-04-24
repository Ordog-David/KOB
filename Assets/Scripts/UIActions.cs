using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{
    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
