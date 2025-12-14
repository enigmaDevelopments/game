using UnityEngine;

public abstract class LongRangeAttack : AttackBase
{
    [Header("Aim settings")]
    public Transform spawnTransform;
    public Transform rotationTransform;
    public AimingSystem aim;
    public Quaternion rotationOffeset = Quaternion.Euler(-180, 0, 90);
    private int i;
    private void Start()
    {
        if (spawnTransform == null)
            spawnTransform = transform;
        aim = transform.root.GetComponent<AimingSystem>();
        if (aim != null)
        {
            aim.weaponTransforms.Add(spawnTransform);
            if (rotationTransform == null)
                rotationTransform = transform.parent.parent.parent.parent;
            aim.rotationTransforms.Add(rotationTransform);
            aim.rotationOffesets.Add(rotationOffeset);
            i = aim.rotationOffesets.Count;
        }
    }

    private void OnDestroy()
    {
        if (aim != null)
        {
            aim.weaponTransforms.Remove(spawnTransform);
            aim.rotationTransforms.Remove(rotationTransform);
            aim.rotationOffesets.RemoveAt(i);
        }
    }
}
