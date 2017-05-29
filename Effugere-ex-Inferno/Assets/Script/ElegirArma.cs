using UnityEngine;
using System.Collections;

public class ElegirArma : MonoBehaviour {

	public int Arma;
	public NumeroArma Armamento;

	void OnMouseDown () {
		if (Arma == 1) {
			Armamento.Arma = 1;
		}
		if (Arma == 2) {
			Armamento.Arma = 2;
		}
	}
}
