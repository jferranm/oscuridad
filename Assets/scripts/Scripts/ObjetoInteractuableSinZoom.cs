using UnityEngine;
using System.Collections;
using Oscuridad.Clases;

public class ObjetoInteractuableSinZoom : MonoBehaviour 
{
	public Animation animacion;
	public AudioClip clipSonido;

	public Vector3 posicion;
	public Vector3 rotacion;

	void Start()
	{
		if (animacion != null) 
		{
			if (animacion.clip != null)
				animacion.clip.legacy = true;
		}
	}

	public void Ejecutar_Animacion()
	{
		if (animacion != null) 
			animacion.Play();
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(clipSonido);
	}

	public void Mover_Objeto()
	{
		this.gameObject.transform.localPosition = new Vector3 (posicion.x, posicion.y, posicion.z);
		this.gameObject.transform.localEulerAngles = new Vector3(rotacion.x, rotacion.y, rotacion.z);
	}

	public void Lanzar_Sonido()
	{
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(clipSonido);
	}
}
