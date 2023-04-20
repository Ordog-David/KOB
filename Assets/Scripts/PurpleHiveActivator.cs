using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleHiveActivator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectileTemplate;
    //private Animator hiveAnimator;

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
        //hiveAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        var seconds = timer % 60;
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if ( aggroDistance >= distance && aggroed == false)
        {
            aggroed = true;
            Debug.Log("Activated");
        }

        if (distance < activationDistance && seconds >= activationSeconds && aggroed == true)
        {
           //hiveAnimator.SetTrigger("Shoot");
           StartCoroutine(ShootProjectile());
           timer = 0;
        }
    }

    private IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(GetAnimationLength("Shoot"));

        var projectilePosition = transform.position + projectileOffset;
        var projectileRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        var projectile = Instantiate(projectileTemplate, projectilePosition, projectileRotation);
        var straightProjectile = projectile.GetComponent<StraightProjectile>();
        straightProjectile.speed = projectileSpeed;

        yield return null;
    }

    private float GetAnimationLength(string animationName)
    {
        //RuntimeAnimatorController animationController = hiveAnimator.runtimeAnimatorController;
        //for (int i = 0; i < animationController.animationClips.Length; i++)
        //{
        //    if (animationController.animationClips[i].name == animationName)
        //    {
        //        return animationController.animationClips[i].length;
        //    }
        //}

        return 0f;
    }
}