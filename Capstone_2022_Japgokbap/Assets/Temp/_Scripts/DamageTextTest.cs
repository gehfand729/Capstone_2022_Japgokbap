using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageTextTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    TextMeshPro text;
    private Color alpha;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destroyTime;
    public int damage;

    private void Start() {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }
    private void Update() {
        transform.Translate(new Vector3(0, moveSpeed *Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a , 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject(){
        Destroy(gameObject);
    }
}
