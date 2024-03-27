using UnityEngine;

public class Circulo : Entidad
{
    public int empuje;
    void Update()
    {
        if(Jugador != null)
        {
            Rotar(Jugador.transform.position);
            Mover(Jugador.transform.position, false, 0);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Entidad e) && e == Jugador)
        {
            Mover(Jugador.transform.position, true, 0, empuje);
            Jugador.Vida -= 100 * Time.deltaTime * (GameManager.Juego.ronda / 2);
        }
    }
}