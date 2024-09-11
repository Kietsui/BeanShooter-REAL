    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        public Image healthBar; // Reference to the fill image
        private Camera mainCamera;

        void Start()
        {
            // Find the main camera
            mainCamera = Camera.main;
        }

        void Update()
        {
            // Make the health bar face the camera
            FaceCamera();
        }

        public void UpdateHealthbar(float fraction)
        {
            healthBar.fillAmount = fraction;
        }

        void FaceCamera()
        {
            // Rotate the parent GameObject (which contains the bar, background, and fill) to face the camera
            Vector3 direction = transform.position - mainCamera.transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
