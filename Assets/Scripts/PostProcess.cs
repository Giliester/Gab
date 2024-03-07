using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using System;

public class PostProcess : MonoBehaviour
{
    public static PostProcessVolume Volume;
    private static bool activating;
    private static bool deactivating;
    public static Bloom Bloom
    {
        get 
        {
            return Volume == null ? null : Volume.profile.GetSetting<Bloom>();
        }
    }
    public static Vignette Vignette
    {
        get 
        {
            return Volume == null ? null : Volume.profile.GetSetting<Vignette>();
        }
    }
    public static DepthOfField Depth
    {
        get
        {
            return Volume == null ? null : Volume.profile.GetSetting<DepthOfField>();
        }
    }
    public static ColorGrading Color
    {
        get
        {
            return Volume == null ? null : Volume.profile.GetSetting<ColorGrading>();
        }
    }
    private void Awake()
    {
        if (Volume == null)
            Volume = GetComponent<PostProcessVolume>();
        else 
            Destroy(this.gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    { 

    }
    public static IEnumerator Activate(float time = 0.5f)
    {
        if (activating)
            yield return null;
        if (deactivating)
            deactivating = false;
        activating = true;
        float start = Volume.weight;
        float t = 0;
        while (t < 1 && activating)
        {
            t += Time.deltaTime;
            float i = Mathf.Clamp01(t/time);
            Volume.weight = Mathf.Lerp(start,1,t);
            yield return null;
        }
        Volume.weight = 1;
    }
    public static IEnumerator Deactivate(float time = 0.5f)
    {
        if (deactivating)
            yield return null;
        if (activating)
            activating = false;
        deactivating = true;
        float start = Volume.weight;
        float t = 0;
        while (t < 1 && deactivating)
        {
            t += Time.deltaTime;
            float i = Mathf.Clamp01(t / time);
            Volume.weight = Mathf.Lerp(start, 0, t);
            yield return null;
        }
        Volume.weight = 0;
    }
}
