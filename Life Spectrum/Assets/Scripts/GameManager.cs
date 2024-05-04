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

    private string key = "평오는귀여워히히";

    private Dictionary<string, GameObject> statUI;
    private TMP_Text ageText;
    private GameObject storyCard;

    [Header("세이브 파일 경로")] public string saveFilePath = null;
    [Header("게임 데이터")] public Stats stats;

    public Option[] nowOptions;


    public void Start()
    {
        saveFilePath = Application.persistentDataPath + "/LifeSpectrum.json";
        StartGame();
    }

    public void StartGame()
    {
        stats = new Stats(50, 50, 50, 50, 100, 100, 100, 100, 0);
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

        float quarter = (int)stats.age - stats.age;

        if (quarter < 1)
        {
            ageText.text = $"{(int)stats.age}살 {quarter * 100 / 25} 분기";
        }
        else
        {
            ageText.text = $"{stats.age}살";
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

        statUI["Intelligence"].GetComponent<Image>().fillAmount = stats.statIntelligence / stats.maxIntelligence;
        statUI["Personality"].GetComponent<Image>().fillAmount = stats.statPersonality / stats.maxPersonality;
        statUI["Strength"].GetComponent<Image>().fillAmount = stats.statStrength / stats.maxStrength;
        statUI["Money"].GetComponent<Image>().fillAmount = stats.statMoney / stats.maxMoney;
    }
    public void ChangeStoryUI(StoryObject story,bool isDragging = false, bool isLeft = false)
    {
        if(storyCard == null)
        {
            storyCard = GameObject.FindWithTag("StoryCard");
        }

        storyCard.transform.Find("StoryCardImage").GetComponent<MeshRenderer>().material = story.image;

        if (isDragging == false)
        {
            storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = story.titleText;
            storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>().text = story.storyText;
        }
        else
        {
            if(isLeft == true)
            {
                storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[1].optionText;
                storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[1].optionDetailText;
            }
            else
            {
                storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[0].optionText;
                storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>().text = GameSystem.Instance.nowOptions[0].optionDetailText;
            }
        }

    }

    // 게임 데이터 저장
    private void SaveGmaeData(Stats data)
    {
        // JSON 직렬화
        string jsonData = JsonConvert.SerializeObject(data);

        // 데이터를 바이트 배열로 변환
        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(jsonData);

        // 암호화
        byte[] encryptedBytes = Encrypt(bytesToEncrypt);

        // 암호화된 데이터를 Base64 문자열로 변환
        string encryptedData = Convert.ToBase64String(encryptedBytes);

        // 파일 저장
        File.WriteAllText(saveFilePath, encryptedData);
    }

    // 게임 데이터 불러오기
    private Stats LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            // 파일에서 암호화된 데이터 읽기
            string encryptedData = File.ReadAllText(saveFilePath);

            // Base64문자열을 바이트 배열로 변환
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // 복호화
            byte[] decryptedBytes = Decrypt(encryptedBytes);

            // byte 배열을 문자열로 변환
            string jsonData = Encoding.UTF8.GetString(decryptedBytes);

            // JSON 파일 역 직렬화
            Stats data = JsonConvert.DeserializeObject<Stats>(jsonData);
            return data;
        }
        else
        {
            return null;
        }
    }

    // 데이터 암호화
    private byte[] Encrypt(byte[] plainBytes)
    {
        // AES 암호화 알고리즘 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 비트 (32 바이트)로 키 크기 조정
            aesAlg.IV = new byte[16];   // 초기화 벡터

            // 암호화 변환기 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // 메모리 스트림 생성
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // 암호화 스트림 생성
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // 데이터 쓰기
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    // 암호화된 데이터를 바이트 배열로 반환
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    // 데이터 복호화
    private byte[] Decrypt(byte[] encryptedBytes)
    {
        // AES 복호화 알고리즘 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = AdjustKeySize(key, 256); // 256 비트 (32 바이트)로 키 크기 조정
            aesAlg.IV = new byte[16];   // 초기화 벡터

            // 복호화 변환기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // 메모리 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                // 복호화 스트림 생성
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    // 복호화된 데이터를 담을 바이트 배열 생성
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];

                    // 데이터 읽기
                    int decryptedByteCount = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);

                    // 실제로 읽힌 크기 만큼의 바이트 배열을 반환
                    return decryptedBytes.Take(decryptedByteCount).ToArray();
                }
            }
        }
    }

    // 데이터 사이즈 조절
    private byte[] AdjustKeySize(string key, int keySize)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // 키를 바이트 배열로 변환
        Array.Resize(ref keyBytes, keySize / 8); // 원하는 키 크기에 맞게 배열 크기 조정
        return keyBytes; // 조정된 바이트를 반환
    }

}
