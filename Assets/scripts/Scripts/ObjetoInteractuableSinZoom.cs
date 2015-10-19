using UnityEngine;
using System.Collections;

public class ObjetoInteractuableSinZoom : MonoBehaviour 
{
	public Animation animation;
    public AnimationClip clip;
	public AudioClip clipSonido;
	public Vector3 posicion;
	public Vector3 rotacion;

	void Start()
	{
		clip.legacy = true;
	}

	public void Ejecutar_Animacion()
	{
		animation.clip = clip;
        animation.AddClip(clip, clip.name);

        animation.Play(clip.name);
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(clipSonido);
	}

	public void Mover_Objeto()
	{
		this.gameObject.transform.Translate(posicion);
		this.gameObject.transform.Rotate(rotacion);
	}
}
