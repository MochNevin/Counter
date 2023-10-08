using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

namespace Counter
{
    public readonly struct CounterData
    {
        public static HashSet<string> Signals = new()
        {
            "+", "-", "*", "/"
        };
    }

    public class CounterBase : MonoBehaviour
    {
        public TMP_Text resultText;
        public TMP_Text displayText;
        private StringBuilder _currentInput;
        private DataTable _dataTable;
        private bool _hasEnter;

        private void Awake()
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            _currentInput = new StringBuilder();
            _dataTable = new DataTable();
        }

        public void OnButtonClick(string value)
        {
            if (_hasEnter)
                OnClear();
            _currentInput.Append(value);
            displayText.text = _currentInput.ToString();
        }

        public void OnEscape()
        {
            if (_hasEnter)
                OnClear();
            if (_currentInput.Length == 0)
                return;
            _currentInput.Remove(_currentInput.Length - 1, 1);
            displayText.text = _currentInput.ToString();
        }

        public void OnCalculate()
        {
            try
            {
                var result = Convert.ToDouble(_dataTable.Compute(_currentInput.ToString(), null));
                resultText.text = result.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                resultText.text = "错误";
            }
            finally
            {
                _hasEnter = true;
            }
        }

        public void OnClear()
        {
            _currentInput.Clear();
            resultText.text = "";
            displayText.text = "";
            _hasEnter = false;
        }
    }
}