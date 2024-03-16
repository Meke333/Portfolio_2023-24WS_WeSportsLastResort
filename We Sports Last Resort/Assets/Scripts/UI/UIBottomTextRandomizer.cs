using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIBottomTextRandomizer : MonoBehaviour
    {
        [SerializeField] private string[] bottomTexts;

        [SerializeField] private TextMeshProUGUI _textField;

        private void Awake()
        {
            int index = Random.Range((int)0, (int)bottomTexts.Length);

            _textField.text = bottomTexts[index];
        }
    }
}
