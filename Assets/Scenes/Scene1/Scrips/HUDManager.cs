using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot()?.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

            WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetWeaponSprite(WeaponModel model)
    {
        GameObject obj = null;
        switch (model)
        {
            case WeaponModel.PistolM1911:
                obj = Resources.Load<GameObject>("PistolM1911_Weapon");
                break;
            case WeaponModel.AK47:
                obj = Resources.Load<GameObject>("AK47_Weapon");
                break;
        }
        return obj ? obj.GetComponent<SpriteRenderer>()?.sprite : null;
    }

    private Sprite GetAmmoSprite(WeaponModel model)
    {
        GameObject obj = null;
        switch (model)
        {
            case WeaponModel.PistolM1911:
                obj = Resources.Load<GameObject>("Pistol_Ammo");
                break;
            case WeaponModel.AK47:
                obj = Resources.Load<GameObject>("Rifle_Ammo");
                break;
        }
        return obj ? obj.GetComponent<SpriteRenderer>()?.sprite : null;
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
                return weaponSlot;
        }
        return null;
    }
}



/*using UnityEngine;
using TMPro;                  // For TextMeshProUGUI
using UnityEngine.UI;         // For Image


public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;  // The transparent sprite

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

            WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
        }
    }

    private Sprite GetWeaponSprite(WeaponModel model) 
    {
        switch(model)
        {
            case WeaponModel.PistolM1911:
                return Instantiate(Resources.Load<GameObject>("PistolM1911_Weapon")).GetComponet<SpriteRenderer>().sprite;

            case WeaponModel.AK47:
                return Instantiate(Resources.Load<GameObject>("AK47_Weapon")).GetComponet<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }


    private Sprite GetAmmoSprite(WeaponModel model)
    {
        switch (model)
        {
            case WeaponModel.PistolM1911:
                return Instantiate(Resources.Load<GameObject>("Pistol_Ammo")).GetComponet<SpriteRenderer>().sprite;

            case WeaponModel.AK47:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponet<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }

        // this will never happen, just a dummy return statement
        return null;    
    }

}
*/