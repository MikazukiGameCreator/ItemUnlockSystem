using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TwoBitMachines.FlareEngine;

/// <summary>
/// アンロックデータの設定
/// </summary>
public class UnlockSet : MonoBehaviour
{
    [SerializeField] WorldFloat _mainUiCoin; // コイン(メインUI)
    [SerializeField] WorldFloat _mainUiGem; // ジェム(メインUI)
    [SerializeField] WorldFloat _powerupUiCoin; // コイン(パワーアップUI)
    [SerializeField] WorldFloat _powerupUiGem; // ジェム(パワーアップUI)
    [SerializeField] WorldFloat _clearLevelWF; // クリア済ステージ
    [SerializeField] GameObject _unlockedUI;
    [SerializeField] TMP_Text _unlockNameText;
    [SerializeField] TMP_Text _costCoinText;
    [SerializeField] TMP_Text _costGemText;
    [SerializeField] TMP_Text _needClearStageText;
    [SerializeField] Button _unlockButton;
    [SerializeField] Image _inputBlockPanel;
    [SerializeField] Image _iconImage;
    [SerializeField] float _unlockedStopTime;
    WorldBool _unlockworldBool; // アンロックBool値
    Unlock _unlock;
    string _unlockName;
    int _clearLevel; // クリア済ステージレベル 
    int _costCoin;
    int _costGem;
    int _hasCoin;
    int _hasGem;


    /// <summary>
    /// アンロックデータを設定
    /// </summary>
    /// <param name="worldBool">アンロック有無のBool</param>
    /// <param name="unlockName">アンロック対象の名前</param>
    /// <param name="costCoin">消費コイン</param>
    /// <param name="costGem">消費ジェム</param>
    /// <param name="unlock">アンロック関連クラス</param>
    /// <param name="iconSprite">アンロック対象の画像</param>
    /// <param name="unlockLevel">アンロックに必要なステージレベル</param>
    public void SetUnlockData(
        WorldBool worldBool, string unlockName, string costCoin,
        string costGem, Unlock unlock, Sprite iconSprite, int unlockLevel)
    {
        _unlockworldBool = worldBool;
        _unlockName = unlockName;
        _unlock = unlock;
        _clearLevel = (int)_clearLevelWF.GetValue();

        // ホップアップ時にUI情報を更新
        SetStartUi();

        // テキストを変更
        SetText(costCoin, costGem, unlockLevel);

        // アイコンを変更
        _iconImage.sprite = iconSprite;

        // 取得データをログ表示
        ShowUnlockDataLog();

        // アンロックボタン有効化
        EnableUnlockButton(costCoin, costGem, unlockLevel);
    }


    /// <summary>
    /// アンロック実行(アンロックボタンクリック時)
    /// </summary>
    public void SetTrueUnlock()
    {
        // 指定秒数入力をブロックするコルーチン
        StartCoroutine(WaitInputBlock());

        // コストを消費
        CostPay();

        // 解放フラグをTrue
        _unlockworldBool.SetTrue();
        Debug.Log($"{_unlockName} をアンロックしました");

        // UI表示をアンロック後に更新
        _unlock.ActivateUnlockedUI();
    }


    /// <summary>
    /// ホップアップ時にUI情報を更新
    /// </summary>
    void SetStartUi()
    {
        // アンロックボタン無効化
        _unlockButton.interactable = false;

        // 解放条件の文字表示
        _needClearStageText.enabled = true;
    }


    /// <summary>
    /// アンロックデータのログ表示
    /// </summary>
    void ShowUnlockDataLog()
    {
        string varName = _unlockworldBool.variableName;
        bool isUnlockWB = _unlockworldBool.GetValue();
        Debug.Log($"{_unlockName}を取得");
        Debug.Log($"{varName}のBool値は{isUnlockWB}");
        Debug.Log($"クリア済のステージレベルは{_clearLevel}");
    }


    /// <summary>
    /// アンロックボタン有効化
    /// </summary>
    void EnableUnlockButton(string costCoin, string costGem, int unlockLevel)
    {
        // 所持コイン＆ジェム
        _hasCoin = (int)_mainUiCoin.GetValue();
        _hasGem = (int)_mainUiGem.GetValue();

        // 消費コイン＆ジェム
        _costCoin = int.Parse(costCoin);
        _costGem = int.Parse(costGem);

        // 消費コイン＆ジェム所持と対象ステージクリアでアンロック可
        if (_hasCoin > _costCoin && _hasGem > _costGem && _clearLevel >= unlockLevel)
        {
            // アンロックボタン有効化
            _unlockButton.interactable = true;
            Debug.Log("解放ボタンを有効化");

            // 解放条件の文字非表示
            _needClearStageText.enabled = false;
        }
        else
        {
            Debug.Log($"コインかジェム不足か対象ステージ未クリアで、{_unlockName}解放不可");
        }
    }


    /// <summary>
    /// テキストを変更
    /// </summary>
    void SetText(string costCoin, string costGem, int unlockLevel)
    {
        _unlockNameText.text = _unlockName;
        _costCoinText.text = costCoin;
        _costGemText.text = costGem;
        _needClearStageText.text = $"解放条件：ステージ{unlockLevel}クリア";
    }


    /// <summary>
    /// コストを消費
    /// </summary>
    void CostPay()
    {
        // 所持 - 消費
        _mainUiCoin.Increment(-_costCoin);
        _mainUiGem.Increment(-_costGem);

        // 所持WorldFloatを保存
        _mainUiCoin.Save();
        _mainUiGem.Save();

        // 所持WorldFloatを保存
        _mainUiCoin.RestoreValue();
        _mainUiGem.RestoreValue();

        // 所持WorldFloatを保存
        _powerupUiCoin.RestoreValue();
        _powerupUiGem.RestoreValue();

        Debug.Log($"コイン{_costCoin}とジェム{_costGem}を消費");
    }


    /// <summary>
    /// 指定秒数入力をブロック
    /// </summary>
    IEnumerator WaitInputBlock()
    {
        // UIを表示
        _unlockedUI.SetActive(true);

        // 入力をブロック
        _inputBlockPanel.enabled = true;

        // 指定秒数停止
        yield return new WaitForSeconds(_unlockedStopTime);

        // 入力を受付開始
        _inputBlockPanel.enabled = false;
    }
}
