using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraSonidos
{
	//Sonidos------------------
	public GameObject EmisorFX;
	public AudioSource emisorFX;

	public AudioClip sonidoFalloPulsar;
	public AudioClip sonidoAcertarPulsar;
	//-------------------------------

	//B.S.O.-----------------------------
	public GameObject EmisorBSO;
	public AudioSource emisorBSO;

	public AudioClip bsoEscenarioWard;
	//-------------------------------------

	public float volumenSonido
	{
		set{EmisorFX.GetComponent<AudioSource>().volume = value;}
	}

	public float volumenMusica
	{
		set{EmisorBSO.GetComponent<AudioSource>().volume = value;}
	}

	public ControladoraSonidos()
	{
	}
	
	private static ControladoraSonidos instanceRef;
	
	public static ControladoraSonidos InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraSonidos();
			instanceRef.Inicializar();
		}
		
		return instanceRef;
	}

	public void Inicializar()
	{
		EmisorBSO = GameObject.Find ("EmisorBSO");
		EmisorFX = GameObject.Find ("EmisorFX");
	}

	public void Start()
	{
		sonidoFalloPulsar = Resources.Load("Sonidos/pulsarFallo", typeof(AudioClip)) as AudioClip;
		sonidoAcertarPulsar = Resources.Load("Sonidos/pulsarAcertar", typeof(AudioClip)) as AudioClip;
		bsoEscenarioWard = Resources.Load("Musica/EscenarioWard", typeof(AudioClip)) as AudioClip;

		emisorFX = EmisorFX.GetComponent<AudioSource> ();
		emisorBSO = EmisorBSO.GetComponent<AudioSource> ();
	}

	public void Lanzar_Bso(string escenarioActual)
	{
		if(escenarioActual.Contains("EscenaWard"))
		{
			EmisorBSO.GetComponent<AudioSource>().clip = bsoEscenarioWard;
			emisorBSO.Play ();
			return;
		}
	}

	public void Lanzar_Fx(AudioClip fxSeleccionado)
	{
		EmisorFX.GetComponent<AudioSource>().PlayOneShot(fxSeleccionado);
	}


}
