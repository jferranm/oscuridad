﻿using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraSonidos
{
	//Sonidos
	public AudioClip sonidoFalloPulsar;
	public AudioClip sonidoAcertarPulsar;

	//B.S.O.
	public AudioClip bsoEscenaCasa;

	public ControladoraSonidos()
	{
	}
	
	private static ControladoraSonidos instanceRef;
	
	public static ControladoraSonidos InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraSonidos();
		}
		
		return instanceRef;
	}

	public void Start()
	{
		sonidoFalloPulsar = Resources.Load("Sonidos/pulsarFallo", typeof(AudioClip)) as AudioClip;
		sonidoAcertarPulsar = Resources.Load("Sonidos/pulsarAcertar", typeof(AudioClip)) as AudioClip;
		bsoEscenaCasa = Resources.Load("Musica/Escenario 1_v1", typeof(AudioClip)) as AudioClip;
	}
}
