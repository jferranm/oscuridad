using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraSonidos
{
	#region VARIABLES

	//Sonidos------------------
	public GameObject EmisorFX;
	public AudioSource emisorFX;

	public AudioClip sonidoFalloPulsar;
	public AudioClip sonidoAcertarPulsar;
	public AudioClip sonidoPasarPagina;
	public AudioClip sonidoEscribir;
	public AudioClip sonidoCerrajeria;
	public AudioClip sonidoCerrajeriaFallo;
	public AudioClip sonidoCerrarCremallera;
	public AudioClip sonidoBuscarMochila;
	public AudioClip sonidoApagarVela;
	public AudioClip sonidoEncenderVela;
	public AudioClip sonidoBotellaChocando;
	public AudioClip sonidoLibroCaer;
	//-------------------------------

	//B.S.O.-----------------------------
	public GameObject EmisorBSO;
	public AudioSource emisorBSO;

	public AudioClip bsoEscenarioWard;
	public AudioClip bsoMenuPrincipal;
	//-------------------------------------

	public float volumenSonido
	{
		set{emisorFX.volume = value;}
	}

	public float volumenMusica
	{
		set{emisorBSO.volume = value;}
	}

	#endregion

	#region CONSTRUCTORES

	public ControladoraSonidos()
	{

	}
	
	#endregion

	#region METODOS

	public void Awake()
	{
		sonidoFalloPulsar = Resources.Load("Sonidos/pulsarFallo", typeof(AudioClip)) as AudioClip;
		sonidoAcertarPulsar = Resources.Load("Sonidos/pulsarAcertar", typeof(AudioClip)) as AudioClip;
		sonidoPasarPagina = Resources.Load("Sonidos/PasarPagina", typeof(AudioClip)) as AudioClip;
		sonidoEscribir = Resources.Load("Sonidos/EscribirPapel", typeof(AudioClip)) as AudioClip;
		sonidoCerrajeria = Resources.Load("Sonidos/Cerrajeria", typeof(AudioClip)) as AudioClip;
		sonidoCerrajeriaFallo = Resources.Load("Sonidos/CerrajeriaFallo", typeof(AudioClip)) as AudioClip;
		sonidoCerrarCremallera = Resources.Load("Sonidos/CerrarCremallera", typeof(AudioClip)) as AudioClip;
		sonidoBuscarMochila = Resources.Load("Sonidos/BuscarMochila", typeof(AudioClip)) as AudioClip;
		sonidoApagarVela = Resources.Load("Sonidos/ApagarVela", typeof(AudioClip)) as AudioClip;
		sonidoEncenderVela = Resources.Load("Sonidos/EncenderVela", typeof(AudioClip)) as AudioClip;
		sonidoBotellaChocando = Resources.Load("Sonidos/BotellaChocando", typeof(AudioClip)) as AudioClip;
		sonidoLibroCaer = Resources.Load("Sonidos/LibroCaer", typeof(AudioClip)) as AudioClip;

		bsoEscenarioWard = Resources.Load("Musica/EscenarioWard", typeof(AudioClip)) as AudioClip;
		bsoMenuPrincipal = Resources.Load("Musica/MenuPrincipal", typeof(AudioClip)) as AudioClip;

		EmisorBSO = GameObject.Find ("EmisorBSO");
		EmisorFX = GameObject.Find ("EmisorFX");

		emisorFX = EmisorFX.GetComponent<AudioSource> ();
		emisorBSO = EmisorBSO.GetComponent<AudioSource> ();
	}

	public void Lanzar_Bso(string escenarioActual)
	{
		if(escenarioActual.Contains("EscenaWard"))
		{
			emisorBSO.clip = bsoEscenarioWard;
			emisorBSO.Play ();
			return;
		}

		if(escenarioActual.Contains("Menu"))
		{
			EmisorBSO.GetComponent<AudioSource>().clip = bsoMenuPrincipal;
			emisorBSO.Play ();
			return;
		}
	}

	public void Lanzar_Fx(AudioClip fxSeleccionado)
	{
		emisorFX.clip = fxSeleccionado;
		emisorFX.Play ();
	}

	public void Lanzar_Fx(AudioClip fxSeleccionado, float retardo)
	{
		emisorFX.clip = fxSeleccionado;
		emisorFX.PlayDelayed(retardo);
	}

	#endregion
}
