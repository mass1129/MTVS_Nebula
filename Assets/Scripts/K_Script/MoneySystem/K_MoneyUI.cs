using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class K_MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public K_MoneySystem player; 
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(MoneyCoroutine());
        UpdateText();
        player.OnGoldAmountChanged += Instance_OnGoldAmountChanged;
 
    }
    IEnumerator MoneyCoroutine()
    {
        while (true)
        {
            player.MoneyLoad();
            yield return new WaitForSeconds(5f);
        }
    }
    private void OnDestroy()
    {
        player.OnGoldAmountChanged -= Instance_OnGoldAmountChanged;
    }
    private void UpdateText()
    {
        goldText.text = player.GetGoldAmount().ToString();

    }
    private void Instance_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }


}
