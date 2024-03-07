using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour
{
    private static Jugador jugador;
    public static Jugador JugadorIns { private set { } get { return jugador; } }
    public float vida;
    private string enemigoTag = "Enemigo";
    private string proyectilTag = "Proyectil";
    public GameObject proyectil;
    public Color NaveNormal;
    public Color NaveCritico;

    private void Awake()
    {
        if(jugador == null)
            jugador = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {

    }

    void Update()
    {
        if(GameManager.Game.enJuego)
        {
            Movimiento();
        }
        if (vida < 0)
            vida = 0;
        if (vida >= 100)
            vida = 100;
        GetComponent<SpriteRenderer>().color = Color.Lerp(NaveCritico, NaveNormal,vida/100);
    }
    public static void Desenlazar()
    {
        Jugador.JugadorIns = null;
    }
    void Movimiento()
    {
        Vector3 direccion = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg), 15 * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            direccion.Normalize();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(direccion*8*Time.deltaTime, ForceMode2D.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(proyectil, transform.position + Vector3.forward, transform.rotation).GetComponent<Proyectil>().propio = true;
        }
    }
    void Estadisticas()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemigoTag))
        {
            Vector2 direccion = collision.transform.position - transform.position;
            direccion.Normalize();

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direccion * 3*Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemigoTag))
        {
            vida -= 100 * Time.deltaTime * (GameManager.Game.ronda/2);
        }
    }
}
