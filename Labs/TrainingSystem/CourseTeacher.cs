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
using WebSocketSharp;

namespace TrainingSystem
{
    public partial class CourseTeacher : Form
    {
        public CourseTeacher(string id)
        {
            InitializeComponent();
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getCourseInfo",
                    id = id,
                    isTeacher = true
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str != "")
                {
                    var course = str.Split('\n');
                    this.label1.Text = course[0];
                    this.textBox3.Text = id;
                    this.textBox2.Text = course[1];
                    if (course.Length >= 3 && course[2] != "")
                    {
                        foreach (string item in course[2].Split('&'))
                        {
                            var tmp = item.Split('=');
                            this.listBox2.Items.Add(new Var.ListBoxItem() { ID = tmp[0], Item = $"{tmp[1]}" });
                        }
                    }
                    if (course.Length == 4)
                    {
                        foreach (string item in course[3].Split('&'))
                        {
                            var tmp = item.Split('=');
                            this.listBox1.Items.Add(new Var.ListBoxItem() { ID = tmp[0], Item = $"{tmp[1]}" });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string description = this.textBox2.Text;
            if (description.Length == 0)
            {
                MessageBox.Show("Заповніть поле «Опис»!");
            }
            else
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "editCourseDescription",
                        id = this.textBox3.Text,
                        description = description
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewTask form = new NewTask(this.textBox3.Text, this.listBox2);
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox2.Text.Length != 0)
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "removeTask",
                        id = this.textBox3.Text,
                        taskId = ((Var.ListBoxItem)listBox2.SelectedItem).ID
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                        MessageBox.Show(str);
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
            else
            {
                MessageBox.Show("Оберіть завдання для видалення!");
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getTaskCondition",
                    id = this.textBox3.Text,
                    taskId = ((Var.ListBoxItem)listBox2.SelectedItem).ID
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text.Length != 0)
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "removeStudent",
                        id = this.textBox3.Text,
                        login = ((Var.ListBoxItem)listBox1.SelectedItem).ID
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
                        MessageBox.Show(str);
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
            else
            {
                MessageBox.Show("Оберіть студента для видалення!");
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox2.Text.Length != 0)
            {
                var id = this.textBox3.Text;
                var taskId = ((Var.ListBoxItem)listBox2.SelectedItem).ID;
                var user = ((Var.ListBoxItem)listBox1.SelectedItem).ID;
                TaskTeacher form = new TaskTeacher(id, taskId, user);
                form.Show();
            }
            else
            {
                MessageBox.Show("Оберіть завдання!");
            }

        }
    }
}
