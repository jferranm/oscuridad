using UnityEngine;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class ZoomCamara : MonoBehaviour 
{
	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;
	public GameObject escenaVistaCamara;

	private RaycastHit hit;

	void OnEnable()
	{
		ActivarHijos (escenaVistaCamara);
	}

	void OnDisable()
	{
		DesactivarHijos (escenaVistaCamara);
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
		if (GameCenter.InstanceRef.controladoraJugador.EstadoJugador.Equals(EstadosJugador.enEspera))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray rayo = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
				{
					if(Physics.Raycast(rayo, out hit, Mathf.Infinity))
					{
						switch(hit.collider.gameObject.layer)
						{
							//Objeto Interactuable
							case 8: 
							{
								if(GameCenter.InstanceRef.controladoraJuego.camaraActiva.ExisteInteractuable(hit.collider.tag.ToString()))
							   	{
									GameCenter.InstanceRef.controladoraJuego.interactuablePulsado =  GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Interactuable(hit.collider.tag.ToString());
									GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoOpciones = hit.collider.gameObject.GetComponent<ObjetoInteractuableConZoom>();
									GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoGameObject = hit.collider.gameObject;
									GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoAcertarPulsar);
									GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomIn;
								}
								break;
							}

							//Objeto Interactuable Sin Zoom
							case 9:
							{
								GameCenter.InstanceRef.controladoraJuego.EjecutarAccion(hit.collider.gameObject);
								break;
							}

							//UI
							case 5:
							{
								break;
							}

							//Objeto no Interactuable
							default: 
							{
								GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
								GameCenter.InstanceRef.controladoraJuego.interactuablePulsado = null;

								break;
							}
						}
					}
				}
			}
		}
	}

	private void DesactivarHijos(GameObject g) 
	{
		if (g != null) 
		{
			foreach (Transform child in g.transform) 
			{
				child.gameObject.SetActive (false);
			}
		}
	}
	
	private void ActivarHijos(GameObject g) 
	{
		if (g != null) 
		{
			foreach (Transform child in g.transform) 
			{
				child.gameObject.SetActive (true);
			}
		}
	}
}
