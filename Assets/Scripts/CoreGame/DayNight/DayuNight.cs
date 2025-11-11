using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

[ExecuteAlways]
public class DayNightCycle2D_Simple : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayDuration = 120f;
    [Range(0f, 24f)] public float timeOfDay = 12f;
    public TextMeshProUGUI timeText;
    public bool autoRun = true;

    [Header("Light Settings")]
    public Light2D globalLight;
    public Gradient lightColorOverDay;
    public AnimationCurve intensityCurve;
    public Light2D playerLight;

    [Header("Real Time Settings")]
    public bool isRealTime = false;

    private void Awake()
    {
        // khoi tao gradient neu null
        if (lightColorOverDay == null || lightColorOverDay.colorKeys.Length == 0)
        {
            lightColorOverDay = new Gradient();
            lightColorOverDay.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(new Color(0.05f,0.05f,0.2f),0.005f),    // dem
                    new GradientColorKey(new Color(1f,0.5f,0.2f),0.25f),     // Binh minh
                    new GradientColorKey(new Color(1f,1f,0.8f),0.5f),        // trua
                    new GradientColorKey(new Color(1f,0.4f,0.2f),0.75f),     // Hoang hon
                    new GradientColorKey(new Color(0.05f,0.05f,0.2f),1f)     // dem
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f,0.005f),
                    new GradientAlphaKey(1f,0.25f),
                    new GradientAlphaKey(1f,0.5f),
                    new GradientAlphaKey(1f,0.75f),
                    new GradientAlphaKey(1f,1f)
                }
            );
        }

        // intensity curve ma dinh
        if (intensityCurve == null || intensityCurve.length == 0)
        {
            intensityCurve = new AnimationCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.25f, 0.7f),
                new Keyframe(0.5f, 1f),
                new Keyframe(0.75f, 0.7f),
                new Keyframe(1f, 0.2f)
            );
        }
    }

    private void Update()
    {
        if (isRealTime)
        {
            DayNightRealTime();
        }
        else
        {
            DayNightGame();
        }
    }

    private void DayNightRealTime()
    {
        // lay thoi gian he thong
        System.DateTime now = System.DateTime.Now;
        timeOfDay = now.Hour + now.Minute / 60f;

        float t = timeOfDay / 24f;

        /// 
        if (globalLight != null)
        {
            globalLight.color = lightColorOverDay.Evaluate(t);
            globalLight.intensity = intensityCurve.Evaluate(t);
        }

        //Debug.Log($"Game Time: {timeOfDay:0.00}m | Real Time: {now:HH:mm}");
    }

    private void DayNightGame()
    {
        // tang thoi gian lien tu, va lam muot
        if (autoRun && Application.isPlaying)
        {
            timeOfDay += (24f / dayDuration) * Time.deltaTime;
            timeOfDay %= 24f;
        }

        float t = timeOfDay / 24f;

        // anh sang chinh
        if (globalLight != null)
        {
            globalLight.color = lightColorOverDay.Evaluate(t);
            globalLight.intensity = intensityCurve.Evaluate(t);
        }
        //Debug.Log($"Game Time: {timeOfDay:0.00}m");
    }
    
    private void LateUpdate()
    {
        
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(timeOfDay);
            int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60f);
            timeText.text = $"{hours:00}:{minutes:00}";
        }

        // đồng bộ ánh sáng nhân vật
        // if (playerLight != null && globalLight != null)
        // {
        //     playerLight.color = globalLight.color;
        //     playerLight.intensity = globalLight.intensity;
        // }
    }
}
