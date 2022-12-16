using Microsoft.VisualBasic.Logging;
using StudentTable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(string role)
        {
            InitializeComponent();
            Role = role;
        }

        string connectionString = @"Server=emiit.ru;Database=tereshkin_epb211;User Id=user-96;Password=Baf62932;";
         static string Role;
        Form3 form3 = new Form3(Role);
        Form4 form4 = new Form4();
        private async void button2_Click(object sender, EventArgs e)
        {

           
            form3.dataGridView1.AllowUserToAddRows = false;

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("№ Студенческого ", typeof(string));
            dataTable.Columns.Add("Пол", typeof(string));
            dataTable.Columns.Add("Дата рождения", typeof(DateTime));
            dataTable.Columns.Add("Задолжности", typeof(int));
            dataTable.Columns.Add("Основа Обучения", typeof(string));
            dataTable.Columns.Add("Примечание", typeof(string));
            List<Student> students = new List<Student>();
            students = await GetStudentsByOrder();
            for (int i = 0; i < students.Count; i++)
            {
                string fio = students[i].Surname + " " + students[i].Name + " " + students[i].SName;
                dataTable.Rows.Add(fio, students[i].S_number, students[i].Gender, students[i].BDate.Date.ToShortDateString(), students[i].Debts, students[i].L_base, students[i].Note);
            }
            form3.dataGridView1.DataSource = dataTable;
            form3.ShowDialog();
            
        }
        int GetStudentCount()
        {
            int count = 0;
            string sqlExpr = "SELECT COUNT(1) FROM Studentsv1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpr, connection);
                SqlDataReader reader = command.ExecuteReader();
                ()

                return reader.GetInt32(0);
            }
        }
       async Task<List<Student>> GetStudentsByOrder()
        {
            string sqlExpression = "SELECT * FROM Studentsv1 Order By surname";
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {

                    while (await reader.ReadAsync()) // построчно считываем данные
                    {
                        Student student = new Student();

                        student.Name = reader.GetString(0);
                        student.Surname = reader.GetString(1);
                        student.SName = reader.GetString(2);
                        student.Gender = reader.GetString(3);
                        student.L_base = reader.GetString(4);
                        student.S_number = reader.GetInt32(5);
                        student.BDate = reader.GetDateTime(6);
                        student.Debts = reader.GetInt32(7);
                      
                        try
                        {
                            student.Note = reader.GetString(8);
                        }
                        catch 
                        {

                            student.Note = "";
                        }
                        

                      students.Add(student);
                    }
                }

                await reader.CloseAsync();
            }
            return students;
        }
        async Task<List<Student>> GetStudentsBySnumberDesc()
        {
            string sqlExpression = "SELECT * FROM Studentsv1 where gender = 'мужской' Order By Snumber desc";
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {

                    while (await reader.ReadAsync()) // построчно считываем данные
                    {
                        Student student = new Student();

                        student.Name = reader.GetString(0);
                        student.Surname = reader.GetString(1);
                        student.SName = reader.GetString(2);
                        student.Gender = reader.GetString(3);
                        student.L_base = reader.GetString(4);
                        student.S_number = reader.GetInt32(5);
                        student.BDate = reader.GetDateTime(6);
                        student.Debts = reader.GetInt32(7);

                        try
                        {
                            student.Note = reader.GetString(8);
                        }
                        catch
                        {

                            student.Note = "";
                        }


                        students.Add(student);
                    }
                }

                await reader.CloseAsync();
            }
            return students;
        }
        bool CheckInitials(TextBox textBox1, TextBox textBox2, TextBox textBox3)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || (!Regex.IsMatch(textBox1.Text, @"^[a-zA-Zа-яА-Я]+$")) || String.IsNullOrEmpty(textBox2.Text) || (!Regex.IsMatch(textBox2.Text, @"^[a-zA-Zа-яА-Я]+$")) || String.IsNullOrEmpty(textBox3.Text) || (!Regex.IsMatch(textBox3.Text, @"^[a-zA-Zа-яА-Я]+$")))
            {
                MessageBox.Show("Введите корректные инициалы");
                return false;
            }
            else return true;

        }
        /// <summary>
        /// проверка табельногго номера 
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        bool CheckDebts(TextBox textBox)
        {
            if (String.IsNullOrEmpty(textBox.Text)) return true;
            if (!int.TryParse(textBox.Text, out int b))
           {
                MessageBox.Show("Введите корректное число долгов");
                return false;
            }
           else return true;
        }
        /// <summary>
        /// Проверяет правильность заполнения полей с выбором ответа
        /// </summary>
        /// <param name="comboBox1"></param>
        /// <param name="comboBox2"></param>
        /// <returns></returns>
        bool CheckComboBoxes(ComboBox comboBox1, ComboBox comboBox2)
        {
            if (String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(comboBox2.Text)) return false;
            else return true;
        }
        bool CheckGender(ComboBox comboBox)
        {
            if (comboBox.Text != "мужской" || comboBox.Text != "женский")
                {
                MessageBox.Show("Введите корректный пол");
                return false;
            }
            else return true;
        }
        bool CheckBase(ComboBox comboBox)
        {
            if (comboBox.Text != "бюджетная" || comboBox.Text != "платная")
            {
                MessageBox.Show("Введите корректное значение основы обучения");
                return false;
         
            }

            else return true;
        }
        /// <summary>
        /// проверяет дату и сравнивает с текущей
        /// </summary>
        /// <param name="dateTimePicker"></param>
        /// <returns></returns>
        
        bool CheckDate(DateTimePicker dateTimePicker)
        {
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = dateTimePicker.Value;

            if (dt1.Date.Equals(dt2.Date)||dt1.Date < dt2.Date)
            {
                MessageBox.Show("Введите корректную дату");
                return false;
            }
            
            else return true;
        }
        
        /// <summary>
        /// проверяет табельный номер на корректность и уникальность
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        bool CheckTabNum(TextBox textBox)
        {
            if (String.IsNullOrEmpty(textBox.Text) || !Regex.IsMatch(textBox.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Введите корректный табельный номер");
                return false;
            }
            else
                return true;

        }
        bool isUniqueTab(TextBox textBox)
        {
            SqlDataReader sqlDataReader;
            int snumber;
            string sqlExpression = "SELECT Snumber FROM Studentsv1";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);

            sqlDataReader = command.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())// построчно считываем данные​
                {
                    snumber = sqlDataReader.GetInt32(0);

                    if (int.Parse(textBox.Text) == snumber)
                    {
                        MessageBox.Show("табельный номер уже есть в базе");
                        connection.Close();
                        return false;

                    }
                }
                }
     
            return true;
        }
        public bool CheckStudent()
        {
            if (CheckInitials(textBox1, textBox2, textBox3)) 
                if(CheckDate(dateTimePicker1))
                    if (CheckComboBoxes(comboBox1, comboBox2))
                       // if (CheckGender(comboBox1))
                            if (CheckTabNum(textBox4))
                                if(isUniqueTab(textBox4))
                                   //  if (CheckBase(comboBox2))
                                           if (CheckDebts(textBox5)) return true;

            return false;
        }
        public int AddStudent()
        {
            SqlDataReader sqlDataReader;
            string sqlExpression = $"INSERT INTO Studentsv1 (Name, Surname,Sname,Gender,Lbase,Snumber,Birthday,Debts,Notes) VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{comboBox1.Text}','{comboBox2.Text}','{textBox4.Text}','{dateTimePicker1.Value.Date.ToShortDateString()}',{textBox5.Text},'{textBox6.Text}')";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            UpdateLabel();
            MessageBox.Show("студен добавлен");
           
            return command.ExecuteNonQuery();
            connection.Close();
        }
        void UpdateLabel()
        {
            label10.Text = $"Количество записей:{GetStudentCount()}";
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            UpdateLabel();
            if (Role == "guest")
            {
                button1.Visible = false;
                button3.Visible = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
                if (CheckStudent())
                {
                  AddStudent();       
                }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
         
            form4.dataGridView1.AllowUserToAddRows = false;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("№ Студенческого ", typeof(string));
            dataTable.Columns.Add("Пол", typeof(string));
            dataTable.Columns.Add("Дата рождения", typeof(DateTime));
            dataTable.Columns.Add("Задолжности", typeof(int));
            dataTable.Columns.Add("Основа Обучения", typeof(string));
            dataTable.Columns.Add("Примечание", typeof(string));
            List<Student> students = new List<Student>();
            students = await GetStudentsBySnumberDesc();
            for (int i = 0; i < students.Count; i++)
            {
                string fio = students[i].Surname + " " + students[i].Name + " " + students[i].SName;
                dataTable.Rows.Add(fio, students[i].S_number, students[i].Gender, students[i].BDate.Date.ToShortDateString(), students[i].Debts, students[i].L_base, students[i].Note);
            }
            form4.dataGridView1.DataSource = dataTable;
            form4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            form3.Close();
            form4.Close();
        }
    }
}
