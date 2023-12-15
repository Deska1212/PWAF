using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActivationEffect : MonoBehaviour
{
    [SerializeField] private float effectTime = 1f;
    [SerializeField] private SpriteRenderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(this.gameObject, Vector2.one * 1.5f, effectTime).setEase(LeanTweenType.easeOutCirc);
        LeanTween.value(this.gameObject, SetAlpha, 1f, 0f, effectTime).setOnComplete(DestroyEffect);
    }

    private void DestroyEffect()
    {
        Destroy(this.gameObject);
    }

    private void SetAlpha(float a)
    {
        Color col = renderer.color;
        col.a = a;
        renderer.color = col;
    }
}
