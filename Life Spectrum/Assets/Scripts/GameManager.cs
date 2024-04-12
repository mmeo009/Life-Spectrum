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
    public static GameManager Instance { get; private set; }    //�̱���ȭ

    private void Awake()
    {
        if (Instance)       // ���� �޴����� ���� �� ���
        {
            Destroy(gameObject);        // ���߿� ���� ���� ����
            return;         // �Լ� ����
        }
        else                // ���� �޴����� �������� ���� ���
        {
            transform.parent = null;        // Ʈ������ �䷱�� ����(�� �θ� ������Ʈ ����)
            Instance = this;                // ���� Instance�� �ȴ�
            DontDestroyOnLoad(gameObject);  // ���� ��� �ִ� ������Ʈ�� ���� ������ ���� �ı� ���� ����
        }
    }
    //=======================���� ����=======================

    public string saveFilePath = null;              // ���̺� ���� ���
    [SerializeField] private Stats stats = new Stats();     // ���� ���� ���� Ŭ����

    private string key = "����±Ϳ�������";            // ��ȣȭ Ű (����� �Ϳ��� �ϴٱ� ����)

    public GameObject OptionsWindow;                // �Ͻ����� �� ������ â ������Ʈ

    public void Start()
    {
        saveFilePath = Application.persistentDataPath + "/LifeSpectrum.json";
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveGmaeData(stats);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            stats = LoadData();
        }
    }
    //======================== �Լ� ========================
    public void ManagerAction(Enums.ActonType type, string sceneName = null)
    {

        if (type == Enums.ActonType.SceneMove)          // ���࿡ ���� Ÿ���� �� �̵��� ���
        {
            if (sceneName != null)              // ���࿡ �� �̸��� ���� ���
            {
                SceneManager.LoadScene("LoadingScene");     // �ε������� �̵�
                StartCoroutine(LoadingSceneAndFillLoadingBarCoroutine(sceneName)); // ���� �񵿱�� ������ �ε�
            }
            else
            {
                Debug.LogError("�� �̸��� ���Ե��� �ʾҽ��ϴ�.");        // ���̸��� ���ٰ� ���� ���
                SceneManager.LoadScene("StartScene");                  // ���۾����� �̵�
            }

        }
        else if (type == Enums.ActonType.ExitGame)
        {
            Application.Quit();                 // ����� ���ӿ����� ����
        }
        else if (type == Enums.ActonType.PauseGame)
        {
            if (Time.timeScale == 1)            // �ð��� �������� �ʾҴٸ�
            {
                Time.timeScale = 0;
                OptionsWindow.SetActive(true);
            }
            else                               // �ð��� ���� �� �ִٸ�
            {
                Time.timeScale = 1;
                OptionsWindow.SetActive(false);
            }
        }
        else if (type == Enums.ActonType.SaveGame)      // ���࿡ ���� Ÿ���� ���� ������ ���
        {
            SaveGmaeData(stats);                // ���� ����
        }
        else if (type == Enums.ActonType.LoadGame)      // ���࿡ ���� Ÿ���� ���� �ε��� ���
        {
            stats = LoadData();                 // ����� ������ �ε�
        }
    }

    // ���� ���� �񵿱�� �ε��� ���� �ڷ�ƾ
    private IEnumerator LoadingSceneAndFillLoadingBarCoroutine(string sceneName)
    {
        Image loadingBar;           // �ε��� �̹���
        if (SceneManager.GetActiveScene().name != "LoadingScene")           // ���࿡ �� ���� �ε� ���� �ƴ� ���
        {
            AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("LoadingScene");      // �ε����� �ε��ؿ�
            while (!loadLoadingScene.isDone)        // �� �ε��� �Ϸ� �Ǳ� ������ ����
            {
                yield return null; // Update() ������ �Ʒ��� ������
            }
        }

        loadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();       // �ε��� �̹����� ã�ƺ�
        if (loadingBar == null)             // ���࿡ �ε��ٸ� ã�� ������ ���
        {
            Debug.LogError("�ε��ٸ� ã�� �� �����ϴ�.");      // �ε��ٸ� ã�� �������� ���
            yield break;                // �ڷ�ƾ Ż��~~~!
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);         // �񵿱� �۾� ����(sceneName)�ε� �ؿ���
        op.allowSceneActivation = false;                // �ε尡 ���ڸ��� ������ ��ȯ x
        float timer = 0f;           // Ÿ�̸� ����
        while (!op.isDone)          // �񵿱� �۾��� ������ ���� ����
        {
            yield return null;      // Update() ������ �Ʒ��� ������

            if (op.progress <= 0.9f)        // ���൵�� 0.9f���� �۰ų� �������
            {
                loadingBar.fillAmount = op.progress;        // �ε��ٸ� ���൵��ŭ ä��
            }
            else                            // ���൵�� 0���� Ŭ���
            {
                timer += Time.unscaledDeltaTime;        // Ÿ�̸� �÷�
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);            // 0,1���̷� Ÿ�̸� ��ŭ �÷�
                if (loadingBar.fillAmount >= 1f || op.isDone)                   // ���� �ε��ٰ� �� á�ų� �� �ε��� ������ �ܿ�
                {
                    op.allowSceneActivation = true;                 // ���̵� Ȱ��ȭ
                    yield break;                    // �ڷ�ƾ Ż��~~~~~
                }
            }
        }
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
