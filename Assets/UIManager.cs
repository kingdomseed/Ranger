using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text _ammoText;

	public void UpdateAmmunation(int count)
    {
        _ammoText.text = "Ammo: " + count;
    }
}
