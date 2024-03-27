using System.Collections;
using UnityEngine;

public class Jugador : Entidad
{
    public GameObject proyectil;
    public Color NaveNormal;
    public Color NaveCritico;
    void Update()
    {
        Movimiento();
        GetComponent<SpriteRenderer>().color = Color.Lerp(NaveCritico, NaveNormal, Vida / 100);
    }
    void Movimiento()
    {
        Vector3 objetivo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rotar(objetivo);

        if (Input.GetMouseButton(1))
            Mover(objetivo, true);

        if (Input.GetMouseButtonDown(0))
            Instantiate(proyectil, transform.position + Vector3.forward, transform.rotation).GetComponent<Proyectil>().origen = this.gameObject;
    }
    public override IEnumerator Destruir()
    {
        GameManager.Juego.enJuego = false;
        StartCoroutine(GameManager.Juego.FinPartida());
        Camera.main.backgroundColor = GameManager.Juego.FondoCritico;
        PostProcess.Vignette.color.value = GameManager.Juego.ViñetaCritico;
        yield return StartCoroutine(base.Destruir());
    }
}