using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public GameObject sun;
    public GameObject moon;
    public GameObject stars; // Reference to stars GameObject
    public float offsetRotation;
    public Light dirLight;

    [Header("Cycle Settings")]
    [Range(0f, 24f)]
    public float timeOfDay = 0f; // Time of day, where 0 is midnight and 12 is noon
    public float cycleSpeed = 1f; // Speed of the day-night cycle

    [Header("Directional Light Settings")]
    public Color dayColor = Color.white; // Light color at noon
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // Light color at midnight
    public float dayIntensity = 1f; // Light intensity at noon
    public float nightIntensity = 0.1f; // Light intensity at midnight

    [Header("Texture Settings")]
    public Material unlitMaterial; // Reference to the unlit material
    public Vector2 midnightOffset = Vector2.zero; // Offset at midnight
    public Vector2 middayOffset = new Vector2(0.4f, 0); // Offset at midday

    [Header("Star Settings")]
    public Material starMaterial; // Material for the stars

    void Update()
    {
        // Increment timeOfDay based on cycleSpeed and ensure it wraps around 24 hours
        timeOfDay += Time.deltaTime * cycleSpeed;
        if (timeOfDay > 24f) timeOfDay -= 24f;

        // Determine blend factor (0 is night, 1 is day)
        float blendFactor = Mathf.Clamp01(Mathf.Cos((timeOfDay / 24f) * Mathf.PI * 2) * -0.5f + 0.5f);

        // Interpolate between midnightOffset and middayOffset based on blendFactor
        Vector2 currentOffset = Vector2.Lerp(midnightOffset, middayOffset, blendFactor);
        unlitMaterial.SetTextureOffset("_MainTex", currentOffset);

        // Set the rotation angles based on the time of day
        float rotationAngle = (timeOfDay / 24f) * 360f; // Calculate rotation based on time of day

        // Rotate the sun and moon around the X and Z axes to simulate a day-night cycle
        if (sun != null)
        {
            sun.transform.rotation = Quaternion.Euler(rotationAngle + offsetRotation, 0, 0);
        }

        if (moon != null)
        {
            moon.transform.rotation = Quaternion.Euler((rotationAngle + offsetRotation + 180), 0, 0); // Rotate in the opposite direction
        }

        // Rotate the stars at a slower speed than the sun
        if (stars != null)
        {
            stars.transform.rotation = Quaternion.Euler(0, rotationAngle + offsetRotation, 0);
        }

        // Update the Directional Light settings based on blendFactor
        if (dirLight != null)
        {
            dirLight.color = Color.Lerp(nightColor, dayColor, blendFactor);
            dirLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, blendFactor);
        }

        // Control the opacity of the stars
        if (starMaterial != null)
        {
            float starOpacity = 1f - blendFactor; // Stars are more visible at night
            Color starColor = starMaterial.color;
            starColor.a = starOpacity;
            starMaterial.color = starColor;
        }
    }
}
