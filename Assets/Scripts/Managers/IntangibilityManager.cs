using UnityEngine;

public class IntangibilityManager : MonoBehaviour
{
    public int mainLayer;
    public int intangibleLayer;
    private float _timer;
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
            _timer = 0;
        }
        else
        {
            health.canTakeDamage = false;
            gameObject.layer = intangibleLayer;
        }
    }
}
