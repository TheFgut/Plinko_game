using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LowMoneyModal : MonoBehaviour
{
    [SerializeField] private Button YesBut;
    [SerializeField] private Button NoBut;
    /// <summary>
    /// 
    /// </summary>
    /// <returns>true - user want to refill money, false - dont</returns>
    public async Task<bool> OpenModalAndAskForMoneyRefill()
    {
        OpenModal();

        bool userAnswer = false;
        bool userGaveAnsver = false;

        UnityAction yesAction = () => { userAnswer = true; userGaveAnsver = true; };
        UnityAction noAction = () => { userAnswer = false; userGaveAnsver = true; };
        YesBut.onClick.AddListener(yesAction);
        NoBut.onClick.AddListener(noAction);
        while (!userGaveAnsver)
        {
            await Task.Yield();
        }
        YesBut.onClick.RemoveListener(yesAction);
        NoBut.onClick.RemoveListener(noAction);
        return userAnswer;
    }

    public void OpenModal()
    {
        gameObject.SetActive(true);
    }

    public void CloseModal()
    {
        gameObject.SetActive(false);
    }
}
