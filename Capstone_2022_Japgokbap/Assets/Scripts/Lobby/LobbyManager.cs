using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;

public class LobbyManager : MonoBehaviour
{
    #region Private

    private static LobbyManager m_instance;

    [Header ("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject lobbyPanel;

    [Header ("Login UI")]
    [SerializeField] private InputField idInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private InputField confirmPasswordInput;
    [SerializeField] private Toggle rememberMeToggle;

    [SerializeField] private Text statusText;
    [SerializeField] private Text userName;

    //플레이팹 로그인 정보 세팅
    [SerializeField] private GetPlayerCombinedInfoRequestParams InfoRequestParams;
    private PlayfabManager m_authService = PlayfabManager.Instance;

    [Header ("Lobby Settings")]
    [SerializeField] private bool isLoginSuccessed;
    private bool isCharacterTouched;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject[] cameraPoints;
    [SerializeField] private GameObject characterSelectedUi;
    private Vector3 velocity = Vector3.zero;

    [Header("Buttons")]
    [SerializeField] private GameObject changeUsernameButton;
    [SerializeField] private GameObject showLeaderboardButton;
    [SerializeField] private GameObject showShopButton;
    [SerializeField] private GameObject showInventoryButton;
    [SerializeField] private GameObject showSettingButton;

    [Header("Login Panels")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Change Usernames")]
    [SerializeField] private GameObject changeUsernameInput;
    [SerializeField] private Text changeUsernameInputText;
    [SerializeField] private GameObject changeUsernameSureButton;
    [SerializeField] private GameObject changeUsernamePanel;
    [SerializeField] private Text guestChangeUsernameText;

    [Header("Character Selects")]
    [SerializeField] private GameObject warriorCharacterInstance;
    [SerializeField] private GameObject archerCharacterInstance;
    [SerializeField] private GameObject warriorSelectedUi;
    [SerializeField] private GameObject archerSelectedUi;

    [Header("Shop & Inventory")]
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private GameObject[] inventoryItems;
    [SerializeField] private GameObject archerCharacterItem;
    [SerializeField] private GameObject purchaseSuccessMessage;
    [SerializeField] private GameObject purchaseFailureMessage;
    [SerializeField] private Text currentGoldInfoText;
    [SerializeField] private int currentHaveGold;

    #endregion

    #region Public

    public static LobbyManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<LobbyManager>();
            }

            return m_instance;
        }
    }

    #endregion

    #region "Temp"
    public static string selectName;
    #endregion

    #region Unity Callbacks

    public void Awake()
    {
        //재로그인시 초기화가 필요할 경우 경우
        ClearLoginFlag();

        //Set our remember me button to our remembered state.
        rememberMeToggle.isOn = m_authService.RememberMe;

        //Subscribe to our Remember Me toggle
        rememberMeToggle.onValueChanged.AddListener(
            (toggle) =>
            {
                m_authService.RememberMe = toggle;
            });
    }

    public void Start()
    {
        // Hide all our panels until we know what UI to display
        InitializeUI();

        // Subscribe to events that happen after we authenticate
        PlayfabManager.OnDisplayAuthentication += OnDisplayAuthentication;
        PlayfabManager.OnLoginSuccess += OnLoginSuccess;
        PlayfabManager.OnPlayFabError += OnPlayFaberror;

        // Set the data we want at login from what we chose in our meta data.
        m_authService.InfoRequestParams = InfoRequestParams;

        // Start the authentication process.
        m_authService.Authenticate();
    }

    public void Update() 
    {
        if (isLoginSuccessed)
            OnCharacterSelected();
    }

    #endregion

    #region Public Methods

    public void ClearLoginFlag()
    {
        //if (조건)
        //{
            m_authService.UnlinkSilentAuth();
            m_authService.ClearRememberMe();
            m_authService.AuthType = Authtypes.None;
        //}
    }

    public void InitializeUI()
    {
        registerPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void Gamestart()
    {
        //게임 시작
        //현재는 디버깅을 위해 씬 로딩
        //실제 구현 시 매칭 시작
        LoadingSceneManager.LoadScene("GameScene");
    }

    //디바이스 아이디 등을 통해 게스트로 로그인
    //silently authenticate을 사용하여 처리
    public void OnPlayAsGuestClicked()
    {
        statusText.text = "게스트로 로그인하는 중";

        m_authService.Authenticate(Authtypes.Silent);
    }

    /// <summary>
    /// Login Button means they've selected to submit a username (email) / password combo
    /// Note: in this flow if no account is found, it will ask them to register.
    /// </summary>
    public void OnLoginClicked()
    {
        statusText.text = string.Format("{0} 으로 로그인하는 중", idInput.text);

        m_authService.Email = idInput.text;
        m_authService.Password = passwordInput.text;
        m_authService.Authenticate(Authtypes.EmailAndPassword);
    }

    /// <summary>
    /// No account was found, and they have selected to register a username (email) / password combo.
    /// </summary>
    public void OnRegisterButtonClicked()
    {
        if (passwordInput.text != confirmPasswordInput.text)
        {
            statusText.text = "비밀번호가 일치하지 않습니다";
            return;
        }

        statusText.text = string.Format("{0} 으로 회원가입 하는 중", idInput.text);

        m_authService.Email = idInput.text;
        m_authService.Password = passwordInput.text;
        m_authService.Authenticate(Authtypes.RegisterPlayFabAccount);
    }

    /// <summary>
    /// They have opted to cancel the Registration process.
    /// Possibly they typed the email address incorrectly.
    /// </summary>
    public void OnCancelRegisterButtonClicked()
    {
        // Reset all forms
        idInput.text = string.Empty;
        passwordInput.text = string.Empty;
        confirmPasswordInput.text = string.Empty;

        // Show panels
        registerPanel.SetActive(false);
    }

    public void OnChangeUsernameButtonClicked()
    {
        changeUsernamePanel.SetActive(true);

        if ( m_authService.AuthType == Authtypes.Silent)
        {
            // 게스트 로그인 시 닉변 안됨
            Invoke("SetActiveFalseNicknamePanel", 3f);
        }
        else
        {
            guestChangeUsernameText.gameObject.SetActive(false);

            changeUsernameInput.SetActive(true);
            changeUsernameSureButton.SetActive(true);
        }
    }

    public void ChangeUsernameButtonClicked()
    {
        if (changeUsernameInputText.text != string.Empty)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = changeUsernameInputText.text
            }, result => {
                Debug.Log("The player's display name is now: " + result.DisplayName);
                userName.text = result.DisplayName;
                changeUsernamePanel.SetActive(false);
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }
    }

    public void OnActivePanelsButtonClicked(GameObject panel)
    {
        settingPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void OnDeactivePanelsButtonClicked(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void PurchaseItem()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        
        if (clickObject.transform.childCount == 0)
        {
            return;
        }

        var request = new PurchaseItemRequest()
        {
            CatalogVersion = "Characters",
            ItemId = "Archer Character",
            VirtualCurrency = "GD",
            Price = 1000
        };

        PlayFabClientAPI.PurchaseItem(request,
            (result) =>
            {
                purchaseSuccessMessage.SetActive(true);

                Invoke("SetActiveFalsePurchaseSuccessPanel", 2f);

                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                (result) =>
                {
                    currentHaveGold = result.VirtualCurrency["GD"];
                    currentGoldInfoText.text = string.Format("보유 골드 : " + currentHaveGold);
                },
                (error) =>
                {
                    //error
                });
            },
            (error) =>
            {
                purchaseFailureMessage.SetActive(true);

                Invoke("SetActiveFalsePurchaseFailurePanel", 2f);
            });
    }

    public void OnActiveShopPanel()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            (result) =>
            {
                currentHaveGold = result.VirtualCurrency["GD"];
                currentGoldInfoText.text = string.Format("보유 골드 : " + currentHaveGold);
            },
            (error) =>
            {
                //error
            });
    }

    public void OnActiveInventoryPanel()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            (result) =>
            {
                for (int i = 0; i < result.Inventory.Count; i++)
                {
                    if (inventoryItems[i].transform.childCount != 0)
                    {
                        return;
                    }

                    var inventoryItem = result.Inventory[i];

                    switch (inventoryItem.ItemId)
                    {
                        case "Archer Character" :
                            GameObject item = Instantiate(archerCharacterItem, inventoryItems[i].transform);
                            item.tag = "InventoryItem";
                            break;
                    }
                }
            },
            (error) =>
            {
                //error
            });
    }

    public void OnActiveInventoryItem()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        if (clickObject.transform.childCount != 0)
        {
            switch (clickObject.transform.GetChild(0).name)
            {
                case "Archer Character(Clone)" :
                    archerCharacterInstance.SetActive(true);
                    break;
            }
        }
    }

    public GameObject GetShopPanel()
    {
        return shopPanel;
    }

    #endregion

    #region Private Methods

    //성공적으로 로그인됨
    //로비로 이동
    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        isLoginSuccessed = true;
        Debug.LogFormat("{0} 으로 로그인하였습니다", result.PlayFabId);
        statusText.text = "";
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        if (m_authService.AuthType == Authtypes.Silent)
        {
            userName.text = result.InfoResultPayload.AccountInfo.Username ?? result.PlayFabId;

            return;
        }

        var request = new GetAccountInfoRequest { };
        PlayFabClientAPI.GetAccountInfo(request, GetAccountSuccess
            , error =>
                Debug.LogError(error.GenerateErrorReport()));
    }

    private void GetAccountSuccess(GetAccountInfoResult result)
    {
        if (result.AccountInfo.TitleInfo.DisplayName != null)
        {
            userName.text = result.AccountInfo.TitleInfo.DisplayName;
        }
        else
        {
            OnChangeUsernameButtonClicked();
        }
    }

    //로그인 에러 발생 처리
    private void OnPlayFaberror(PlayFabError error)
    {
        //There are more cases which can be caught, below are some
        //of the basic ones.
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                statusText.text = "이메일 주소가 유효하지 않습니다";
                break;
            case PlayFabErrorCode.InvalidPassword:
                statusText.text = "비밀번호가 유효하지 않습니다";
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                statusText.text = "이메일이나 비밀번호가 유효하지 않습니다";
                break;

            case PlayFabErrorCode.AccountNotFound:
                registerPanel.SetActive(true);
                return;
            default:
                statusText.text = error.GenerateErrorReport();
                break;
        }

        //Also report to debug console, this is optional.
        Debug.Log(error.Error);
        Debug.LogError(error.GenerateErrorReport());
    }

    /// Choose to display the Auth UI or any other action.
    private void OnDisplayAuthentication()
    {
        //Here we have choses what to do when AuthType is None.
        loginPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        //statusText.text = "";

        /*
         * Optionally we could Not do the above and force login silently
         * 
         * m_authService.Authenticate(Authtypes.Silent);
         * 
         * This example, would auto log them in by device ID and they would
         * never see any UI for Authentication.
         * 
         */
    }

    private void OnCharacterSelected()
    {
        if (Input.GetMouseButtonDown(0) && !isCharacterTouched)
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                switch (hit.collider.name)
                {
                    case "Warrior":
                        isCharacterTouched = true;
                        mainCamera.transform.position = cameraPoints[1].transform.position;
                        warriorSelectedUi.SetActive(true);
                        selectName = hit.collider.name;
                        break;
                    case "Archer" :
                        isCharacterTouched = true;
                        mainCamera.transform.position = cameraPoints[2].transform.position;
                        archerSelectedUi.SetActive(true);
                        selectName = hit.collider.name;
                        break;
                    case "Magacian" :
                        selectName = hit.collider.name;
                        break;
                    default :
                        return;
                }

                characterSelectedUi.SetActive(true);
            }
        } 
        else if (Input.GetMouseButtonDown(0) && isCharacterTouched)
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                switch (hit.collider.name)
                {
                    case "Warrior" :
                        isCharacterTouched = false;
                        mainCamera.transform.position = cameraPoints[0].transform.position;
                        warriorSelectedUi.SetActive(false);
                        break;
                    case "Archer" :
                        isCharacterTouched = false;
                        mainCamera.transform.position = cameraPoints[0].transform.position;
                        archerSelectedUi.SetActive(false);
                        break;
                    case "Magacian" :
                        isCharacterTouched = false;
                        mainCamera.transform.position = cameraPoints[0].transform.position;
                        break;
                    default :
                        return;    
                }

                characterSelectedUi.SetActive(false);
            }   
        }   
    }

    private void SetActiveFalseNicknamePanel()
    {
        changeUsernamePanel.SetActive(false);
    }

    private void SetActiveFalsePurchaseSuccessPanel()
    {
        purchaseSuccessMessage.SetActive(false);
    }

    private void SetActiveFalsePurchaseFailurePanel()
    {
        purchaseFailureMessage.SetActive(false);
    }

    #endregion
}