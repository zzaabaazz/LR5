using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        int id = 0;
        int id1 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            // Создание объекта команды на языке SQL.
            string strSQL = "SELECT * FROM cars";
            // Создание объекта NpgsqlDataAdapter
            // параметры: строка запроса и строка подключения
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL, connString);
            // Создание объекта DataSet
            DataSet ds = new DataSet();
            // Задаем соответствие имен столбцов базы данны (таблица cars) и названий столбцов в таблице.
            DataTableMapping custMap = da.TableMappings.Add("cars", "Автомобили");
            custMap.ColumnMappings.Add("car_id", "ID");
            custMap.ColumnMappings.Add("marka", "Марка");
            custMap.ColumnMappings.Add("color", "Цвет");
            custMap.ColumnMappings.Add("reg_nom", "Рег.номер");
            da.Fill(ds, "cars");
            // Отображение таблицы через таблицу DataGridView
            //выводит всю таблицу с указанными заголовками
            dataGridView1.DataSource = ds.Tables["Автомобили"].DefaultView;
            conn.Close(); //закрытие соединения

            //мясо :)

            conn.Open();
            // Создание объекта команды на языке SQL.
            string strSQL1 = "SELECT * FROM owners";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(strSQL1, connString);
            // Создание объекта DataSet
            DataSet ds1 = new DataSet();
            DataTableMapping custMap1 = da1.TableMappings.Add("owners", "Владельцы");
            custMap1.ColumnMappings.Add("owner_id", "ID владельца");
            custMap1.ColumnMappings.Add("car_id", "ID машины");
            custMap1.ColumnMappings.Add("name", "Имя");
            da1.Fill(ds1, "owners");
            // Отображение таблицы через таблицу DataGridView
            //выводит всю таблицу с указанными заголовками
            dataGridView2.DataSource = ds1.Tables["Владельцы"].DefaultView;
            conn.Close(); //закрытие соединения

        }

        void UpdateGrid() //Вывод данных из БД в dataGridView
        {
            string conn1 = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            string strSQL2 = "Select * From cars";
            // Создание объекта NpgsqlDataAdapter
            // задаем строку запроса и строку подключения
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL2, conn1);
            // Создание объекта DataSet 
            DataSet ds = new DataSet();
            da.Fill(ds, "cars"); //заполнение ds данными таблицы cars
                                 // Отображение таблицы через таблицу DataGridView
            dataGridView1.DataSource = ds.Tables["cars"].DefaultView;
        }

        private void button2_Click(object sender, EventArgs e) // Обновление - кнопка
        {
            UpdateGrid();
            textBox1.Text = ""; // очистка текстовых полей
            textBox2.Text = "";
            textBox3.Text = "";
        }

        
        private void dataGridView1_SelectionChahged(object sender, DataGridViewCellEventArgs e)
        //Вывод выделенной записи в поля
        {
            // определение номера выделенной строки
            DataGridViewSelectedRowCollection t = dataGridView1.SelectedRows;
            if (t.Count > 0)
            {
                DataGridViewRow row = t[0];
                id = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = Convert.ToString(row.Cells[1].Value).Trim();
                textBox2.Text = Convert.ToString(row.Cells[2].Value).Trim();
                textBox3.Text = Convert.ToString(row.Cells[3].Value).Trim();
            }
        }
        


        private void button4_Click_1(object sender, EventArgs e) // Сохранение записи
        {
            try
            {
                
                string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
                NpgsqlConnection conn = new NpgsqlConnection(connString);
                conn.Open();
                string strSQL = string.Format("UPDATE cars SET marka='{0}', color='{1}', reg_nom = '{2}' where car_id ={3}", textBox1.Text, textBox2.Text, textBox3.Text, id);
                NpgsqlCommand command = new NpgsqlCommand(strSQL, conn);
                command.ExecuteNonQuery();
                conn.Close(); // закрытие подключения
                UpdateGrid(); // обновление таблицы
                textBox1.Text = ""; // очистка текстовых полей
                textBox2.Text = "";
                textBox3.Text = "";
                id = 0;
            }
            catch (NpgsqlException) // обработка исключения записи в базу данных
            {
                MessageBox.Show("Ошибка записи в БД");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            string sql = "INSERT INTO cars (marka, color, reg_nom) VALUES(@Mark, @Color, @Peg_nom)";
            // У этой команды будут внутренние параметры.
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
            {
                // Заполнение коллекции параметров.
                NpgsqlParameter param = new NpgsqlParameter();
                cmd.Parameters.Add("@Mark", NpgsqlTypes.NpgsqlDbType.Varchar).Value = this.textBox1.Text;
                cmd.Parameters.Add("@Color", NpgsqlTypes.NpgsqlDbType.Varchar).Value = this.textBox2.Text;
                cmd.Parameters.Add("@Peg_nom", NpgsqlTypes.NpgsqlDbType.Varchar).Value = this.textBox3.Text;
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            UpdateGrid();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)//удаление
        {

            int n = 0;
            try
            {
                string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
                NpgsqlConnection conn = new NpgsqlConnection(connString);
                conn.Open();
                string strSQL = string.Format("DELETE FROM cars WHERE car_id={0}", id);
                NpgsqlCommand myCommand1 = new NpgsqlCommand(strSQL, conn); // Получение результата
                n = myCommand1.ExecuteNonQuery();
                conn.Close();
                UpdateGrid(); // обновление таблицы
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = ""; // очистка текстовых полей
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка удаления");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label4.Text = Convert.ToString(id);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            
                string strSQL = "";
                if (textBox1.Text != "") strSQL = string.Format("SELECT * FROM cars WHERE marka = '{0}'",
               textBox1.Text);
                else if (textBox2.Text != "") strSQL = string.Format("SELECT * FROM cars WHERE color = '{0}'",
               textBox2.Text);
                else if (textBox3.Text != "") strSQL = string.Format("SELECT * FROM cars WHERE reg_nom = '{0}'",
               textBox3.Text);
                string conn = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
                // Создание объекта NpgsqlDataAdapter
                // задаем строку запроса и строку подключения
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL, conn);
                // Создание объекта DataSet
                DataSet ds = new DataSet();
                da.Fill(ds, "cars");
                // Отображение таблицы через таблицу DataGridView
                dataGridView1.DataSource = ds.Tables["cars"].DefaultView;
                textBox1.Text = ""; // очистка текстовых полей
                textBox2.Text = "";
                textBox3.Text = "";
            
        }



        //ВЛАДЕЛЬЦЫ



        
        private void dataGridView2_SelectionChahged(object sender, DataGridViewCellEventArgs e)
        //Вывод выделенной записи в поля
        {
                // определение номера выделенной строки
                DataGridViewSelectedRowCollection t1 = dataGridView2.SelectedRows;
                if (t1.Count > 0)
                {
                    DataGridViewRow row1 = t1[0];
                    id1 = Convert.ToInt32(row1.Cells[0].Value);
                    textBox_CARID.Text = Convert.ToString(row1.Cells[1].Value).Trim();
                    textBoxNAME.Text = Convert.ToString(row1.Cells[2].Value).Trim();
             
            }
        }

        void UpdateGrid1() //Вывод данных из БД в dataGridView
        {
            string conn1 = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            string strSQL2 = "Select * From owners";
            // Создание объекта NpgsqlDataAdapter
            // задаем строку запроса и строку подключения
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL2, conn1);
            // Создание объекта DataSet 
            DataSet ds = new DataSet();
            da.Fill(ds, "owners"); //заполнение ds данными таблицы owners
                                 // Отображение таблицы через таблицу DataGridView
            dataGridView2.DataSource = ds.Tables["owners"].DefaultView;
        }
        private void button8_Click(object sender, EventArgs e)//getID
        {
            label5.Text = Convert.ToString(id1);
        }

        private void button9_Click(object sender, EventArgs e)//обновление
        {
            UpdateGrid1(); // очистка текстовых полей
            textBox_CARID.Text = "";
            textBoxNAME.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)//добавление
        {
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            string sql = "INSERT INTO owners (car_id, name) VALUES(@car_id, @name)";
            // У этой команды будут внутренние параметры.
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
            {
                // Заполнение коллекции параметров.
                NpgsqlParameter param = new NpgsqlParameter();
                cmd.Parameters.Add("@car_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(this.textBox_CARID.Text);
                cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = this.textBoxNAME.Text;
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            UpdateGrid1(); // очистка текстовых полей
            textBox_CARID.Text = "";
            textBoxNAME.Text = "";
        }
        /*
         * "owner_id", "ID владельца");
            custMap1.ColumnMappings.Add("car_id", "ID машины");
            custMap1.ColumnMappings.Add("name", "Имя");
        */

        private void button11_Click(object sender, EventArgs e)//сохранение
        {
            {
                try
                {

                    string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
                    NpgsqlConnection conn = new NpgsqlConnection(connString);
                    conn.Open();
                    string strSQL = string.Format("UPDATE owners SET car_id='{0}', name = '{1}' where owner_id ={2}", textBox_CARID.Text, textBoxNAME.Text, id1);
                    NpgsqlCommand command = new NpgsqlCommand(strSQL, conn);
                    command.ExecuteNonQuery();
                    conn.Close(); // закрытие подключения
                    UpdateGrid(); // обновление таблицы
                    textBox_CARID.Text = "";
                    textBoxNAME.Text = "";
                    id1 = 0;
                }
                catch (NpgsqlException) // обработка исключения записи в базу данных
                {
                    MessageBox.Show("Ошибка записи в БД");
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)//удаление
        {

            int n = 0;
            try
            {
                string connString = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
                NpgsqlConnection conn = new NpgsqlConnection(connString);
                conn.Open();
                string strSQL = string.Format("DELETE FROM owners WHERE owner_id={0}", id1);
                NpgsqlCommand myCommand1 = new NpgsqlCommand(strSQL, conn); // Получение результата
                n = myCommand1.ExecuteNonQuery();
                conn.Close();
                UpdateGrid(); // обновление таблицы
                textBox_CARID.Text = "";
                textBoxNAME.Text = "";
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка удаления");
            }
        }

        private void button13_Click(object sender, EventArgs e)//поиск
        {

            string strSQL = "";
            if (textBox_CARID.Text != "") strSQL = string.Format("SELECT * FROM owners WHERE car_id = '{0}'",
           textBox_CARID.Text);
            else if (textBoxNAME.Text != "") strSQL = string.Format("SELECT * FROM owners WHERE name = '{0}'",
           textBoxNAME.Text);
            string conn1 = "Host=localhost;Port=5432;Username=postgres;Password=0104;Database=inventory";
            // Создание объекта NpgsqlDataAdapter
            // задаем строку запроса и строку подключения
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(strSQL, conn1);
            // Создание объекта DataSet
            DataSet ds = new DataSet();
            da.Fill(ds, "owners");
            // Отображение таблицы через таблицу DataGridView
            dataGridView2.DataSource = ds.Tables["owners"].DefaultView;
            textBox_CARID.Text = "";
            textBoxNAME.Text = "";
        }
    }
}
