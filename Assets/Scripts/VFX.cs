using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Destruir());
    }
    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
