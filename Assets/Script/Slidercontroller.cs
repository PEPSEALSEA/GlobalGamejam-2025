using UnityEngine;
using UnityEngine.UI;

public class Slidercontroller : MonoBehaviour
{
    public Slider slider; // ลิงก์ Slider
    public RectTransform imageTransform; // ลิงก์ RectTransform ของภาพ

    private Vector2 initialPosition; // ตำแหน่งเริ่มต้นของภาพ

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้น
        if (imageTransform != null)
        {
            initialPosition = imageTransform.anchoredPosition;
        }
    }

    void Update()
    {
        if (slider != null && imageTransform != null)
        {
            // ปรับตำแหน่งของภาพตามค่า Slider
            float xOffset = slider.value * (imageTransform.rect.width - slider.GetComponent<RectTransform>().rect.width);
            imageTransform.anchoredPosition = initialPosition - new Vector2(xOffset, 0);
        }
    }
}
