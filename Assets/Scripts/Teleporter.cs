using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 teleportcoordinates;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private RawImage minimapImage;
    [SerializeField] private Image minimapBorder;

    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = globalLight.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            collision.transform.position = teleportcoordinates;
            globalLight.color = defaultColor;
            minimapImage.enabled = true;
            minimapBorder.enabled = true;
        }
    }
}
