using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingSystem
{
    public partial class Reg : Form
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string surname = this.textBox1.Text;
            string name = this.textBox2.Text;
            string patronymic = this.textBox3.Text;
            string login = this.textBox4.Text;
            string password_1 = this.textBox5.Text;
            string password_2 = this.textBox6.Text;
            string role = this.listBox1.Text;

            if (name.Length == 0 || surname.Length == 0 || patronymic.Length == 0 || login.Length == 0 || password_1.Length == 0 || password_2.Length == 0 || role.Length == 0)
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else if (Regex.IsMatch(surname, @"[^\u0400-\u04FF]"))
            {
                MessageBox.Show("Поле «Прізвище» повинно містити тільки кирилицю!");
            }
            else if (Regex.IsMatch(name, @"[^\u0400-\u04FF]"))
            {
                MessageBox.Show("Поле «Ім'я» повинно містити тільки кирилицю!");
            }
            else if (Regex.IsMatch(patronymic, @"[^\u0400-\u04FF]"))
            {
                MessageBox.Show("Поле «По батькові» повинно містити тільки кирилицю!");
            }
            else if (login.Length < 6)
            {
                MessageBox.Show("Логін повинен містити мінімум 6 символів!");
            }
            else if (!Regex.IsMatch(login, @"^[0-9a-zA-Z]+$"))
            {
                MessageBox.Show("Логін повинен містити тільки цифри та латинські букви!");
            }
            else if (password_1.Length < 6)
            {
                MessageBox.Show("Пароль повинен містити мінімум 6 символів!");
            }
            else if (!Regex.IsMatch(password_1, @"^[0-9a-zA-Z]+$"))
            {
                MessageBox.Show("Пароль повинен містити тільки цифри та латинські букви!");
            }
            else if (password_1 != password_2)
            {
                MessageBox.Show("Паролі повинні співпадати!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() != "Open") Socket.ws.Connect();

                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "registration",
                        surname = surname,
                        name = name,
                        patronymic = patronymic,
                        login = login,
                        password = password_1,
                        role = role
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str == "login_exists")
                    {
                        MessageBox.Show("Користувач з даним логіном вже існує!");
                    }
                    else
                    {
                        MessageBox.Show("Реєстрація пройшла успішно!");
                        var tmp = str.Split('\n');
                        Var.Login = login;
                        Var.User = tmp[1];
                        if (tmp[0] == "Студент")
                        {
                            Student from = new Student();
                            from.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                            from.Show();
                            this.Hide();
                        }
                        else
                        {
                            Teacher from = new Teacher();
                            from.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                            from.Show();
                            this.Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Auth form = new Auth();
            form.FormClosed += new FormClosedEventHandler(delegate { Close(); });
            form.Show();
            this.Hide();
        }

        private void Reg_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
