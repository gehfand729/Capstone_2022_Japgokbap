using UnityEngine;
using UnityEngine.UI;
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
        LoadingSceneManager.LoadScene("SpawnTestScene");
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

    #endregion

    #region Private Methods

    //성공적으로 로그인됨
    //로비로 이동
    private void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.LogFormat("{0} 으로 로그인하였습니다", result.PlayFabId);
        statusText.text = "";
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        userName.text = result.InfoResultPayload.AccountInfo.Username ?? result.PlayFabId;
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
        statusText.text = "";

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

    #endregion
}