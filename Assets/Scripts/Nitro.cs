using UnityEngine;
using UnityEngine.UI;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroForce = 300f;
    [SerializeField] private float nitroDrain = 30f;
    [SerializeField] private float nitroRecharge = 10f;
    [SerializeField] private Slider nitroSlider;

    private float currentNitro;
    private bool isUsingNitro;

    public float CurrentNitro => currentNitro;
    public bool IsUsingNitro => isUsingNitro;
    public float NitroForce => nitroForce;

    private void Start()
    {
        currentNitro = maxNitro;
        if (nitroSlider != null)
        {
            nitroSlider.maxValue = maxNitro;
            nitroSlider.value = currentNitro;
        }
    }

    private void Update()
    {
        if (isUsingNitro && currentNitro > 0)
        {
            currentNitro -= nitroDrain * Time.deltaTime;
            if (currentNitro <= 0)
            {
                currentNitro = 0;
                StopNitro();
            }
        }
        else if (!isUsingNitro && currentNitro < maxNitro)
        {
            currentNitro += nitroRecharge * Time.deltaTime;
            if (currentNitro > maxNitro)
            {
                currentNitro = maxNitro;
            }
        }

        if (nitroSlider != null)
        {
            nitroSlider.value = currentNitro;
        }
    }

    public void StartNitro()
    {
        if (currentNitro > 0)
        {
            isUsingNitro = true;
        }
    }

    public void StopNitro()
    {
        isUsingNitro = false;
    }
}