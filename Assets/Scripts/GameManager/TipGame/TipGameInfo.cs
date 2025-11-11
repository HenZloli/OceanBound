using UnityEngine;
using TMPro;
using System.Collections;

public class TipGameInfo : MonoBehaviour
{
    public static TipGameInfo instance;
    [SerializeField] private TextMeshProUGUI tip_game_message;

    private Coroutine currentCoroutine;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (tip_game_message != null)
            tip_game_message.text = "";
        else
            Debug.LogWarning("⚠️ TipGameInfo: tip_game_message chưa được gán trong Inspector!");
    }

    public void TipMessage(string text)
    {
        if (tip_game_message == null) return;

        tip_game_message.text = text;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(TextDelete(4f));
    }

    private IEnumerator TextDelete(float delay)
    {
        yield return new WaitForSeconds(delay);
        tip_game_message.text = "";
    }
}
