using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class BoostManager : Singleton<BoostManager>
    {
        public static bool isBoostSpeed;
        public static bool isBoostMetabolism;
        private static bool isBoostSpeedCooldown;
        private static bool isBoostMetabolismCooldown;

        private static int boostTime;
        private static int boostCooldownTime;

        [SerializeField]
        private Image _speedCooldown;

        [SerializeField]
        private Image _metabolismCooldown;

        private static Image metabolismCooldown;
        private static Image speedCooldown;

        [SerializeField]
        private TextMeshProUGUI _timerSpeed;

        [SerializeField]
        private TextMeshProUGUI _timerMetabolism;

        private static TextMeshProUGUI timerSpeed;
        private static TextMeshProUGUI timerMetabolism;

        public static int AdrenalineGlands
        {
            get => PlayerPrefs.GetInt("AdrenalineGlands", 0);
            set => PlayerPrefs.SetInt("AdrenalineGlands", value);
        }

        public static int FastMetabolism
        {
            get => PlayerPrefs.GetInt("FastMetabolism", 0);
            set => PlayerPrefs.SetInt("FastMetabolism", value);
        }

        private void Start()
        {
            Snake.Instance.SetBloomAmount(0);
            speedCooldown = _speedCooldown;
            metabolismCooldown = _metabolismCooldown;
            timerSpeed = _timerSpeed;
            timerMetabolism = _timerMetabolism;
        }

        public static void UseSpeedBoost()
        {
            if (isBoostSpeedCooldown) return;
            isBoostSpeed = true;
            isBoostSpeedCooldown = true;
            timerSpeed.gameObject.SetActive(true);
            TutorialManager.Instance.HideBoostHand();
            Instance.StartCoroutine(BoostSpeed(0));
            if (!isBoostMetabolism)
                Instance.StartCoroutine(StartBloom());
        }

        private static IEnumerator StartBloom()
        {
            float i = 0;
            const float step = 0.01f;
            while (i < 0.05f)
            {
                yield return null;
                i += step;
                Snake.Instance.SetBloomAmount(i);
            }
        }

        private static IEnumerator StopBloom()
        {
            float i = 0.05f;
            const float step = 0.01f;
            while (i > 0)
            {
                yield return null;
                i -= step;
                Snake.Instance.SetBloomAmount(i);
            }
        }

        private static IEnumerator BoostSpeed(int boost)
        {
            int i = 0;
            while (i < boostTime)
            {
                i++;
                switch (boost)
                {
                    case 0:
                        speedCooldown.fillAmount = (float) i / boostTime;
                        timerSpeed.text = TimeSpan.FromSeconds(boostTime - i).ToString(@"mm\:ss");
                        break;
                    case 1:
                        metabolismCooldown.fillAmount = (float) i / boostTime;
                        timerMetabolism.text = TimeSpan.FromSeconds(boostTime - i).ToString(@"mm\:ss");
                        break;
                }

                yield return new WaitForSecondsRealtime(1f);
            }


            switch (boost)
            {
                case 0:
                    isBoostSpeed = false;
                    Instance.StartCoroutine(CooldownBoost(0));
                    break;
                case 1:
                    isBoostMetabolism = false;
                    Instance.StartCoroutine(CooldownBoost(1));
                    break;
            }

            if (!isBoostSpeed && !isBoostMetabolism)
            {
                Instance.StartCoroutine(StopBloom());
            }
        }

        private static IEnumerator CooldownBoost(int boost)
        {
            int i = 0;
            while (i < boostCooldownTime)
            {
                i++;
                switch (boost)
                {
                    case 0:
                        timerSpeed.text = TimeSpan.FromSeconds(boostCooldownTime - i).ToString(@"mm\:ss");
                        speedCooldown.fillAmount = (float) (boostCooldownTime - i) / boostCooldownTime;
                        break;
                    case 1:
                        timerMetabolism.text = TimeSpan.FromSeconds(boostCooldownTime - i).ToString(@"mm\:ss");
                        metabolismCooldown.fillAmount = (float) (boostCooldownTime - i) / boostCooldownTime;
                        break;
                }

                yield return new WaitForSecondsRealtime(1f);
            }

            switch (boost)
            {
                case 0:
                    isBoostSpeedCooldown = false;
                    timerSpeed.gameObject.SetActive(false);
                    break;
                case 1:
                    isBoostMetabolismCooldown = false;
                    timerMetabolism.gameObject.SetActive(false);
                    break;
            }
        }

        public static void UseMetabolismBoost()
        {
            if (isBoostMetabolismCooldown) return;
            isBoostMetabolism = true;
            isBoostMetabolismCooldown = true;
            timerMetabolism.gameObject.SetActive(true);
            TutorialManager.Instance.HideBoostHand();
            Instance.StartCoroutine(BoostSpeed(1));
            if (!isBoostSpeed)
                Instance.StartCoroutine(StartBloom());
        }

        public static void LoadValues()
        {
            boostTime = (int) RemoteConfig.GetDouble("Boost_Length");
            boostCooldownTime = (int) RemoteConfig.GetDouble("Boost_Cooldown");
        }
    }
}