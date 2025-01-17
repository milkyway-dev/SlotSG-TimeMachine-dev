using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{

    [Header("AutoSpin Popup")]
    [SerializeField] private Button AutoSpinButton;
    [SerializeField] private Button AutoSpinPopUpClose;
    [SerializeField] private TMP_Text autoSpinCost;
    [SerializeField] private GameObject autoSpinPopupObject;
    [Header("Free Spin Popup")]
    [SerializeField] private GameObject FreeSPinPopUpObject;
    [SerializeField] private TMP_Text FreeSpinCount;
    [SerializeField] private GameObject freeSpinBg;
    [SerializeField] private Image[] purpleBar;

    [Header("Popus UI")]
    [SerializeField] private GameObject MainPopup_Object;

    [Header("Paytable Popup")]
    [SerializeField] private Button paytable_Button;
    [SerializeField] private GameObject payTablePopup_Object;
    [SerializeField] private Button paytableExit_Button;


    [Header("Paytable Texts")]
    [SerializeField] private TMP_Text[] SymbolsText;
    [SerializeField] private TMP_Text BonusFreeSpins_Text;
    [SerializeField] private TMP_Text Wild_Text;

    [Header("Pagination")]
    int CurrentIndex = 0;
    [SerializeField] private GameObject[] paytableList;
    [SerializeField] private Button RightBtn;
    [SerializeField] private Button LeftBtn;




    [Header("Settings Popup")]
    [SerializeField] private GameObject SettingsPopup_Object;
    [SerializeField] internal Button Settings_Button;
    [SerializeField] private Button SettingsExit_Button;
    [SerializeField] private Button SoundToggle_button;
    [SerializeField] private Button MusicToggle_button;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite button;
    private bool isMusic = true;
    private bool isSound = true;

    [Header("all Win Popup")]
    [SerializeField] private GameObject specialWinObject;
    [SerializeField] private Image specialWinTitle;
    [SerializeField] private Sprite[] winTitleSprites;
    [SerializeField] private GameObject WinPopup_Object;
    [SerializeField] private TMP_Text Win_Text;
    [SerializeField] private TMP_Text SpecialWin_Text;
    [SerializeField] private Button winPopUpExit_Button;
    Tween WintextTween;


    [Header("low balance popup")]
    [SerializeField] private GameObject LowBalancePopup_Object;
    [SerializeField] private Button Close_Button;


    [Header("disconnection popup")]
    [SerializeField] private GameObject DisconnectPopup_Object;
    [SerializeField] private Button CloseDisconnect_Button;

    [Header("Quit Popup")]
    [SerializeField] private GameObject QuitPopupObject;
    [SerializeField] private Button GameExit_Button;
    [SerializeField] private Button no_Button;
    [SerializeField] private Button yes_Button;

    [Header("Splash Screen")]
    [SerializeField] private GameObject spalsh_screen;
    [SerializeField] private Image progressbar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField]
    private Button QuitSplash_button;

    [Header("AnotherDevice Popup")]
    [SerializeField] private Button CloseAD_Button;
    [SerializeField] private GameObject ADPopup_Object;

    [Header("free spin popup")]

    [SerializeField] private TMP_Text freeSpinInfo;
    [SerializeField] private TMP_Text freeSpinWinnings;

    [SerializeField] private GameObject freeSpinPanel;
    [SerializeField] private GameObject gameButtonPanel;
    [SerializeField] private Transform freeSpinText;

    [SerializeField]
    private Button m_AwakeGameButton;

    private bool isExit = false;

    [Header("player texts")]
    [SerializeField] private TMP_Text playerCurrentWinning;
    [SerializeField] private TMP_Text playerBalance;

    private GameObject currentPopup;

    internal Action<bool, string> ToggleAudio;
    internal Action<string> playButtonAudio;

    internal Action OnExit;

    Tween balanceTween;
    private void Awake()
    {
        //if (spalsh_screen) spalsh_screen.SetActive(true);
        //StartCoroutine(LoadingRoutine());
        SimulateClickByDefault();
    }

    private void SimulateClickByDefault()
    {
        Debug.Log("Awaken The Game...");
        m_AwakeGameButton.onClick.AddListener(() => { Debug.Log("Called The Game..."); });
        m_AwakeGameButton.onClick.Invoke();
    }

    private void Start()
    {
        // Set up each button with the appropriate action
        SetButton(yes_Button, CallOnExitFunction);
        SetButton(no_Button, () => { if (!isExit) { ClosePopup(); } });
        SetButton(GameExit_Button, () => OpenPopup(QuitPopupObject));
        SetButton(paytable_Button, () => OpenPopup(payTablePopup_Object));
        SetButton(paytableExit_Button, () => ClosePopup());
        SetButton(Settings_Button, () => OpenPopup(SettingsPopup_Object));
        SetButton(SettingsExit_Button, () => ClosePopup());

        SetButton(MusicToggle_button, ToggleMusic);
        SetButton(SoundToggle_button, ToggleSound);

        SetButton(LeftBtn, () => Slide(false));
        SetButton(RightBtn, () => Slide(true));
        SetButton(CloseDisconnect_Button, CallOnExitFunction);
        SetButton(Close_Button, ClosePopup);
        SetButton(QuitSplash_button, () => OpenPopup(QuitPopupObject));
        SetButton(AutoSpinButton, () => OpenPopup(autoSpinPopupObject));
        SetButton(AutoSpinPopUpClose, () => ClosePopup());
        // Initialize other settings
        paytableList[CurrentIndex = 0].SetActive(true);
        isMusic = false;
        isSound = false;
        ToggleMusic();
        ToggleSound();

        winPopUpExit_Button.onClick.AddListener(()=>{
            CloseWinPopup();
        });
    }

    private void SetButton(Button button, Action action)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            playButtonAudio?.Invoke("default");
            action?.Invoke();

        });
    }



    // private IEnumerator LoadingRoutine()
    // {
    //     StartCoroutine(LoadingTextAnimate());
    //     float fillAmount = 0.7f;
    //     progressbar.DOFillAmount(fillAmount, 2f).SetEase(Ease.Linear);
    //     yield return new WaitForSecondsRealtime(2f);
    //     yield return new WaitUntil(() => !socketManager.isLoading);
    //     progressbar.DOFillAmount(1, 1f).SetEase(Ease.Linear);
    //     yield return new WaitForSecondsRealtime(1f);
    //     if (spalsh_screen) spalsh_screen.SetActive(false);
    //     StopCoroutine(LoadingTextAnimate());
    // }



    internal void UpdatePlayerInfo(PlayerData playerData)
    {
        balanceTween?.Kill();
        playerCurrentWinning.text = playerData.currentWining.ToString("f3");
        playerBalance.text = playerData.Balance.ToString("f3");

    }

    private IEnumerator LoadingTextAnimate()
    {
        while (true)
        {
            if (loadingText) loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            if (loadingText) loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            if (loadingText) loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }
    }


    internal void UpdateAutoSpinCost(double cost)
    {
        autoSpinCost.text = cost.ToString();
    }
    internal void LowBalPopup()
    {

        OpenPopup(LowBalancePopup_Object);
    }

    internal void ToggleFreeSpinPanel(bool toggle)
    {

        freeSpinPanel.SetActive(toggle);
        gameButtonPanel.SetActive(!toggle);


    }

    internal void EnablePurplebar(bool enable)
    {

        if (enable)
        {
            purpleBar[0].color = Color.white;
            purpleBar[1].color = Color.white;
        }
        else
        {
            purpleBar[0].DOFade(0, 1f);
            purpleBar[1].DOFade(0, 1f);
        }

    }

    internal void FreeSpinTextAnim()
    {

        freeSpinText.localScale *= 0; ;
        freeSpinText.gameObject.SetActive(true);
        freeSpinText.DOScale(2, 0.5f).OnComplete(() =>
        {

            freeSpinText.DOScale(0, 0.5f).OnComplete(() => freeSpinText.gameObject.SetActive(false));
        });

    }
    internal void ADfunction()
    {
        OpenPopup(ADPopup_Object);
    }

    internal void PopulateSymbolsPayout(UIData uIData)
    {
        string text = "";
        for (int i = 0; i < SymbolsText.Length; i++)
        {
            text = "";
            for (int j = 0; j < uIData.symbols[i].Multiplier.Count; j++)
            {
                text += $"{5 - j}x - {uIData.symbols[i].Multiplier[j][0]+"X"} \n";
            }
            SymbolsText[i].text = text;
        }

        Wild_Text.text = uIData.symbols[10].description.ToString();
        BonusFreeSpins_Text.text=uIData.symbols[11].description.ToString();
    }

    private void CallOnExitFunction()
    {
        isExit = true;
        OnExit?.Invoke();
        // audioController.PlayButtonAudio();
        // socketManager.CloseSocket();
    }

    private void OpenPopup(GameObject Popup)
    {
        if (currentPopup != null && !DisconnectPopup_Object.activeSelf)
        {
            ClosePopup();
        }
        if (Popup) Popup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        currentPopup = Popup;
        // paytableList[CurrentIndex].SetActive(true);
    }

    internal void ClosePopup()
    {
        if (!DisconnectPopup_Object.activeSelf)
        {
            if (currentPopup != null)
            {
                currentPopup.SetActive(false);
                if (MainPopup_Object) MainPopup_Object.SetActive(false);

                currentPopup = null;
            }

        }

        // CurrentIndex=0;
        // paytableList[CurrentIndex].SetActive(true);
    }



    private void Slide(bool inc)
    {
        if(inc){
            CurrentIndex++;
             if (CurrentIndex > paytableList.Length - 1 )
             CurrentIndex=0;

        }else{
            CurrentIndex--;
            if (CurrentIndex <0)
            CurrentIndex=paytableList.Length - 1;

        }
        foreach (var item in paytableList)
        {
            item.SetActive(false);
            
        }
            paytableList[CurrentIndex].SetActive(true);
 

    }

    internal void FreeSpinPopup(int amount, bool enableBg)
    {
        FreeSpinCount.text = amount.ToString();
        OpenPopup(FreeSPinPopUpObject);
        if (enableBg)
            freeSpinBg.SetActive(true);
    }

    internal void CloseFreeSpinPopup()
    {
        ClosePopup();
        FreeSpinCount.text = "0";
        if (freeSpinBg.activeSelf)
            freeSpinBg.SetActive(false);
    }
    internal void EnableWinPopUp(int type,double value)
    {

        OpenPopup(WinPopup_Object);
        if (type > 0){
            specialWinObject.SetActive(true);
            Win_Text.gameObject.SetActive(false);
        }
        else{
            Win_Text.gameObject.SetActive(true);
        }

        switch (type)
        {
            case 0: StartCoroutine(WinTextAnim(value,false));
                break;
            case 1:
                specialWinTitle.sprite = winTitleSprites[0];
                StartCoroutine(WinTextAnim(value,true));
                break;
            case 2:
                specialWinTitle.sprite = winTitleSprites[1];
                StartCoroutine(WinTextAnim(value,true));
                break;
            case 3:
                specialWinTitle.sprite = winTitleSprites[2];
                StartCoroutine(WinTextAnim(value,true));
                break;
        }
    }

    internal void DeductBalanceAnim(double finalAmount, double initAmount)
    {

        balanceTween=DOTween.To(() => initAmount, (val) => initAmount = val, finalAmount, 0.8f).OnUpdate(() =>
        {
            playerBalance.text = initAmount.ToString("f3");

        }).OnComplete(() =>
        {

            playerBalance.text = finalAmount.ToString("f3");
        });
    }

    internal IEnumerator WinTextAnim(double amount,bool special)
    {
        Sequence sequence = DOTween.Sequence();
        if(!special){
        Win_Text.text = amount.ToString("f3");
        Win_Text.transform.localScale *= 4;
        Color InitCOlor = Win_Text.color;
        Win_Text.color = new Color(0, 0, 0, 0);
        sequence.Append(Win_Text.transform.DOScale(Vector2.one, 1f));
        sequence.Join(Win_Text.DOColor(InitCOlor, 1f));
        }else{
            SpecialWin_Text.text=amount.ToString("f3");
        }
        WintextTween=sequence;
        yield return new WaitForSeconds(3f);
        CloseWinPopup();

    }

    void CloseWinPopup(){
        ClosePopup();
        if (specialWinObject.activeSelf)
            specialWinObject.SetActive(false);
        GameManager.winAnimComplete=true;
        WintextTween?.Kill();
        Win_Text.color=new Color(1,1,1,1);
        Win_Text.transform.localScale=Vector3.one;
    }
    internal void DisconnectionPopup()
    {
        if (!isExit)
        {
            OpenPopup(DisconnectPopup_Object);
        }
    }

    internal void UpdateFreeSpinInfo(int freespinCount = -1, double winnings = -1)
    {
        if (freespinCount >= 0)
            freeSpinInfo.text = freespinCount.ToString();
        if (winnings >= 0)
            freeSpinWinnings.text = winnings.ToString();
    }

    private void ToggleMusic()
    {
        isMusic = !isMusic;
        if (isMusic)
        {
            MusicToggle_button.image.sprite = button;

            ToggleAudio?.Invoke(false, "bg");
        }
        else
        {
            MusicToggle_button.image.sprite = empty;
            ToggleAudio?.Invoke(true, "bg");
        }
    }

    private void ToggleSound()
    {
        isSound = !isSound;
        if (isSound)
        {
            SoundToggle_button.image.sprite = button;
            ToggleAudio?.Invoke(false, "button");
            ToggleAudio?.Invoke(false, "wl");
        }
        else
        {
            SoundToggle_button.image.sprite = empty;
            ToggleAudio?.Invoke(true, "button");
            ToggleAudio?.Invoke(true, "wl");
        }
    }

}
