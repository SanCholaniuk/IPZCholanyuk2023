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
    public partial class Teacher : Form
    {
        public Teacher()
        {
            InitializeComponent();
            this.label2.Text = Var.User;
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getCourses",
                    login = Var.Login
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str != "")
                {
                    this.listBox1.Items.AddRange(str.Split('\n'));
                }
            }
            else
            {
                MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NewCourse form = new NewCourse(this.listBox1);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("Поле для пошуку не може бути пустим!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "searchUserCourses",
                        login = Var.Login,
                        search = this.textBox1.Text
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str == "")
                    {
                        MessageBox.Show("Пошук не дав результатів.");
                    }
                    else
                    {
                        this.listBox1.Items.Clear();
                        this.listBox1.Items.AddRange(str.Split('\n'));
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "getCourses",
                        login = Var.Login
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox1.Items.Clear();
                        this.listBox1.Items.AddRange(str.Split('\n'));
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text.Length == 0)
            {
                MessageBox.Show("Оберіть курс для видалення!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "removeCourse",
                        login = Var.Login,
                        id = this.listBox1.Text.Split(':')[0]
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox1.Items.Remove(this.listBox1.Text);
                        MessageBox.Show(str);
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            CourseTeacher form = new CourseTeacher(this.listBox1.Text.Split(':')[0]);
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult logout = MessageBox.Show("Ви впевнені, що хочете вийти?", "Підтвердження", MessageBoxButtons.YesNo);
            if (logout == DialogResult.Yes)
            {
                if (Socket.ws.ReadyState.ToString() == "Open") Socket.ws.Close();
                Auth form = new Auth();
                form.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                form.Show();
                this.Hide();
            }
        }
    }
}
