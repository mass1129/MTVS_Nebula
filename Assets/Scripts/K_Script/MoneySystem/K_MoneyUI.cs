using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class K_MoneyUI : MonoBehaviourPun
{
    public TextMeshProUGUI goldText;
    public K_MoneySystem player; 
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (!player.photonView.IsMine) return;
        StartCoroutine(MoneyCoroutine());
        UpdateText();
        player.OnGoldAmountChanged += Instance_OnGoldAmountChanged;
 
    }
    IEnumerator MoneyCoroutine()
    {
        
        while (true&&photonView.IsMine)
        {
            player.MoneyLoad();
            yield return new WaitForSeconds(5f);
        }
    }
    private void OnDestroy()
    {
        if (!player.photonView.IsMine) return;
        player.OnGoldAmountChanged -= Instance_OnGoldAmountChanged;
    }
    private void UpdateText()
    {
        if (!photonView.IsMine) return;
        goldText.text = player.GetGoldAmount().ToString();

    }
    private void Instance_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        if (!photonView.IsMine) return;
        UpdateText();
    }


}
