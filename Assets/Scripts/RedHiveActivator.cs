using UnityEngine;

public class RedHiveActivator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectileTemplate;
    private Animator hiveAnimator;

    [SerializeField] private float activationDistance = 20;
    [SerializeField] private float activationSeconds;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private Vector3 projectileOffset;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        hiveAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < activationDistance && timer >= activationSeconds)
        { 
            hiveAnimator.SetTrigger("Shoot");
            timer = 0;
        }
    }

    public void OnShootAnimationEnd()
    {
        var projectilePosition = transform.position + transform.rotation * projectileOffset;
        var projectile = Instantiate(projectileTemplate, projectilePosition, transform.rotation);
        var hiveProjectile = projectile.GetComponent<RedHiveProjectile>();
        hiveProjectile.speed = projectileSpeed;
    }
}
