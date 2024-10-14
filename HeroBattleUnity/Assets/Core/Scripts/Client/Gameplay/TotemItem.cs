using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemItem : MonoBehaviour
{

    public Image icon;
    public GameObject isSelectedLabel;
    public bool isTotemSelected;
    public Totem data;
    public TotemManager manager;
    public float spacing = 14;
    public float size = 80;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnPointerEnter()
    {
        manager.OnPointerEnter(this);
    }


    public void OnPointerDown()
    {
        manager.OnPointerDown(this);
    }

    public void Initializa(TotemManager manager)
    {
        this.manager = manager;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void SetTotem(Totem data)
    {
        this.data = data;
        isTotemSelected = false;
        SetTotemUnselected();
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2()
        {
            x = data.x > 7 ? (data.x - 7) * (size + spacing) + 0.5f * (spacing + size) : -((7 - data.x) * (size + spacing) - 0.5f * (spacing + size)),
            y = data.y == 0 ? (spacing + size) * 0.5f : -(spacing + size) * 0.5f
        };
        icon.sprite = manager.sprites[data.value];
    }

    public void SetTotemSelected()
    {
        isSelectedLabel.gameObject.SetActive(true);
    }

    public void SetTotemUnselected()
    {
        isSelectedLabel.gameObject.SetActive(false);
    }

    public void MoveTotemTo(int x, int y, float duration)
    {
        StartCoroutine(IEMoveTotemTo(x, y, duration));
    }


    IEnumerator IEMoveTotemTo(int x, int y, float duration)
    {
        float targetX = x > 7 ? (x - 7) * (size + spacing) + 0.5f * (spacing + size) : -((7 - x) * (size + spacing) - 0.5f * (spacing + size));
        RectTransform rectTransform = GetComponent<RectTransform>();
        float startX = rectTransform.anchoredPosition.x;
        float startY = rectTransform.anchoredPosition.y;
        Vector2 pos = new Vector2(startX, startY);
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / duration;
            pos.x = Mathf.Lerp(startX, targetX, progress);
            rectTransform.anchoredPosition = pos;
            yield return null;
        }
        pos.x = targetX;
        rectTransform.anchoredPosition = pos;
        data.x = x;
    }

}
