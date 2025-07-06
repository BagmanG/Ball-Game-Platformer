using UnityEngine;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    void Update()
    {
        // Проверяем нажатие клавиши T
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // Создаем папку для скриншотов, если её нет
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Генерируем случайное число для имени файла
        int randomNumber = Random.Range(0, 1000000);

        // Формируем имя файла
        string fileName = $"ru_{randomNumber}.png";
        string filePath = Path.Combine(folderPath, fileName);

        // Делаем скриншот
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"Скриншот сохранен: {filePath}");
    }
}