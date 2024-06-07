using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private float MaxBgmVolume;
    [SerializeField] private float MaxSfxVolume;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    public void Start()
    {
        bgmSlider.value = SoundManager.Instance.bgmVolume / MaxBgmVolume;
        sfxSlider.value = SoundManager.Instance.sfxVolume / MaxSfxVolume;
        bgmSlider.onValueChanged.AddListener(delegate { BgmValueChangeCheck(); });
        sfxSlider.onValueChanged.AddListener(delegate { SfxValueChangeCheck(); });
    }

    public void BgmValueChangeCheck()
    {
        SoundManager.Instance.ModifyBgmVolume(bgmSlider.value * MaxBgmVolume);
    }

    public void SfxValueChangeCheck()
    {
        SoundManager.Instance.ModifySfxVolume(sfxSlider.value * MaxSfxVolume);
    }
}
