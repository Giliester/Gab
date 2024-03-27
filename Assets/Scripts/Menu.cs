using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image panelPrincipal;
    public TMP_Text rondaMaxTexto;
    public GameObject menuPanel;
    public GameObject infoPanel;
    public AudioClip audioMenu;
    public AudioClip audioTuto;
    private bool interactuable;
    private void Start()
    {
        StartCoroutine(MostrarMenu());
    }
    public void BotonJugar()
    {
        if (interactuable)
            StartCoroutine(GameManager.Juego.Jugar());
    }
    public IEnumerator MostrarMenu()
    {
        UIMenu();
        GameManager.Juego.enJuego = false;
        GameManager.Juego.Audio.clip = audioMenu;
        GameManager.Juego.Audio.Play();
        panelPrincipal.gameObject.SetActive(true);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            panelPrincipal.transform.GetComponentInParent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, t / 1);
            PostProcess.Depth.focalLength.value = Mathf.Lerp(0, 300, t / 1);
            GameManager.Juego.Audio.volume = Mathf.Clamp01(t / 1);
            yield return null;
        }
        interactuable = true;
    }
    public IEnumerator OcultarMenu()
    {
        interactuable = false;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            panelPrincipal.transform.GetComponentInParent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, t / 1);
            GameManager.Juego.Audio.volume = 1 - Mathf.Clamp01(t / 1);
            yield return null;
        }
        menuPanel.SetActive(false);
        infoPanel.SetActive(false);
    }
    public void Salir()
    {
        if (interactuable)
        {
            StopAllCoroutines();
            Application.Quit();
        }
    }
    public void UIMenu()
    {
        menuPanel.SetActive(true);
        infoPanel.SetActive(false);
    }
    public void UIInstrucciones()
    {
        menuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }
}
