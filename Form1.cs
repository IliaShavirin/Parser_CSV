using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Parser_CSV
{
    public abstract partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                return;

            var path = textBox1.Text;
            Parse.chapter = Convert.ToChar(textBox2.Text);
            try
            {
                var list = Transform.TransformToList(path);

                for (var j = 0; j < list.Count; j++)
                {
                    dataGridView1.RowCount = list.Count;
                    dataGridView1.ColumnCount = list[j].Count;

                    for (var i = 0; i < list[j].Count; i++)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = list[j][i];
                        var column = dataGridView1.Columns[i];
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                }

                foreach (var oleg in list.SelectMany(item => item))
                    File.AppendAllText("note1.txt", oleg.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // показать диалог выбора папки
            var result = openFileDialog1.ShowDialog();

            // если папка выбрана и нажата клавиша `OK` - значит можно получить путь к папке
            if (result == DialogResult.OK)
                // запишем в нашу переменную путь к папке
                textBox1.Text = openFileDialog1.FileName;
        }

        public abstract class Parse
        {
            public static char chapter;

            public static List<object> ParseString(string line)
            {
                line = line.Replace("\"", string.Empty);
                var list = new List<object>(line.Split(chapter));
                return list;
            }
        }

        public abstract class Transform
        {
            public static List<List<object>> TransformToList(string path)
            {
                var list = File.ReadAllLines(path).Select(Parse.ParseString).ToList();
                return list;
            }
        }
    }
}