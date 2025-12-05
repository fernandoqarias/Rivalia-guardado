using UnityEngine;
using UnityEngine.UI;

public class PieceUI : MonoBehaviour
{
    public BasePiece target;

    [Header("Slider References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider energySlider;

    private Vector3 offset = new Vector3(0, 1.0f, 0);

    private void Start()
    {
        if (target != null)
        {
            healthSlider.maxValue = target.maxHealth;
            energySlider.maxValue = target.maxEnergy;

            healthSlider.value = target.currentHealth;
            energySlider.value = target.currentEnergy;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.transform.position + offset;

        healthSlider.value = target.currentHealth;
        energySlider.value = target.currentEnergy;
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
