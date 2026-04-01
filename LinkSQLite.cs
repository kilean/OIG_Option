using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;


public class LinkSQLite
{
    public string FileName = "Database.sqlite";
    private bool bIsConnect = false;

    public bool IsConnect
    {
        get { return bIsConnect; }
    }

    private SQLiteConnection conn = null;
    //連線
    public void Connect()
    {
        bIsConnect = false;
        //string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
        string connStr = "Data Source=" + FileName + ";";
        conn = new SQLiteConnection(connStr);

        // 連線到資料庫
        try
        {
            conn.Open();
            bIsConnect = true;
        }
        catch (SqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Console.WriteLine("無法連線到資料庫.");
                    break;
                case 1045:
                    Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
                    break;
            }
        }
    }

    public void Disconnect()
    {
        bIsConnect = false;
        conn.Close();
        conn.Dispose();
        conn = null;
    }

    public DataTable ExecuteReader(String sql)
    {
        DataTable tb = new DataTable();

        if (conn == null) return tb;

        // 進行select            
        try
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader myData = cmd.ExecuteReader();

            tb.Load(myData);
            myData.Close();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " : " + ex.Message);
        }
        return tb;
    }

    public void ExecuteNonQuery(String sql)
    {
        if (conn == null) return;
        try
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            int ret = cmd.ExecuteNonQuery();
            //Console.WriteLine("ExecuteNonQuery:" + ret.ToString());
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " : " + ex.Message);
        }
        return;
    }
}

