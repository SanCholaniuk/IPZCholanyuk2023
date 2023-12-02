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
    public partial class TaskStudent : Form
    {
        string Id;
        string TaskId;
        public TaskStudent(string id, string taskId)
        {
            Id = id;
            TaskId = taskId;
            InitializeComponent();
            if (Socket.ws.ReadyState.ToString() == "Open")
            {
                var data = new Json
                {
                    func = "getTaskInfo",
                    login = Var.Login,
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
                        this.textBox1.ReadOnly = true;
                        this.textBox1.Text = tmp[2];
                    }
                    if (tmp.Length == 4)
                    {
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
            if (this.textBox1.ReadOnly)
            {
                MessageBox.Show("Ви вже відправили на оцінювання дане завдання!");
            }
            else
            {
                string answer = this.textBox1.Text;
                if (answer.Length == 0)
                {
                    MessageBox.Show("Заповніть поле «Відповідь»!");
                }
                else
                {
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "addAnswer",
                            login = Var.Login,
                            id = Id,
                            taskId = TaskId,
                            answer = answer
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
    }
}
