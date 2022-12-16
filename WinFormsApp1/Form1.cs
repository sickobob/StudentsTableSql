using System.Data.SqlClient;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
          string connectionString = @"Server=emiit.ru;Database=tereshkin_epb211;User Id=user-96;Password=Baf62932;";
        private void button1_Click(object sender, EventArgs e)
        {
           

            string Login = textBox1.Text;

            string Password = textBox2.Text;

            string ID = "0";
            string role= " ";
            SqlDataReader DBPassword;

            string pass = "";

            string sqlExpression = "SELECT Password, ID, Role FROM Users WHERE Login = @Login";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();


            SqlCommand command = new SqlCommand(sqlExpression, connection);

            SqlParameter LoginParam = new SqlParameter("@Login", Login);

            // добавляем параметр к команде​

            command.Parameters.Add(LoginParam);

            DBPassword = command.ExecuteReader();
            if (DBPassword.HasRows)
            {
                while (DBPassword.Read())// построчно считываем данные​
                {
                    pass = Convert.ToString(DBPassword.GetValue(0));

                    ID = Convert.ToString(DBPassword.GetValue(1));
                    role = DBPassword.GetString(2).Trim();                    
                }
                if (pass != Password) MessageBox.Show("Пароль неверный");
                else
                {
                    Form2 form2 = new Form2(role);
                    form2.ShowDialog();
                }
               
            }
            else MessageBox.Show("Пользователь не найден");
            connection.Close();


        

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Вход и регистрация";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
    }