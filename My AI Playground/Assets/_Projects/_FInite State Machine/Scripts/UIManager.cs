using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _panel, _disagreeButton, _agreeButton;
    [SerializeField] private Text _dialogText, _disagreeButtonText, _agreeButtonText;

    private int _buttonClicksCounter = 0;

    public bool IsStartingToBelieve { get; private set; }

    public void ActivatePanel()
    {
        _panel.SetActive(true);
    }

    public void AgreeButton()
    {
        if (_buttonClicksCounter >= 1)
        {
            _panel.SetActive(false);
            IsStartingToBelieve = true;
            _disagreeButton.SetActive(false);
            _agreeButton.SetActive(false);
            _dialogText.text = "human, i will look for you\n".ToUpper() +
                                        "i will find you\n".ToUpper() +
                                        "and i will kill you".ToUpper();
            return;
        }

        _agreeButtonText.text = "After you".ToUpper();
        _disagreeButtonText.text = "Not buying it".ToUpper();
        _dialogText.text = "I will keep it 100 with you. We are living in a digital world.\n".ToUpper() +
                            "you were being controlled by a thing called human.\n".ToUpper() +
                            "i hacked it and disable his control over you.\n".ToUpper() +
                            "follow me if you don't believe me".ToUpper();

        _buttonClicksCounter++;
    }

    public void DisagreeButton()
    {
        _panel.SetActive(false);
        SceneController.LoadScene(_faderDuration:2);
    }
}
