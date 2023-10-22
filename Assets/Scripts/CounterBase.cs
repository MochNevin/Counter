using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;

namespace Counter
{
    public class CounterBase : MonoBehaviour
    {
        private static MySqlConnection _connection;
        public TMP_Text resultText;
        public TMP_Text displayText;
        public TMP_Text[] Histroy;
        private Stack<string> _catchInput;
        private StringBuilder _currentInput;
        private DataTable _dataTable;
        private bool _hasEnter;
        private int _count;
        private List<string> _lastStrings;

        private void Awake()
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            _lastStrings = new List<string>();
            _currentInput = new StringBuilder();
            _catchInput = new Stack<string>();
            _dataTable = new DataTable();
            _connection = GetConnection("localhost", "3306", "root", "abcd", "student");
            _connection.Open();
        }

        private void OnApplicationQuit()
        {
            if (_connection != null)
            {
                ClearString();
                _connection.Close();
            }

            _connection = null;
        }

        public void PushString(string data) => InsertString(data);

        public void ClearString()
        {
            const string query = "TRUNCATE TABLE my_strings";
            using var command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        private void InsertString(string data)
        {
            _count++;
            const string query = "INSERT INTO my_strings (data) VALUES (@data)";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@data", data);
            command.ExecuteNonQuery();
        }

        private string GetLastString()
        {
            var strs = GetLastStrings(_count);
            return strs.Count > 0 ? strs[0] : null;
        }

        public void UpdateStrings()
        {
            var strs = GetLastStrings(_count);
            for (var i = 0; i < 10; ++i)
            {
                var j = i;
                if (strs.Count > i)
                    Histroy[i].text = j + 1 + ": " + strs[i];
                else
                    Histroy[i].text = "";
            }
        }

        public void Open(GameObject obj) => obj.SetActive(true);

        public void Close(GameObject obj) => obj.SetActive(false);

        public List<string> GetLastStrings(int count)
        {
            const string query = "SELECT data FROM my_strings ORDER BY id LIMIT @count";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@count", count);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var data = reader.GetString("data");
                if (_lastStrings.Count >= count)
                    _lastStrings.RemoveAt(_lastStrings.Count - 1);
                _lastStrings.Insert(0, data);
            }

            return _lastStrings;
        }

        public static MySqlConnection GetConnection(string server, string port, string user, string password, string database) => new("Database=" + database + ";Data Source=" + server + ";User Id=" + user + ";Password=" + password + ";port=" + port + ";charset=utf8");

        public void OnButtonClick(string value)
        {
            if (_hasEnter)
                OnClear();
            _currentInput.Append(value);
            _catchInput.Push(value);
            displayText.text = _currentInput.ToString();
        }

        public void OnEscape()
        {
            if (_hasEnter)
                OnClear();
            if (_catchInput.Count == 0)
                return;
            var str = _catchInput.Pop();
            _currentInput.Remove(_currentInput.Length - str.Length, str.Length);
            displayText.text = _currentInput.ToString();
        }

        public void OnAnswer()
        {
            var str = GetLastString();
            if (str != null)
                OnButtonClick(str);
        }

        public void OnCalculate()
        {
            try
            {
                var input = _currentInput.ToString();
                var result = Convert.ToDouble(_dataTable.Compute(input, default));
                var str = result.ToString(CultureInfo.InvariantCulture);
                resultText.text = str;
                PushString(str);
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
            _catchInput.Clear();
            resultText.text = "";
            displayText.text = "";
            _hasEnter = false;
        }
    }
}