using UnityEngine;
using UnityEngine.UI; // need this for ui elements 
using TMPro; // We need this for TextMeshPro

//this script controls one upgrade card in the UI
//each card:
//displays upgarde info
//detects when the player clicks it
//tell UpgradeManagers which upgrade was chosen

public class UpgradeCard : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Button selectButton;

    private UpgradeData myUpgrade; // stores the upgrade this card represents
    private UpgradeManager manager;// reference to UpgradeManager so i can notify is when clicked

    // this method is called by UpgradeManager when upgrades are presented
    public void Setup(UpgradeData data, UpgradeManager mgr)
    {
        myUpgrade = data;// store the upgrade data
        manager = mgr;//store the UpgradeManager reference

        //if the upgrade data exist
        if (data != null)
        {
            //set ui text
            titleText.text = data.upgradeName;
            descriptionText.text = data.description;
            //set icon if exists(is not for now:D )
            if (data.icon != null) iconImage.sprite = data.icon;
        }

        
        selectButton.onClick.RemoveAllListeners(); //removes old Listeners
        selectButton.onClick.AddListener(OnClicked);// add new Listeners
    }

    private void OnClicked()
    {
        if (manager != null)
        {
            manager.SelectUpgrade(myUpgrade);
        }
    }
}