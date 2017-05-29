using UnityEngine;
using System.Collections;

public class ArmaEncontrada : MonoBehaviour {

	public GameObject ArmaSuelo;

	public int NumeroArmamento;
	public NumeroArma Numero;

	void OnTriggerEnter () {
		if (NumeroArmamento == 1){
			Numero.Arma = 1;
			ArmaSuelo.SetActive (false);
		}
		if (NumeroArmamento == 2) {
			Numero.Arma = 2;
			ArmaSuelo.SetActive (false);
		}
	}
}
