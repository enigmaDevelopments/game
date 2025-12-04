using UnityEngine;

public class Bulldoze : DashThroughDamage
{
    [Header("Charge Settings")]
    public float chargeDuration;
    protected override System.Collections.IEnumerator Dash()
    {
        Vector3 position = transform.position;
        float startTime = Time.time;
        while (Time.time < startTime + chargeDuration)
        {
            if (position != transform.position)
            {
                position = transform.position;
                startTime = Time.time;
            }
            if (!dash.IsPressed())
                yield break;
            yield return null;
        }
        StartCoroutine(base.Dash());
        yield break;
    }
}
