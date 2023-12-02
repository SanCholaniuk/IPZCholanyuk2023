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
    public partial class TaskTeacher : Form
    {
        string Id;
        string TaskId;
        string User;
        public TaskTeacher(string id, string taskId, string user)
        {
            Id = id;
            TaskId = taskId;
            User = user;
            InitializeComponent();
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getTaskInfo",
                    login = user,
                    id = id,
                    taskId = taskId
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str != "")
                {
                    var tmp = str.Split('\n');
                    this.label1.Text = tmp[0];
                    this.textBox2.Text = tmp[1];
                    if (tmp.Length >= 3)
                    {
                        this.textBox1.Text = tmp[2];
                    }
                    if (tmp.Length == 4)
                    {
                        this.textBox3.ReadOnly = true;
                        this.textBox3.Text = tmp[3];
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
            if (this.textBox3.ReadOnly)
            {
                MessageBox.Show("Ви вже оцінили дане завдання!");
            }
            else
            {
                string rating = this.textBox3.Text;
                if (rating.Length == 0)
                {
                    MessageBox.Show("Заповніть поле «Оцінка»!");
                }
                else if (int.Parse(rating) < 0 || int.Parse(rating) > 100)
                {
                    MessageBox.Show("Оцінка повинна бути в межах від 0 до 100!");
                }
                else
                {
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "addRating",
                            login = User,
                            id = Id,
                            taskId = TaskId,
                            rating = rating
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str != "")
                        {
                            MessageBox.Show(str);
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
