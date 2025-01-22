using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private static int sortingOrder;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.DamageNumberPopup,position,Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }
    
    public void Setup(int damageAmount,bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if(!isCriticalHit)
        {
            textMesh.fontSize = 8;
            textColor = new Color(1f,0.7725f,0f);
        }
        else
        {
            textMesh.fontSize = 9;
            textColor = new Color(1f,0f,0f);
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(0.5f,1f) * 8f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 4f * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX *0.5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float increaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if(disappearTimer<0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if(textColor.a < 0 )
            {
                Destroy(gameObject);
            }
        }
    }
}
