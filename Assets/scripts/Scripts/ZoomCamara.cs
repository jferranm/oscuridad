﻿using UnityEngine;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class ZoomCamara : MonoBehaviour 
{
	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;

	private RaycastHit hit;

	void Start() 
	{
		posicionInicial.x = transform.position.x;
		posicionInicial.y = transform.position.y;
		posicionInicial.z = transform.position.z;
		
		rotacionInicial = transform.rotation;
	}

	void Update()
	{
		if (GameCenter.InstanceRef.controladoraJugador.EstadoJugador.Equals(EstadosJugador.enEspera))
		{
			if (Input.GetMouseButtonDown(0))
			{
				try
				{
					Ray rayo = this.camera.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(rayo, out hit, Mathf.Infinity))
					{
						GameCenter.InstanceRef.controladoraJuego.objetoPulsado =  GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Objeto(hit.collider.tag.ToString());
						GameCenter.InstanceRef.controladoraJuego.personajePulsado = GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Personaje(hit.collider.tag.ToString());

						try
						{
							//TODO: arreglar el tema de sonidos
							GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoAcertarPulsar);
							GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomIn;
						}
						catch
						{
							GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
						}
					}
					else
					{
					}
				} catch {}
			
			}
		}
	}
}
