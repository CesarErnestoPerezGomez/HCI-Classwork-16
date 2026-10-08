using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMC : MonoBehaviour
{
    private Rigidbody rb;
    
    private AudioSource fuenteAudio;
    public AudioClip sonidoFlipDerecha;
    public AudioClip sonidoMegaSalto;
    public AudioClip sonidoFlipIzquierda;
    
    private List<string> listaInputs = new List<string>();
    
    public float tiempoMaximoCombo = 1.5f; 
    public float fuerzaSalto = 6f;
    public float fuerzaEmpujeLateral = 5f;
    public float fuerzaGiro = 300f;
    private float tiempoUltimaTecla;    
    private string[] combo_FlipDerecha = { "Up", "Up", "Down", "Down", "Q", "A" };
    private  string[] combo_MegaSalto   = { "Up", "Up", "Down", "Q", "A" };
    private string[] combo_FlipIzquierda = { "Up", "Up", "Up" };

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        fuenteAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        DetectarEntradas();

        if (listaInputs.Count > 0 && Time.time - tiempoUltimaTecla > tiempoMaximoCombo)
        {
            Debug.Log("Out of Time");
            listaInputs.Clear();
        }
    }

    void DetectarEntradas()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) RegistrarTecla("Up");
        else if (Input.GetKeyDown(KeyCode.DownArrow)) RegistrarTecla("Down");
        else if (Input.GetKeyDown(KeyCode.Q)) RegistrarTecla("Q");
        else if (Input.GetKeyDown(KeyCode.A)) RegistrarTecla("A");
    }

    void RegistrarTecla(string tecla)
    {
        listaInputs.Add(tecla);
        tiempoUltimaTecla = Time.time;
        Debug.Log("Entrada: " + string.Join(" + ", listaInputs));
        EvaluarCombos();
    }

    void EvaluarCombos() { 
        if (EsIgualA(combo_FlipDerecha)) 
        {
            fuenteAudio.PlayOneShot(sonidoFlipDerecha);
            EjecutarFlipDerecha();
            listaInputs.Clear();
        }
        else if (EsIgualA(combo_MegaSalto)) {
            fuenteAudio.PlayOneShot(sonidoMegaSalto);
            EjecutarMegaSalto();
            listaInputs.Clear();
        }
        else if (EsIgualA(combo_FlipIzquierda)) {
            fuenteAudio.PlayOneShot(sonidoFlipIzquierda);
            EjecutarFlipIzquierda();
            listaInputs.Clear();
        }
    }

    bool EsIgualA(string[] comboObjetivo)
    {
        if (listaInputs.Count < comboObjetivo.Length) return false;
        int inicio = listaInputs.Count - comboObjetivo.Length;
        for (int i = 0; i < comboObjetivo.Length; i++)
        {
            if (listaInputs[inicio + i] != comboObjetivo[i])
            {
                return false;
            }
        }
        return true;
    }


    void EjecutarFlipDerecha()
    {
        Debug.Log("(↑+↑+↓+↓+Q+A)");
        Vector3 direccionImpulso = (Vector3.up * 1.8f + Vector3.right).normalized;
        rb.linearVelocity = Vector3.zero; 
        rb.AddForce(direccionImpulso * fuerzaSalto, ForceMode.Impulse);
        rb.AddTorque(Vector3.back * fuerzaGiro, ForceMode.Impulse); 
    }

    void EjecutarMegaSalto()
    {
        Debug.Log("(↑+↑+↑+↓+Q+A)");
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * (fuerzaSalto * 1.8f), ForceMode.Impulse);
        rb.AddTorque(Vector3.right * fuerzaGiro * 0.5f, ForceMode.Impulse);
    }

    void EjecutarFlipIzquierda()
    {
        Debug.Log("(↑+↑+↑)");
        Vector3 direccionImpulso = (Vector3.up * 1.8f + Vector3.left).normalized;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direccionImpulso * fuerzaSalto, ForceMode.Impulse);
        rb.AddTorque(Vector3.forward * fuerzaGiro, ForceMode.Impulse);
    }
}