using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика
{
    public partial class Form2 : Form
    {
        private bool isDragging;
        private Point lastCursor;
        private Point lastForm;
        public Form2()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            label1.Text = "";
            timer2.Enabled = true;
            timer2.Interval = 1000;
            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {

                SQLiteConnection conn = new SQLiteConnection("Data source=C:/db/raspisanie.db; Version=3");
                conn.Open();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = conn,
                    CommandText = "SELECT * FROM doctors"
                };
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SQLiteDataAdapter adap = new SQLiteDataAdapter(command);
                adap.Fill(dt);
                dataGridView2.DataSource = dt;
                this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                timer2.Start();
            }
            catch (Exception ex)
            {
                label1.ForeColor = Color.Red;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ошибка вывода изображения: " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Form2_Load(null, null);


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int dx = Cursor.Position.X - lastCursor.X;
                int dy = Cursor.Position.Y - lastCursor.Y;
                Location = new Point(lastForm.X + dx, lastForm.Y + dy);
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = Location;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView2.ClearSelection();
        }
    }
}
