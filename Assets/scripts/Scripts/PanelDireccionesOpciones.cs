using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelDireccionesOpciones : MonoBehaviour 
{
	public Image botonNorte;
	public Image botonSur;
	public Image botonEste;
	public Image botonOeste;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			Reiniciar_Direcciones();
		}
	}

	private void Reorganizar_Direcciones()
	{
		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaNorte.Equals("ninguna")) 
			Desactivar (botonNorte);

		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaSur.Equals("ninguna"))
			Desactivar (botonSur);

		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaEste.Equals("ninguna"))
			Desactivar (botonEste);
		
		if (GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaOeste.Equals("ninguna"))
			Desactivar (botonOeste);
	}

	private void Normalizar_Botones()
	{
		botonNorte.color = new Color (255, 0, 0);
		botonSur.color = new Color (255, 0, 0);
		botonEste.color = new Color (255, 0, 0);
		botonOeste.color = new Color (255, 0, 0);
	}

	private void Desactivar (Image imagenObjeto)
	{
		imagenObjeto.color = new Color (255,255,255);
	}

	public void Reiniciar_Direcciones()
	{
		Normalizar_Botones();
		Reorganizar_Direcciones ();
	}
}
