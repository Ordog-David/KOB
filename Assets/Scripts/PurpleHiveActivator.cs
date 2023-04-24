using System.Collections;
using UnityEngine;

public class PurpleHiveActivator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectileTemplate;
    private Animator hiveAnimator;

    [SerializeField] private float activationDistance = 20;
    [SerializeField] private float activationSeconds;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private Vector3 projectileOffset;
    [SerializeField] private float aggroDistance = 5;
    private bool aggroed = false;
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
        if (aggroDistance >= distance && aggroed == false)
        {
            aggroed = true;
        }

        if (distance < activationDistance && timer >= activationSeconds && aggroed == true)
        {
            hiveAnimator.SetTrigger("Shoot");
            StartCoroutine(ShootProjectile());
            timer = 0;
        }
    }

    public void ProjectileMiss()
    {
        timer += activationSeconds / 2;
    }

    public IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(GetAnimationLength("Shoot"));

        var projectilePosition = transform.position + projectileOffset;
        var projectile = Instantiate(projectileTemplate, projectilePosition, transform.rotation);
        var homingProjectile = projectile.GetComponent<PurpleHiveProjectile>();
        homingProjectile.speed = projectileSpeed;
        homingProjectile.target = player.transform;
        homingProjectile.purpleHive = this;

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