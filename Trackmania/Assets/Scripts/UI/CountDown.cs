using System.Collections;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDown;
    [SerializeField] private int _delayStart;

    public IEnumerator CountDownStart()
    {
        int countDownValue = _delayStart;
        _countDown.text = countDownValue.ToString();

        while(countDownValue > 0)
        {
            yield return new WaitForSeconds(1f);
            _countDown.text = (--countDownValue).ToString();
        }

        _countDown.text = "START!";

        yield return new WaitForSeconds(1f);

        _countDown.text = "";

        GameManager.RaceStart();
    }
}
