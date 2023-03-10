using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string path;

        async private void VisualizingNote(string path_note)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path_note))
                {
                    string text = await reader.ReadToEndAsync();
                    richTextBox1.Text = text;
                }
            }
            catch
            {
                // При попытке редактирования пути к файлу
                toolStripComboBox1.Text = "";
            }
        }

        async private void SavingNote(string pathNote, string text)
        {
            // Сохранение заметки
            using (StreamWriter writer = new StreamWriter(pathNote, false))
            {
                await writer.WriteLineAsync(text);
            }
        }

        private void FirstNote(string pathNote)
        {
            string text = "Привет!\nЭто твоя первая заметка!\nМожешь удалить её и создать свою.\nУдачи!";
            string path_note = path + "1.txt";

            richTextBox1.Text = text;
            
            SavingNote(path_note, text);

            toolStripComboBox1.Items.Add(path_note);
            toolStripComboBox1.Text = path_note;
        }

        public Form1()
        {
            InitializeComponent();
            this.path = AppDomain.CurrentDomain.BaseDirectory.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.AddRange(Directory.GetFiles(this.path, "*.txt"));
            if (toolStripComboBox1.Items.Count == 0)
            {
                FirstNote(this.path);
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Text = "";
            this.richTextBox1.Text = "";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Сохранение файла

            string path_note = toolStripComboBox1.Text;
            string text = richTextBox1.Text;
            
            if (string.IsNullOrEmpty(path_note))
            {
                // Создание нового
                if (toolStripComboBox1.Items.Count == 0)
                {
                    // Если заметок ещё нет
                    path_note = path + "1.txt";
                }
                else
                {
                    // Если заметки уже были
                    int note_name = toolStripComboBox1.Items.Count + 1; 
                    path_note = path + note_name.ToString() + ".txt";                    
                }
                
                SavingNote(path_note, text);

                toolStripComboBox1.Items.Add(path_note);
                toolStripComboBox1.Text = path_note;
            }
            else
            {
                // Редактирование существующего файла
                SavingNote(path_note, text);
            }            
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            // Вывод файла на экран
            string path_note = toolStripComboBox1.Text;
            if (!string.IsNullOrEmpty(path_note))
            {           
                VisualizingNote(path_note);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Удаление заметки
            string path_note = toolStripComboBox1.Text;

            richTextBox1.Text = "";
            toolStripComboBox1.Items.Remove(path_note);
            toolStripComboBox1.Text = "";

            if (!string.IsNullOrEmpty(path_note))
            {
                FileInfo fileInf = new FileInfo(path_note);
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                }
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";
            System.IO.DirectoryInfo di = new DirectoryInfo(this.path);
            foreach (FileInfo note in di.EnumerateFiles("*.txt"))
            {
                note.Delete();
            }
        }
    }
}
