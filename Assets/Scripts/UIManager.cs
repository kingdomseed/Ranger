using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _coinImage;

	public void UpdateAmmunation(int count)
    {
        _ammoText.text = "Ammo: " + count;
    }

    public void UpdateCoinInventory(bool set)
    {
        _coinImage.gameObject.SetActive(set);
    }
}
