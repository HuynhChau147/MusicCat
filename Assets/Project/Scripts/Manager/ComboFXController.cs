using UnityEngine;
using TMPro;
using DG.Tweening;

public class ComboFXController : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI comboText;
    public Transform comboRoot;

    [Header("Particles")]
    public ParticleSystem normalHitFX;
    public ParticleSystem milestoneFX;
    public ParticleSystem superFX;

    [Header("Settings")]
    public float baseScale = 1f;
    public float punchScale = 0.3f;

    private int currentCombo;

    private Material comboMat;

    void Awake()
    {
        comboMat = comboText.fontMaterial;
    }

    public void OnComboChanged(int combo, bool isPerfect)
    {
        currentCombo = combo;

        comboText.text = "x" + combo.ToString();

        // 1. SCALE POP
        PlayScaleFX(isPerfect);

        // 2. COLOR / GLOW
        UpdateColor();

        // 3. PARTICLE
        PlayParticle();

        // 4. MILESTONE
        CheckMilestone();

        // 5. SHAKE nhẹ
        PlayShake();
    }

    public void ShowText(bool isShow)
    {
        comboText.gameObject.SetActive(isShow);
    }

    void PlayScaleFX(bool isPerfect)
    {
        float scale = baseScale + punchScale;

        if (isPerfect)
            scale += 0.2f;

        comboRoot.DOKill();
        comboRoot.localScale = Vector3.one * baseScale;

        comboRoot.DOScale(scale, 0.08f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                comboRoot.DOScale(baseScale, 0.08f);
            });
    }

    void UpdateColor()
    {
        // Simple gradient logic
        if (currentCombo < 10)
        {
            comboText.color = Color.white;
            SetGlow(0f);
        }
        else if (currentCombo < 20)
        {
            comboText.color = Color.cyan;
            SetGlow(0.2f);
        }
        else if (currentCombo < 30)
        {
            comboText.color = Color.yellow;
            SetGlow(0.4f);
        }
        else if (currentCombo < 50)
        {
            comboText.color = new Color(1f, 0.5f, 0f); // orange
            SetGlow(0.7f);
        }
        else
        {
            // 🌈 Rainbow effect
            float t = Time.time * 5f;
            comboText.color = Color.HSVToRGB(Mathf.PingPong(t, 1), 1, 1);
            SetGlow(1.2f);
        }
    }

    void SetGlow(float value)
    {
        if (comboMat != null)
        {
            comboMat.SetFloat("_GlowPower", value);
        }
    }

    int lastMilestoneCombo = -1;

    void PlayParticle()
    {
        normalHitFX?.Play();

        if (currentCombo >= 10 && currentCombo % 5 == 0)
        {
            if (currentCombo == lastMilestoneCombo) return;

            lastMilestoneCombo = currentCombo;

            if (currentCombo < 20)
            {
                BigPunch(1.2f);
                milestoneFX?.Play();
            }
            else
            {
                BigPunch(1.5f);
                milestoneFX?.Play();
                superFX?.Play();
            }
        }
    }

    void CheckMilestone()
    {
        if (currentCombo == 50)
        {
            BigPunch(1.5f);
            milestoneFX?.Play();
        }
        else if (currentCombo == 100)
        {
            BigPunch(2f);
            superFX?.Play();
        }
    }

    void BigPunch(float scaleMultiplier)
    {
        comboRoot.DOKill();
        comboRoot.DOScale(baseScale * scaleMultiplier, 0.15f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                comboRoot.DOScale(baseScale, 0.2f);
            });
    }

    void PlayShake()
    {
        float strength = Mathf.Clamp(currentCombo / 100f, 0f, 1f);

        comboRoot.DOShakePosition(0.1f, strength * 10f, 10, 90, false, true);
    }
}