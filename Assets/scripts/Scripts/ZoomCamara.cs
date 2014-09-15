using UnityEngine;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;

public class ZoomCamara : MonoBehaviour 
{
	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;

	private RaycastHit hit;
	private GameObject objetoPulsado;
	private ObjetoInteractuablev2 objetoInteractuableRef;

	void OnEnable()
	{
		GameCenter.InstanceRef.controladoraJugador.zoomCamaraRef = this;
	}

	void Start() 
	{
		posicionInicial.x = transform.position.x;
		posicionInicial.y = transform.position.y;
		posicionInicial.z = transform.position.z;
		
		rotacionInicial = transform.rotation;
	}

	void Update()
	{
		if (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() == estadoJugador.enEspera)
		{
			if (Input.GetMouseButtonDown(0))
			{
				try
				{
					Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(rayo, out hit, Mathf.Infinity))
					{
						objetoPulsado = GameObject.FindGameObjectWithTag(hit.collider.tag.ToString());
						try
						{
							objetoInteractuableRef = objetoPulsado.GetComponent<ObjetoInteractuablev2>();
							GameCenter.InstanceRef.controladoraTextos.objetoSeleccionado = objetoPulsado;
							GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef = this.objetoInteractuableRef;
							GameCenter.InstanceRef.controladoraJugador.objetoPulsado = this.objetoPulsado;
							if (objetoInteractuableRef.conZoom)
							{
								//TODO: arreglar el tema de sonidos
								GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoAcertarPulsar);
								GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado(estadoJugador.enZoomIn);
							}
							
							if (objetoInteractuableRef.conAnimacion)
							{
								objetoPulsado.animation.clip = objetoInteractuableRef.animacion;
								objetoPulsado.animation.Play();
							}
							
							if (objetoInteractuableRef.conSonido)
							{
								objetoPulsado.audio.clip = objetoInteractuableRef.sonido;
								objetoPulsado.audio.Play();
							}
						}
						catch
						{
							GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
						}
					}
				} catch {}
			
			}
		}
	}
}
