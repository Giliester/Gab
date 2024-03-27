using System.Collections;
using UnityEngine;

public class Atacante : Entidad
{
    public GameObject proyectil;
    void Start()
    {
        StartCoroutine(Disparar());
    }
    void Update()
    {
        if(Jugador != null)
        {
            Rotar(Jugador.transform.position);
            Mover(Jugador.transform.position, false, 5);
        }
    }
    IEnumerator Disparar()
    {
        while (Jugando)
        {
            if (!destrullendo)
            {
                yield return new WaitForSeconds(3);
                Instantiate(proyectil, transform.position + Vector3.forward, transform.rotation).GetComponent<Proyectil>().origen = this.gameObject;
            }
            yield return null;
        }
    }
}