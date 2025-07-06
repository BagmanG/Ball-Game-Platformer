using UnityEngine;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    void Update()
    {
        // ��������� ������� ������� T
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // ������� ����� ��� ����������, ���� � ���
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // ���������� ��������� ����� ��� ����� �����
        int randomNumber = Random.Range(0, 1000000);

        // ��������� ��� �����
        string fileName = $"ru_{randomNumber}.png";
        string filePath = Path.Combine(folderPath, fileName);

        // ������ ��������
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"�������� ��������: {filePath}");
    }
}