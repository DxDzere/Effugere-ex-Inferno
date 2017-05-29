using UnityEngine;
using System.Collections;

public class NumeroArma : MonoBehaviour {

	public GameObject PaloMano;
	public GameObject EspadaMano;

	public GameObject PaloInventario;
	public GameObject PaloInventarioMano;

	public GameObject EspadaInventario;
	public GameObject EspadaInventarioMano;

	public int Arma;

	void Update () {
		if (Arma == 1){
			PaloMano.SetActive(true);
			EspadaMano.SetActive (false);
			PaloInventario.SetActive (true);
			PaloInventarioMano.SetActive (true);
			EspadaInventarioMano.SetActive (false);
		}
		if (Arma == 2){
			EspadaMano.SetActive(true);
			PaloMano.SetActive (false);
			EspadaInventario.SetActive (true);
			EspadaInventarioMano.SetActive (true);
			PaloInventarioMano.SetActive (false);
		}
	}
}
