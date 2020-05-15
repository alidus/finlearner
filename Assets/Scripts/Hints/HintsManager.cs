using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HintType { Message, Confirmation}
public enum HintPreset { NotEnoughMoney,
    NoFreeTime
}

public class HintsManager : MonoBehaviour
{
    public static HintsManager instance;

    // Prefabs
    GameObject hoveringPanelPrefab;
    Transform hintWrapper;

    public Dictionary<HintPreset, Hint> HintPresets = new Dictionary<HintPreset, Hint>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
        FillHintPresets();
    }

    void FillHintPresets()
    {
        HintPresets.Add(HintPreset.NotEnoughMoney, new Hint(hintWrapper, "Не удалось купить", "У вас недосточно денег для покупки", new HintParams(false, true), HintType.Message));
        HintPresets.Add(HintPreset.NoFreeTime, new Hint(hintWrapper, "Нет свободного времени", "У вас слишком мало свободного времени, чтобы заняться этим", new HintParams(false, true), HintType.Message));

    }

    void UpdateReferences()
    {
        hintWrapper = GameObject.Find("PersCanvas").transform.Find("HintWrapper");
        hoveringPanelPrefab = Resources.Load<GameObject>("Prefabs/Hints/HoveringPanel");
    }

    public Hint ShowHint(string title, string msg, HintType hintType = HintType.Message)
    {
        HintParams hintParams = new HintParams(false, true);
        Hint hint = new Hint(hintWrapper, title, msg, hintParams, hintType);

        hint.Show();
        return hint;
    }

    public Hint ShowHint(Hint hint)
    {
        hint.Show();
        return hint;
    }
}
