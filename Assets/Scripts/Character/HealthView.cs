using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textHealth;

    public void SetTextHealth(int health)
    {
        _textHealth.text = "Health = " + health.ToString();
    }
}
