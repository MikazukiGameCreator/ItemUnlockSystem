using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwoBitMachines.FlareEngine;

/// <summary>
/// アンロックの処理
/// </summary>
public class Unlock : MonoBehaviour
{
    [SerializeField] string _unlockName;
    [SerializeField] string _costCoin;
    [SerializeField] string _costGem;
    [SerializeField] int _unlockLevel; // アンロック条件ステージレベル
    [SerializeField] UnlockSet _unlockSet;
    [SerializeField] WorldBool _unlockBool;
    [SerializeField] Button _EquipButton; // 装備ボタン
    [SerializeField] Button _levelUpButton; // レベルアップボタン
    [SerializeField] GameObject _unlockButtonObj; // アンロックボタン
    [SerializeField] Image _selectIcon; // 選択用のアイコン
    [SerializeField] Image _mainIcon; // メインのアイコン
    [SerializeField] Sprite _spriteIcon; // アンロック対象の画像
    [SerializeField] Unlock _thisUnlock;
    [SerializeField] TMP_Text _needClearStageText;


    void Start()
    {
        // アンロックボタンからボタンコンポーネントを取得
        Button unlockButton = _unlockButtonObj.GetComponent<Button>();

        // ボタンクリック時のリスナー追加
        unlockButton.onClick.AddListener(SetUnlockData);
    }


    /// <summary>
    /// アンロック時のUI表示を変更
    /// </summary>
    public void ActivateUnlockedUI()
    {
        // アンロック状態を取得
        bool isUnlocked = _unlockBool.GetValue();

        if (isUnlocked)
        {
            // 解放条件ステージのテキストを非表示
            _needClearStageText.text = "";

            // Imageを初期色に変更
            ResetIconColor();

            // ボタンを有効化
            EnableButton();

            // アンロックボタンを無効化
            DisableUnlockButton();
        }
        else
        {
            // 解放条件ステージのテキスト設定
            _needClearStageText.text = $"解放条件：ステージ{_unlockLevel}クリア";
        }
    }


    /// <summary>
    /// ポップアップ解放UIのデータ更新（ボタンクリック時に呼出すメソッド）
    /// </summary>
    public void SetUnlockData()
    {
        // アンロックデータを更新
        _unlockSet.SetUnlockData(_unlockBool, _unlockName, _costCoin,
            _costGem, _thisUnlock, _spriteIcon, _unlockLevel);
    }


    /// <summary>
    /// Imageを初期色[RGB=255]に変更
    /// </summary>
    void ResetIconColor()
    {
        _selectIcon.color = new Color32(255, 255, 255, 255);
        _mainIcon.color = new Color32(255, 255, 255, 255);
        Debug.Log("Image色を初期色に変更");
    }


    /// <summary>
    /// ボタンを有効化
    /// </summary>
    void EnableButton()
    {
        _levelUpButton.interactable = true;
        _EquipButton.interactable = true;
    }

   
    /// <summary>
    /// アンロックボタンを無効化
    /// </summary>
    void DisableUnlockButton()
    {
        _unlockButtonObj.SetActive(false);
    }
}
