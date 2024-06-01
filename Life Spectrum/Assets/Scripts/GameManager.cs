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
using TMPro;
using Newtonsoft.Json;
using LIFESPECTRUM;



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

    private string key = "����±Ϳ�������";

    private Dictionary<string, GameObject> statUI;
    private TMP_Text ageText;
    private GameObject storyCard;
    private Material noneMaterial;

    [Header("���̺� ���� ���")] public string saveFilePath = null;
    [Header("���� ������")] public Stats stats;

    public Option[] nowOptions;

    public void Start()
    {
        saveFilePath = Application.persistentDataPath + "/LifeSpectrum.json";

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            GameObject.Find("NewGame").GetComponent<Button>().onClick.AddListener(() => StartCoroutine(IE_LoadScene("GameScene")));
            var button = GameObject.Find("LoadGame").GetComponent<Button>();
            button.onClick.AddListener(() => StartCoroutine(IE_LoadScene("GameScene", true)));
            button.gameObject.SetActive(false);

            if (LoadData() != null)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator IE_LoadScene(string sceneName, bool hasSaveFile = false)
    {
        if(SceneManager.GetSceneByName(sceneName) != null)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!op.isDone)
            {
                yield return null;
            }

            if(hasSaveFile == true)
            {
                stats = LoadData();
            }

            StartGame();
            yield break;

        }
        else
        {
            Debug.LogError("���� ���Ҵ�");
        }
    }

    public void StartGame()
    {
        stats = new Stats(50, 50, 50, 50, 100, 100, 100, 100, 0);

        if (GameSystem.Instance == null)
        {

        }
        GameSystem.Instance.StartFirstStory();

        ChangeStoryUI(GameSystem.Instance.nowStory);
        ChangeStatUI();
        ChangeAgeAndUI();
    }
    public void ChangeAgeAndUI()
    {
        if(stats.age < 6)
        {
            stats.age += 0.25f;
        }
        else
        {
            stats.age += 1;
        }

        if(ageText == null)
        {
            ageText = GameObject.FindWithTag("AgeText").GetComponent<TMP_Text>();
        }

        float quarter = stats.age - (int)stats.age ;

        if (quarter < 1)
        {
            if((int)stats.age < 1)
            {
                ageText.text = $"{Mathf.Abs(quarter * 100 / 25)} �б�";
            }
            else
            {
                ageText.text = $"{(int)stats.age}�� {quarter * 100 / 25} �б�";
            }
        }
        else
        {
            ageText.text = $"{stats.age}��";
        }
    }
    public void ChangeStatUI()
    {
        if(statUI == null)
        {
           statUI = new Dictionary<string, GameObject>();
           var statGo = GameObject.FindGameObjectsWithTag("StatImage");

           foreach(GameObject st in statGo)
            {
                statUI.Add(st.name.Substring(5), st);
                Debug.Log(st.name.Substring(5));
            }
        }
        StartCoroutine(IE_ChageStatSlowLy(stats.statIntelligence, Enums.PlayerStats.Intelligence));
        StartCoroutine(IE_ChageStatSlowLy(stats.statStrength, Enums.PlayerStats.Strength));
        StartCoroutine(IE_ChageStatSlowLy(stats.statPersonality, Enums.PlayerStats.Personality));
        StartCoroutine(IE_ChageStatSlowLy(stats.statMoney, Enums.PlayerStats.Money));
    }
    public void ChangeStoryUI(StoryObject story,bool isDragging = false, float targetX = 0)
    {
        if(storyCard == null)
        {
            storyCard = GameObject.FindWithTag("StoryCard");
        }

        if(story.image == null)
        {
            if(noneMaterial == null)
            {
                noneMaterial = Resources.Load<Material>("Materials/Null/Null");
                GameObject.FindWithTag("StoryCardImage").transform.GetChild(0).GetComponent<MeshRenderer>().material = noneMaterial;
            }
            else
            {
                GameObject.FindWithTag("StoryCardImage").transform.GetChild(0).GetComponent<MeshRenderer>().material = noneMaterial;
            }
        }
        else
        {
            GameObject.FindWithTag("StoryCardImage").transform.GetChild(0).GetComponent<MeshRenderer>().material = story.image;
        }

        if (isDragging == false)
        {
            storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = story.titleText;
            storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>().text = story.storyText;
        }
        else
        {
            if(MathF.Abs(targetX) > 0.3f)
            {
                if(targetX <= 0)
                {
                    storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[1].optionText;
                }
                else
                {
                    storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[0].optionText;
                }
            }
            else
            {
                storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = story.titleText;
            }
        }
    }
    private IEnumerator IE_ChageStatSlowLy(float endValue, Enums.PlayerStats stat)
    {
        float elapsedTime = 0f;

        Image statUIImage;
        float maxValue;
        float startValue = 0f;

        if (stat == Enums.PlayerStats.Intelligence)
        {
            statUIImage = statUI["Intelligence"].GetComponent<Image>();
            maxValue = (float)stats.maxIntelligence;
        }
        else if (stat == Enums.PlayerStats.Personality)
        {
            statUIImage = statUI["Personality"].GetComponent<Image>();
            maxValue = (float)stats.maxPersonality;
        }
        else if (stat == Enums.PlayerStats.Strength)
        {
            statUIImage = statUI["Strength"].GetComponent<Image>();
            maxValue = (float)stats.maxStrength;
        }
        else if (stat == Enums.PlayerStats.Money)
        {
            statUIImage = statUI["Money"].GetComponent<Image>();
            maxValue = (float)stats.maxMoney;
        }
        else
        {
            yield break;
        }

        if(statUIImage.fillAmount <= 0 || statUIImage.fillAmount == 1)
        {
            startValue = 0f;
        }

        startValue = statUIImage.fillAmount;

        while (elapsedTime < 1f)
        {
            float newValue = Mathf.Lerp(startValue, endValue / maxValue, elapsedTime / 1f);
            Debug.Log("���� ��: " + newValue);

            if (stat == Enums.PlayerStats.Intelligence)
            {
                statUI["Intelligence"].GetComponent<Image>().fillAmount = newValue;
            }
            else if (stat == Enums.PlayerStats.Personality)
            {
                statUI["Personality"].GetComponent<Image>().fillAmount = newValue;
            }
            else if (stat == Enums.PlayerStats.Strength)
            {
                statUI["Strength"].GetComponent<Image>().fillAmount = newValue;
            }
            else if (stat == Enums.PlayerStats.Money)
            {
                statUI["Money"].GetComponent<Image>().fillAmount = newValue;
            }
            else
            {
                yield break;
            }

            elapsedTime += 0.1f;

            yield return new WaitForSeconds(0.1f);
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
