using Unity.VisualScripting;
using UnityEngine;

public class IntangibilityManager : MonoBehaviour
{
    public enum FlashType : byte
    {
        flashing,
        visable,
        invisable
    }
    public float flashRate;
    public FlashType flashType;
    [Header("Layers")]
    public int mainLayer;
    public int intangibleLayer;
    private float _timer;
    private Health health;
    private Renderer[] renderers;
    private bool rendering = true;
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
        renderers = GetComponentsInChildren<Renderer>();
    }
    void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;
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
        bool invisable = _timer != 0;
        if (flashType == FlashType.visable)
            invisable = false;
        else if (flashType == FlashType.flashing)
            invisable = (int)(_timer % (flashRate * 2) / flashRate) == 1;
        if (invisable)
        {
            if (rendering)
            {
                rendering = false;
                foreach (Renderer renderer in renderers)
                    renderer.enabled = false;
            }
        }
        else if (!rendering)
        {
            rendering = true;
            foreach (Renderer renderer in renderers)    
                renderer.enabled = true;
        }
    }
}
