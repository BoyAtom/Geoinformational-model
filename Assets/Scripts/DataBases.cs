using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

public class DataBases
{
    public class DataBase
    {
        private static string fileName = "GeoInfo.db";
        private static string DBPath;
        private static SqliteConnection connection;
        private static SqliteCommand command;

        [System.Obsolete]
        public DataBase()
        {
            DBPath = GetDatabasePath();
        }

        [System.Obsolete]
        public static void InitDatabasePath(string name) {
            fileName = name;
            DBPath = GetDatabasePath();
        }

        private static string path;

        /// <summary> Возвращает путь к БД. Если её нет в нужной папке на Андроиде, то копирует её с исходного apk файла. </summary>
        [System.Obsolete]
        public static string GetDatabasePath()
        {
            if(Application.platform != RuntimePlatform.Android)
            {
                path = Application.dataPath + "/StreamingAssets/DataBases/" + fileName; // Путь для Windows
            }
            else
            {
                path = Application.persistentDataPath + "/DataBases/" + fileName; // Путь для Android
                if(!File.Exists(path))
                {
                     WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileName);
                     while (!load.isDone) { }
                     File.WriteAllBytes(path, load.bytes);
                }
            }
            return path;
        }

        /// <summary> Распаковывает базу данных в указанный путь. </summary>
        /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
        [System.Obsolete]
        public static void UnpackDatabase(string toPath)
        {
            string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

            WWW reader = new WWW(fromPath);
            while (!reader.isDone) { }

            File.WriteAllBytes(toPath, reader.bytes);
        }

        /// <summary> Этот метод открывает подключение к БД. </summary>
        public static void OpenConnection()
        {
            connection = new SqliteConnection("Data Source=" + DBPath);
            command = new SqliteCommand(connection);
            connection.Open();
        }

        /// <summary> Этот метод закрывает подключение к БД. </summary>
        public static void CloseConnection()
        {
            connection.Close();
            command.Dispose();
        }

        /// <summary> Этот метод выполняет запрос query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public static void ExecuteQueryWithoutAnswer(string query)
        {
            OpenConnection();
            command.CommandText = query;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
        /// <param name="query"> Собственно запрос. </param>
        /// <returns> Возвращает значение 1 строки 1 столбца, если оно имеется. </returns>
        public static string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();
            command.CommandText = query;
            var answer = command.ExecuteScalar();
            CloseConnection();

            if (answer != null) return answer.ToString();
            else return null;
        }

        /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public static DataTable GetTable(string query)
        {
            OpenConnection();

            SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

            DataSet DS = new DataSet();
            adapter.Fill(DS);
            adapter.Dispose();

            CloseConnection();

            return DS.Tables[0];
        }
    }
}