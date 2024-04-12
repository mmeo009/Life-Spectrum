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
    public static GameManager Instance { get; private set; }    //싱글톤화

    private void Awake()
    {
        if (Instance)       // 게임 메니저가 존재 할 경우
        {
            Destroy(gameObject);        // 나중에 생긴 나는 지워
            return;         // 함수 종료
        }
        else                // 게임 메니저가 존재하지 않을 경우
        {
            transform.parent = null;        // 트렌스폼 페런츠 없애(내 부모 오브젝트 없애)
            Instance = this;                // 나는 Instance가 된다
            DontDestroyOnLoad(gameObject);  // 내가 들어 있는 오브젝트가 다음 씬으로 가도 파괴 되지 않음
        }
    }
    //=======================변수 선언=======================

    public string saveFilePath = null;              // 세이브 파일 경로
    [SerializeField] private Stats stats = new Stats();     // 게임 저장 정보 클래스

    private string key = "평오는귀여워히히";            // 암호화 키 (평오가 귀엽긴 하다구 후후)

    public GameObject OptionsWindow;                // 일시정지 시 나오는 창 오브젝트

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
    //======================== 함수 ========================
    public void ManagerAction(Enums.ActonType type, string sceneName = null)
    {

        if (type == Enums.ActonType.SceneMove)          // 만약에 엑션 타입이 씬 이동일 경우
        {
            if (sceneName != null)              // 만약에 씬 이름이 없을 경우
            {
                SceneManager.LoadScene("LoadingScene");     // 로딩씬으로 이동
                StartCoroutine(LoadingSceneAndFillLoadingBarCoroutine(sceneName)); // 이후 비동기로 다음씬 로드
            }
            else
            {
                Debug.LogError("씬 이름이 기입되지 않았습니다.");        // 씬이름이 없다고 오류 출력
                SceneManager.LoadScene("StartScene");                  // 시작씬으로 이동
            }

        }
        else if (type == Enums.ActonType.ExitGame)
        {
            Application.Quit();                 // 빌드된 게임에서만 동작
        }
        else if (type == Enums.ActonType.PauseGame)
        {
            if (Time.timeScale == 1)            // 시간이 정지하지 않았다면
            {
                Time.timeScale = 0;
                OptionsWindow.SetActive(true);
            }
            else                               // 시간이 정지 해 있다면
            {
                Time.timeScale = 1;
                OptionsWindow.SetActive(false);
            }
        }
        else if (type == Enums.ActonType.SaveGame)      // 만약에 엑션 타입이 게임 저장일 경우
        {
            SaveGmaeData(stats);                // 게임 저장
        }
        else if (type == Enums.ActonType.LoadGame)      // 만약에 엑션 타입이 게임 로드일 경우
        {
            stats = LoadData();                 // 저장된 데이터 로드
        }
    }

    // 다음 씬을 비동기로 로드해 오는 코루틴
    private IEnumerator LoadingSceneAndFillLoadingBarCoroutine(string sceneName)
    {
        Image loadingBar;           // 로딩바 이미지
        if (SceneManager.GetActiveScene().name != "LoadingScene")           // 만약에 이 씬이 로딩 씬이 아닐 경우
        {
            AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("LoadingScene");      // 로딩씬을 로딩해옴
            while (!loadLoadingScene.isDone)        // 씬 로딩이 완료 되기 전까지 돌아
            {
                yield return null; // Update() 끝나고 아래로 내려가
            }
        }

        loadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();       // 로딩바 이미지를 찾아봄
        if (loadingBar == null)             // 만약에 로딩바를 찾지 못했을 경우
        {
            Debug.LogError("로딩바를 찾을 수 없습니다.");      // 로딩바를 찾지 못했음을 출력
            yield break;                // 코루틴 탈출~~~!
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);         // 비동기 작업 생성(sceneName)로드 해오기
        op.allowSceneActivation = false;                // 로드가 되자마자 씬으로 전환 x
        float timer = 0f;           // 타이머 생성
        while (!op.isDone)          // 비동기 작업이 끝날때 까지 돌아
        {
            yield return null;      // Update() 끝나고 아래로 내려가

            if (op.progress <= 0.9f)        // 진행도가 0.9f보다 작거나 같을경우
            {
                loadingBar.fillAmount = op.progress;        // 로딩바를 진행도만큼 채워
            }
            else                            // 진행도가 0보다 클경우
            {
                timer += Time.unscaledDeltaTime;        // 타이머 늘려
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);            // 0,1사이로 타이머 만큼 늘려
                if (loadingBar.fillAmount >= 1f || op.isDone)                   // 만약 로딩바가 다 찼거나 씬 로딩이 끝났을 겨우
                {
                    op.allowSceneActivation = true;                 // 씬이동 활성화
                    yield break;                    // 코루틴 탈출~~~~~
                }
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
