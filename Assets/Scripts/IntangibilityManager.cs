using UnityEngine;

public class IntangibilityManager : MonoBehaviour
{
    public int mainLayer;
    public int intangibleLayer;
    public float _timer;
    private Health health;
    public float Timer {
        set
        {
            if (_timer < value)
                _timer = value;
        } 
    }
    void Start()
    {
        health = GetComponent<Health>();
    }
    void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            health.canTakeDamage = true;
            gameObject.layer = mainLayer;
        }
        else
        {
            health.canTakeDamage = false;
            gameObject.layer = intangibleLayer;
        }
    }
}
