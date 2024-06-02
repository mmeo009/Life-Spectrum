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

    private Dictionary<string, TextUIObject> textUIObjcets;
    private Dictionary<string, GameObject> statUI;
    private Dictionary<string, BuffController> buffUI = new Dictionary<string, BuffController>();

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
            stats.age += 0.5f;
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
                ageText.text = $"{Mathf.Abs(quarter * 100 / 50)} �б�";
            }
            else
            {
                ageText.text = $"{(int)stats.age}�� {quarter * 100 / 50} �б�";
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

        if(textUIObjcets == null)
        {
            textUIObjcets = new Dictionary<string, TextUIObject> ();
            FindTextUIObjects();
        }

        if (story.image == null)
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
            if (FindTextUIObjects("Text_Title"))
            {
                textUIObjcets["Text_Title"].text.text = story.titleText;
            }
            else
            {
                Debug.LogError("Can`t find Text_Title!");
            }

            if(FindTextUIObjects("Text_Story"))
            {
                textUIObjcets["Text_Story"].text.text = story.storyText;
            }
            else
            {
                Debug.LogError("Can`t find Text_Story!");
            }

            if (FindTextUIObjects("Right_Option"))
            {
                textUIObjcets["Right_Option"].TextObjectMain.SetActive(false);
            }
            else
            {
                Debug.LogError("Can`t find Right_Option!");
            }

            if (FindTextUIObjects("Left_Option"))
            {
                textUIObjcets["Left_Option"].TextObjectMain.SetActive(false);
            }
            else
            {
                Debug.LogError("Can`t find Left_Option!");
            }

        }
        else
        {
            if(MathF.Abs(targetX) > 0.3f)
            {
                if(targetX <= 0)
                {
                    if (FindTextUIObjects("Right_Option"))
                    {
                        textUIObjcets["Right_Option"].TextObjectMain.SetActive(true);
                        textUIObjcets["Right_Option"].textUI.text = GameSystem.Instance.nowOptions[1].optionText;
                    }
                    else
                    {
                        Debug.LogError("Can`t find Right_Option!");
                    }

                    if (FindTextUIObjects("Left_Option"))
                    {
                        textUIObjcets["Left_Option"].TextObjectMain.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("Can`t find Left_Option!");
                    }
                }
                else
                {
                    if (FindTextUIObjects("Left_Option"))
                    {
                        textUIObjcets["Left_Option"].TextObjectMain.SetActive(true);
                        textUIObjcets["Left_Option"].textUI.text = GameSystem.Instance.nowOptions[0].optionText;
                    }
                    else
                    {
                        Debug.LogError("Can`t find Left_Option!");
                    }

                    if (FindTextUIObjects("Right_Option"))
                    {
                        textUIObjcets["Right_Option"].TextObjectMain.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("Can`t find Right_Option!");
                    }
                }
            }
            else
            {
                if (FindTextUIObjects("Right_Option"))
                {
                    textUIObjcets["Right_Option"].TextObjectMain.SetActive(false);
                }
                else
                {
                    Debug.LogError("Can`t find Right_Option!");
                }

                if (FindTextUIObjects("Left_Option"))
                {
                    textUIObjcets["Left_Option"].TextObjectMain.SetActive(false);
                }
                else
                {
                    Debug.LogError("Can`t find Left_Option!");
                }
            }
        }
    }
    public void ReFrashBuffUI()
    {
        foreach(Debuff debuff in GameSystem.Instance.myDebuffs)
        {
            if (buffUI.ContainsKey(debuff.debuffName))
            {
                buffUI[debuff.debuffName].ChangeTime();
            }
            else if(buffUI.ContainsKey(debuff.debuffName) == false)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefab/Buff_Icon");
                GameObject temp = Instantiate(prefab,GameObject.FindWithTag("BuffBG").transform);
                var bc = temp.GetComponent<BuffController>();
                bc.LoadMyState(debuff);
                buffUI.Add(debuff.debuffName, bc);
            }
        }
    }
    public void RemoveDeBuffUI(Debuff debuff)
    {
        var go = buffUI[debuff.debuffName].gameObject;
        buffUI.Remove(debuff.debuffName);
        Destroy(go);
    }
    private bool FindTextUIObjects(string key = null)
    {
        if(key != null)
        {
            if (textUIObjcets.ContainsKey(key))
            {
                return true;
            }
            else
            {
                switch(key)
                {
                    case "Text_Title":

                        TextUIObject tempTitle = new TextUIObject();
                        tempTitle.name = "Text_Title";
                        tempTitle.text = storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>();
                        tempTitle.TextObjectMain = storyCard;

                        textUIObjcets.Add("Text_Title", tempTitle);

                        return true;
                    case "Text_Story":

                        TextUIObject tempStory = new TextUIObject();
                        tempStory.name = "Text_Story";
                        tempStory.text = storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>();
                        tempStory.TextObjectMain = storyCard;

                        textUIObjcets.Add("Text_Story", tempStory);

                        return true;
                    case "Right_Option":

                        TextUIObject tempRight = new TextUIObject();
                        tempRight.name = "Right_Option";
                        tempRight.TextObjectMain = GameObject.FindWithTag("Right");
                        tempRight.textUI = tempRight.TextObjectMain.transform.Find("Text_Right").GetComponent<TMP_Text>();

                        textUIObjcets.Add("Right_Option", tempRight);
                        return true;
                    case "Left_Option":

                        TextUIObject tempLeft = new TextUIObject();
                        tempLeft.name = "";
                        tempLeft.TextObjectMain = GameObject.FindWithTag("Left");
                        tempLeft.textUI = tempLeft.TextObjectMain.transform.Find("Text_Left").GetComponent<TMP_Text>();

                        textUIObjcets.Add("Left_Option", tempLeft);
                        return true;
                    default: 
                        return false;

                }
            }

        }
        else
        {

            TextUIObject tempTitle = new TextUIObject();
            tempTitle.name = "Text_Title";
            tempTitle.text = storyCard.transform.Find("Text_Title").GetComponent<TextMeshPro>();
            tempTitle.TextObjectMain = storyCard;

            textUIObjcets.Add("Text_Title", tempTitle);

            TextUIObject tempStory = new TextUIObject();
            tempStory.name = "Text_Story";
            tempStory.text = storyCard.transform.Find("Text_Story").GetComponent<TextMeshPro>();
            tempStory.TextObjectMain = storyCard;

            textUIObjcets.Add("Text_Story", tempStory);

            TextUIObject tempRight = new TextUIObject();
            tempRight.name = "Right_Option";
            tempRight.TextObjectMain = GameObject.FindWithTag("Right");
            tempRight.textUI = tempRight.TextObjectMain.transform.Find("Text_Right").GetComponent<TMP_Text>();

            textUIObjcets.Add("Right_Option", tempRight);

            TextUIObject tempLeft = new TextUIObject();
            tempLeft.name = "";
            tempLeft.TextObjectMain = GameObject.FindWithTag("Left");
            tempLeft.textUI = tempLeft.TextObjectMain.transform.Find("Text_Left").GetComponent<TMP_Text>();

            textUIObjcets.Add("Left_Option", tempLeft);

            return true;
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

        foreach(Transform icon in statUIImage.transform)
        {
            icon.gameObject.SetActive(false);
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

            int isBuff = 0;

            if(newValue > startValue)
            {
                isBuff = 1;
            }
            else if(newValue < startValue)
            {
                isBuff = -1;
            }

            statUIImage.fillAmount = newValue;

            if (isBuff == 1)
            {
                statUIImage.transform.Find("Buff").gameObject.SetActive(true);
            }
            else if(isBuff == -1)
            {
                statUIImage.transform.Find("Debuff").gameObject.SetActive(true);
            }

            elapsedTime += 0.1f;

            yield return new WaitForSeconds(0.1f);
        }

        foreach (Transform icon in statUIImage.transform)
        {
            icon.gameObject.SetActive(false);
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

[System.Serializable]
public class TextUIObject
{
    public string name;
    public GameObject TextObjectMain;
    public TextMeshPro text;
    public TMP_Text textUI;
}