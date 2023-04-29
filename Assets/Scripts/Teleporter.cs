using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 teleportToPosition;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private RawImage minimapImage;
    [SerializeField] private Image minimapBorder;

    private Color defaultColor;

    // Start is called before the first frame update
    private void Start()
    {
        defaultColor = globalLight.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            collision.transform.position = teleportToPosition;
            globalLight.color = defaultColor;
            minimapImage.enabled = true;
            minimapBorder.enabled = true;
        }
    }
}
