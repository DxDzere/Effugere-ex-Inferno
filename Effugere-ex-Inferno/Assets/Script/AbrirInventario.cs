using UnityEngine;
using System.Collections;

public class AbrirInventario : MonoBehaviour {

	public GameObject CamaraJugador;
	public GameObject CamaraInventario;

	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			CamaraJugador.SetActive (false);
			CamaraInventario.SetActive (true);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CamaraJugador.SetActive (true);
			CamaraInventario.SetActive (false);
		}
	}
}
