using System.IO;
using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using LifeSpectrum;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    [Header("���̺� ���� ���")] public string saveFilePath = null;
    [Header("���� ������")] public Stats stats = new Stats();

    private string key = "����±Ϳ�������";

    public GameObject OptionsWindow;

    public Dictionary<int, StoryObject> storys = new Dictionary<int, StoryObject>();


    public void Start()
    {
        saveFilePath = Application.persistentDataPath + "/LifeSpectrum.json";
    }

    // ���� ������ ����
    private void SaveGmaeData(Stats data)
    {
        // JSON ����ȭ
        string jsonData = JsonConvert.SerializeObject(data);

        // �����͸� ����Ʈ �迭�� ��ȯ
        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(jsonData);

        // ��ȣȭ
        byte[] encryptedBytes = Encrypt(bytesToEncrypt);

        // ��ȣȭ�� �����͸� Base64 ���ڿ��� ��ȯ
        string encryptedData = Convert.ToBase64String(encryptedBytes);

        // ���� ����
        File.WriteAllText(saveFilePath, encryptedData);
    }

    // ���� ������ �ҷ�����
    private Stats LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(saveFilePath);

            // Base64���ڿ��� ����Ʈ �迭�� ��ȯ
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // ��ȣȭ
            byte[] decryptedBytes = Decrypt(encryptedBytes);

            // byte �迭�� ���ڿ��� ��ȯ
            string jsonData = Encoding.UTF8.GetString(decryptedBytes);

            // JSON ���� �� ����ȭ
            Stats data = JsonConvert.DeserializeObject<Stats>(jsonData);
            return data;
        }
        else
        {
            return null;
        }
    }

    // ������ ��ȣȭ
    private byte[] Encrypt(byte[] plainBytes)
    {
        // AES ��ȣȭ �˰��� ����
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 ��Ʈ (32 ����Ʈ)�� Ű ũ�� ����
            aesAlg.IV = new byte[16];   // �ʱ�ȭ ����

            // ��ȣȭ ��ȯ�� ����
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // �޸� ��Ʈ�� ����
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // ��ȣȭ ��Ʈ�� ����
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // ������ ����
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    // ��ȣȭ�� �����͸� ����Ʈ �迭�� ��ȯ
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    // ������ ��ȣȭ
    private byte[] Decrypt(byte[] encryptedBytes)
    {
        // AES ��ȣȭ �˰��� ����
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 ��Ʈ (32 ����Ʈ)�� Ű ũ�� ����
            aesAlg.IV = new byte[16];   // �ʱ�ȭ ����

            // ��ȣȭ ��ȯ�� ����
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // �޸� ��Ʈ�� ����
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                // ��ȣȭ ��Ʈ�� ����
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    // ��ȣȭ�� �����͸� ���� ����Ʈ �迭 ����
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];

                    // ������ �б�
                    int decryptedByteCount = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);

                    // ������ ���� ũ�� ��ŭ�� ����Ʈ �迭�� ��ȯ
                    return decryptedBytes.Take(decryptedByteCount).ToArray();
                }
            }
        }
    }

    // ������ ������ ����
    private byte[] AdjustKeySize(string key, int keySize)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Ű�� ����Ʈ �迭�� ��ȯ
        Array.Resize(ref keyBytes, keySize / 8); // ���ϴ� Ű ũ�⿡ �°� �迭 ũ�� ����
        return keyBytes; // ������ ����Ʈ�� ��ȯ
    }

}
