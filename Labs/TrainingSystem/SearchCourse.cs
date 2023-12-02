using Newtonsoft.Json;
using System;
using System.Collections;
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
    public partial class SearchCourse : Form
    {
        ListBox list1;
        public SearchCourse(ListBox list)
        {
            list1 = list;
            InitializeComponent();
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
                        func = "searchCourses",
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

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getCourseDescription",
                    id = this.listBox1.Text.Split(':')[0]
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str != "")
                {
                    MessageBox.Show(str);
                }
            }
            else
            {
                MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text.Length == 0)
            {
                MessageBox.Show("Оберіть курс!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var course = this.listBox1.Text.Split(':');
                    var data = new Json
                    {
                        func = "addCourse",
                        login = Var.Login,
                        id = course[0],
                        user = Var.User,
                        name = course[1].Trim()
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str == "false")
                    {
                        MessageBox.Show("Ви вже долучились до даного курсу!");
                    }
                    else
                    {
                        list1.Items.Add(str);
                        MessageBox.Show("Курс успішно додано!");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
        }
    }
}
