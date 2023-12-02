using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingSystem
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = this.textBox1.Text;
            string password = this.textBox2.Text;
            if (login.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() != "Open") Socket.ws.Connect();

                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "login",
                        login = login,
                        password = password

                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str == "missing_login")
                    {
                        MessageBox.Show("Користувач з даним логіном відсутній!");
                    }
                    else if (str == "wrong_password")
                    {
                        MessageBox.Show("Пароль неправильний!");
                    }
                    else if (str == "user_logined")
                    {
                        MessageBox.Show("Даний користувач вже ввійшов в систему!");
                    }
                    else
                    {
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
            Reg form = new Reg();
            form.FormClosed += new FormClosedEventHandler(delegate { Close(); });
            form.Show();
            this.Hide();
        }

        private void Auth_Load(object sender, EventArgs e)
        {

        }
    }
}
