using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Collections.Specialized.BitVector32;


public partial class Fo_MultLangEdit : Form
{
    
    XmlDocument xmlDoc = new XmlDocument(); 

    

    public Fo_MultLangEdit()
    {
        InitializeComponent();       
    }

    public void LoadLanguage()
    {
        dataGridView1.Rows.Clear();
        dataGridView1.Columns.Clear();
        listBox1.Items.Clear();
        

        xmlDoc.RemoveAll();
        XmlElement xmlRoot = xmlDoc.CreateElement("Language");
        xmlDoc.AppendChild(xmlRoot);

        int col = dataGridView1.Columns.Add("Col_Object", "元件名稱");
        dataGridView1.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dataGridView1.Columns[col].ReadOnly = true;
        col = dataGridView1.Columns.Add("Col_Node", "節點(隱藏)");
        dataGridView1.Columns[col].Visible = false;
        dataGridView1.Columns[col].ReadOnly = true;
        // 取得 Language 資料夾下的所有子資料夾
        string[] subDirectories = Directory.GetDirectories(Application.StartupPath + "\\Language\\");
        // 每個子資料夾新增到 DataGridView
        foreach (string dir in subDirectories)
        {
            string folderName = Path.GetFileName(dir); // 取得子資料夾名稱
            col = dataGridView1.Columns.Add("Col_" + folderName, folderName);
            dataGridView1.Columns[col].ReadOnly = true;
            dataGridView1.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TIniFile ini = new TIniFile(dir + "\\" + folderName + ".txt");
            string[] sessions = ini.GetSections();
            foreach (string session in sessions)
            {
                XmlElement xmlSession = xmlRoot.FindFirstByAttribute("Session", "Name", session);                
                if (xmlSession == null) //沒加入過就建立
                {
                    listBox1.Items.Add(session);
                    xmlSession = xmlDoc.CreateElement("Session");
                    xmlSession.SetAttribute("Name", session);
                    xmlRoot.AppendChild(xmlSession);
                }

                string[] keys = ini.GetKeys(session);
                foreach (string obj_name in keys)
                {
                    XmlElement xmlObject = xmlSession.FindFirstByAttribute("Key","Name",obj_name);
                    if (xmlObject == null) //沒加入過就建立
                    {
                        xmlObject = xmlDoc.CreateElement("Key");
                        xmlObject.SetAttribute("Name", obj_name);
                        xmlSession.AppendChild(xmlObject);
                    }
                    string obj_value = ini.ReadString(session, obj_name, "");
                    xmlObject.SetAttribute(folderName, obj_value);
                }
            }
        }

        if (listBox1.Items.Count > 0) 
        {
            listBox1.SelectedIndex = 0;
        }


    }

    private void Fo_MultLangEdit_Load(object sender, EventArgs e)
    {
        LoadLanguage();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (listBox1.SelectedIndex == -1) return;
            string session = listBox1.Items[listBox1.SelectedIndex].ToString();
            dataGridView1.Rows.Clear();

            XmlElement root = xmlDoc.DocumentElement;
            foreach (XmlElement xmlSession in root.ChildNodes)
            {
                if (xmlSession.Name != "Session") continue;
                if (xmlSession.GetAttribute("Name") != session) continue;

                foreach (XmlElement x in xmlSession.ChildNodes)
                {
                    if (x.Name != "Key") continue;
                    string remove = x.GetAttribute("Remove");
                    if (remove != "") continue;

                    string name = x.GetAttribute("Name");
                    int index = dataGridView1.Rows.Add(name);
                    dataGridView1.Rows[index].Cells[1].Value = x;
                    for (int i = 2; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Rows[index].Cells[i].Value = x.GetAttribute(dataGridView1.Columns[i].HeaderText).Replace("\n", "\\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (dataGridView1.CurrentCell == null) return;

            int row = dataGridView1.CurrentCell.RowIndex;
            int col = dataGridView1.CurrentCell.ColumnIndex;

            label1.Text = "(" + col + "," + row + ")";

            dataGridView1.SelectionMode = dataGridView1.CurrentCell.ColumnIndex == 0 ? DataGridViewSelectionMode.FullRowSelect : DataGridViewSelectionMode.RowHeaderSelect;
            textBox1.Text = dataGridView1.CurrentCell.Value.ToString().Replace("\\n", "\r\n");

            //dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[col];
            //dataGridView1.Select();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "Xml File";
            saveFileDialog.Filter = "Xml File|*.xml";
            DialogResult ret = saveFileDialog.ShowDialog();
            if (ret != DialogResult.OK) return;

            xmlDoc.Save(saveFileDialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btn_Apply_Click(object sender, EventArgs e)
    {
        try
        {
            XmlElement root = xmlDoc.DocumentElement;

            for (int i = 2; i < dataGridView1.Columns.Count; i++)
            {
                string lang_name = dataGridView1.Columns[i].HeaderText;
                string filename = Application.StartupPath + "\\Language\\" + lang_name + "\\" + lang_name + ".txt";
                if (File.Exists(filename)) File.Delete(filename);

                TIniFile ini = new TIniFile(filename);

                for (int j = 0; j < listBox1.Items.Count; j++)
                {
                    string session = listBox1.Items[j].ToString();
                    XmlElement xmlSession = root.FindFirstByAttribute("Session", "Name", session);

                    foreach (XmlElement x in xmlSession.ChildNodes)
                    {
                        string name = x.GetAttribute("Name");
                        string value = x.GetAttribute(lang_name);
                        string remove = x.GetAttribute("Remove");
                        if (remove == "") ini.WriteString(session, name, value);
                    }
                }
            }
            MessageBox.Show("套用完成");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            //if (dataGridView1.CurrentRow == null) return;
            List<DataGridViewRow> Rows = new List<DataGridViewRow>();
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    Rows.Add(row);
                }

                foreach (DataGridViewRow row in Rows)
                {
                    XmlElement x = (XmlElement)row.Cells[1].Value;
                    x.SetAttribute("Remove", "True");
                    dataGridView1.Rows.Remove(row);
                }
            }
            else if (e.KeyCode == Keys.V && Control.ModifierKeys == Keys.Control)
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int col = dataGridView1.CurrentCell.ColumnIndex;
                    int row = dataGridView1.CurrentCell.RowIndex;
                    string data = Clipboard.GetText();
                    if (data == null) return;
                    if (data.Last() == '\n') data = data.Substring(0, data.Length - 2);
                    if (data.First() == '"' && data.Last() == '"')
                    {
                        data = data.Substring(1, data.Length - 2).Replace("\r\n", "\\n").Replace("\n", "\\n");
                    }
                    dataGridView1.CurrentCell.Value = data.Replace("\r\n", "\\n");
                    XmlElement x = (XmlElement)dataGridView1.Rows[row].Cells[1].Value;
                    x.SetAttribute(dataGridView1.Columns[col].HeaderText, data);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (bEditMode)
            {
                if (dataGridView1.CurrentCell == null) return;
                int col = dataGridView1.CurrentCell.ColumnIndex;
                int row = dataGridView1.CurrentCell.RowIndex;
                string data = textBox1.Text;
                dataGridView1.CurrentCell.Value = data.Replace("\r\n", "\\n");
                XmlElement x = (XmlElement)dataGridView1.Rows[row].Cells[1].Value;
                x.SetAttribute(dataGridView1.Columns[col].HeaderText, data);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    bool bEditMode = false;
    private void textBox1_Enter(object sender, EventArgs e)
    {
        bEditMode = true;
    }

    private void textBox1_Leave(object sender, EventArgs e)
    {
        bEditMode = false;
    }

    private void btn_Search_Click(object sender, EventArgs e)
    {
        try
        {
            string key = tb_Search.Text.ToLower();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString().ToLower() == key)
                {
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    return;
                }

                for (int j = 2; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString().ToLower() == key)
                    {
                        dataGridView1.Focus();
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[j];
                        return;
                    }
                }
            }
            MessageBox.Show("搜尋完畢, 沒有符合的目標");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btn_Next_Click(object sender, EventArgs e)
    {
        try
        {
            string key = tb_Search.Text.ToLower();

            int index = 0;
            if (dataGridView1.CurrentRow != null) index = dataGridView1.CurrentRow.Index;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int new_index = (index + 1 + i) % dataGridView1.Rows.Count;
                if (dataGridView1.Rows[new_index].Cells[0].Value.ToString().ToLower() == key)
                {
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[new_index].Cells[0];
                    return;
                }

                for (int j = 2; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[new_index].Cells[j].Value.ToString().ToLower() == key)
                    {
                        dataGridView1.Focus();
                        dataGridView1.CurrentCell = dataGridView1.Rows[new_index].Cells[j];
                        return;
                    }
                }
            }
            MessageBox.Show("搜尋完畢, 沒有符合的目標");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void tb_Search_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) btn_Search.PerformClick();
    }

    private void tsmi_Delete_Click(object sender, EventArgs e)
    {
        if (listBox1.SelectedIndex < 0) return;
        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
    }

    private void listBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            if (listBox1.SelectedIndex < 0) return;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }

    private void tb_Search_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tb_Search.Text)) return;
        string key = tb_Search.Text;
        if (key.Last() == '\n') key = key.Substring(0, key.Length - 2);
        if (key.First() == '"' && key.Last() == '"')
        {
            key = key.Substring(1, key.Length - 2).Replace("\r\n", "\\n").Replace("\n", "\\n");
        }
        tb_Search.Text = key;
    }

    private void tsmi_Search_Click(object sender, EventArgs e)
    {
        tb_Search.Text = Clipboard.GetText();
        tb_Search.Focus();

    }

    private void tsmi_Next_Click(object sender, EventArgs e)
    {
        btn_Next.PerformClick();
    }
}

public static class ExClass
{
    public static XmlElement FindFirstByTagName(this XmlElement element, string tagName)
    {
        foreach (XmlElement x in element.ChildNodes)
        {
            if (x.Name == tagName)
            {
                return  x;
            }
        }
        return null;
    }

    public static XmlElement FindFirstByAttribute(this XmlElement element, string tagName, string attName, string attValue)
    {
        foreach (XmlElement x in element.ChildNodes)
        {
            if (x.Name != tagName) continue;
            
            string val = x.GetAttribute(attName);
            if (val == attValue) return x;
        }
        return null;
    }
}