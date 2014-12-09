using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelDireccionesOpciones : MonoBehaviour 
{
	public GameObject botonNorte;
	public GameObject botonSur;
	public GameObject botonEste;
	public GameObject botonOeste;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			Reorganizar_Direcciones ();
		}
	}

	private void Reorganizar_Direcciones()
	{
		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaNorte.Equals(Escenas.ninguna)) 
			Desactivar (botonNorte);

		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaSur.Equals(Escenas.ninguna)) 
			Desactivar (botonSur);

		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaEste.Equals(Escenas.ninguna)) 
			Desactivar (botonEste);
		
		if (GameCenter.InstanceRef.controladoraJuego.escenaActual.EscenaOeste.Equals(Escenas.ninguna)) 
			Desactivar (botonOeste);
	}

	private void Desactivar (GameObject objeto)
	{
		Image imagenBoton = objeto.GetComponent<Image> ();

		imagenBoton.color = new Color (255,0,0);
	}
}
