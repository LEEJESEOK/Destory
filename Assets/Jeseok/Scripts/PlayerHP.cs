using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;



[RequireComponent(typeof(Rigidbody))]
public class PlayerHP : MonoBehaviour
{
    public float explosionRange = 10f;

    public float maxHP;
    float currentHP;

    public Image hpGauge;
    public Text hpTextUI;

    PostProcessVolume Vol;
    Bloom bloom;
    Vignette Vig;
    GameObject process;

    public GameObject EffectDieExplosion;

    public GameObject fire;


    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        hpTextUI.text = "" + currentHP;
        process = GameObject.Find("Post process Volume");
        Vol = process.GetComponent<PostProcessVolume>();
        Vol.profile.TryGetSettings<Bloom>(out bloom);
        Vol.profile.TryGetSettings<Vignette>(out Vig);
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentHP / maxHP) * 100 <= 20 || currentHP! <= 0) //20프로 미만일때
        {
            //bloom
            bloom.enabled.Override(true);
            bloom.intensity.value = 30;
            fire.SetActive(true);

            //Vig
            Vig.enabled.Override(true);
        }
        else //그외 평상시
        {
            bloom.enabled.Override(true);
            bloom.intensity.value = 20;
            Vig.enabled.Override(false);
            fire.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.gameObject.name.Contains("BeamBullet"))
                other.gameObject.SetActive(false);
            else
                Destroy(other.gameObject);

            //TODO 대미지 수치 적용
            --currentHP;

            hpGauge.fillAmount = (float)currentHP / maxHP;
            hpTextUI.text = "" + currentHP;

            if (currentHP <= 0)
            {
                Die();

                bloom.enabled.Override(true);
                bloom.intensity.value = 50;
                fire.SetActive(false);
            }
        }
    }

    void Die()
    {
        GameManager.instance.gameState = GameManager.GameState.Die;


        // GameManager.instance.ExploseWithEffect(transform.position, explosionRange, EffectDieExplosion, ~layer);
        GameManager.instance.ExploseInDie(transform.position, explosionRange);

        GameManager.instance.Die();
        gameObject.SetActive(false);
    }
}
