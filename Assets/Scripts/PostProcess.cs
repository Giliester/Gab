using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcess : MonoBehaviour
{
    public static PostProcessVolume Volume;
    public static Bloom Bloom => Volume == null ? null : Volume.profile.GetSetting<Bloom>();
    public static Vignette Vignette => Volume == null ? null : Volume.profile.GetSetting<Vignette>();
    public static DepthOfField Depth => Volume == null ? null : Volume.profile.GetSetting<DepthOfField>();
    public static ColorGrading Color => Volume == null ? null : Volume.profile.GetSetting<ColorGrading>();
    private void Awake()
    {
        if (Volume == null)
            Volume = GetComponent<PostProcessVolume>();
        else 
            Destroy(this.gameObject);
    }
}