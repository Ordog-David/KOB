using System.Collections;
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
        var seconds = timer % 60;
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < activationDistance && seconds >= activationSeconds)
        { 
            hiveAnimator.SetTrigger("Shoot");
            StartCoroutine(ShootProjectile());
            timer = 0;
        }
    }

    private IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(GetAnimationLength("Shoot"));

        var projectilePosition = transform.position + projectileOffset;
        var projectile = Instantiate(projectileTemplate, projectilePosition, transform.rotation);
        var straightProjectile = projectile.GetComponent<RedHiveProjectile>();
        straightProjectile.speed = projectileSpeed;

        yield return null;
    }

    private float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController animationController = hiveAnimator.runtimeAnimatorController;
        for (int i = 0; i < animationController.animationClips.Length; i++)
        {
            if (animationController.animationClips[i].name == animationName)
            {
                return animationController.animationClips[i].length;
            }
        }

        return 0f;
    }
}
