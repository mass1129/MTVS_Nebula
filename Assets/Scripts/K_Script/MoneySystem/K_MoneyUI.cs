using System.Collections;
using System.Collections.Generic;

using TMPro;
using Photon.Pun;

public class K_MoneyUI : MonoBehaviourPun
{
    public TextMeshProUGUI goldText;
    public K_MoneySystem player; 
    bool isAddedEvent = false;
    // Start is called before the first frame update
  

    private void OnEnable()
    {
        if(!photonView.IsMine) return;
        if (!isAddedEvent)
        {
            player.OnGoldAmountChanged += Instance_OnGoldAmountChanged;
            isAddedEvent = true;
        }
        UpdateText();

    }
    private void OnDestroy()
    {
        if (!player.photonView.IsMine) return;
        player.OnGoldAmountChanged -= Instance_OnGoldAmountChanged;
    }
    private void UpdateText()
    {
        goldText.text = player.goldAmount.ToString();

    }
    private void Instance_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }


}
