// using UnityEngine;
// using TMPro;

// public class GameTimer : MonoBehaviour
// {
//     public TextMeshProUGUI timerText;
//     private float elapsedTime;
//     private float timeLimit;
//     private bool isTimeUp = false;

//     void Start()
//     {
//         timeLimit = GameManager.instance != null ? GameManager.instance.timeLimit : 180f;
//     }

//     void Update()
//     {
//         if (!isTimeUp)
//         {
//             elapsedTime += Time.deltaTime;
//             UpdateTimerUI();

//             if (elapsedTime >= timeLimit)
//             {
//                 isTimeUp = true;
//                 GameManager.instance.TimeUp();
//             }
//         }
//     }

//     void UpdateTimerUI()
//     {
//         float remainingTime = Mathf.Max(timeLimit - elapsedTime, 0f);
//         int minutes = Mathf.FloorToInt(remainingTime / 60f);
//         int seconds = Mathf.FloorToInt(remainingTime % 60f);
//         int milliseconds = Mathf.FloorToInt((remainingTime * 1000) % 1000);
//         timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
//     }
// } 