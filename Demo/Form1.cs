using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.ForeColor = Color.LightGray;
            textBox1.Text = "Viết câu truy vấn";
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            
        }
        private void LoadData(string strFilePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(strFilePath);
            if (lines.Length > 0)
            {
                //Header
                string strfirstLine = lines[0];
                string[] headerLabels = strfirstLine.Split(',');
                foreach (string strheader in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(strheader));
                }
                //Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] strData = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string strheader in headerLabels)
                    {
                        dr[strheader] = strData[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dgvdemo.DataSource = dt;
            }
        }
        public string ExecuteCommandSync(object command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo();
                procStartInfo.FileName = "cmd";
                procStartInfo.Arguments = ""+command;
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                return result;
            }
            catch (Exception objException)
            {
                return "";
            }
        }
        public void ghifile(string a)
        {
            string b = @"hive -e 'use demo; "+a+";' | sed 's/[\\t]\\+/,/g' > output8.csv";
            File.WriteAllText("E:\\DienToanDamMay\\CuoiKy\\docker-hive\\compile.sh", "cd ..\n" + b);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ghifile(textBox1.Text);

            string command = @"/c ""docker cp E:\DienToanDamMay\CuoiKy\docker-hive\compile.sh 3b208d8ab7e5:compile.sh"" ";
            ExecuteCommandSync(command);


            string batFileName = @"E:\DienToanDamMay\CuoiKy\docker-hive" + @"\" + Guid.NewGuid() + ".bat";
            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"E:");
                batFile.WriteLine($@"cd E:\DienToanDamMay\CuoiKy\docker-hive");
                batFile.WriteLine($"docker-compose exec hive-server bash ../compile.sh");
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process p = new Process();
            p.StartInfo = processStartInfo;
            p.Start();
            p.WaitForExit();
            File.Delete(batFileName);


            ExecuteCommandSync(@"/c ""docker cp docker-hive_hive-server_1:/output8.csv E:\DienToanDamMay\CuoiKy""");

            LoadData("E:/DienToanDamMay/CuoiKy/output8.csv");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExecuteCommandSync(@"/c ""docker stop 1a9 281 ca2 74e 3b2 64d""");
            MessageBox.Show("Stop thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCommandSync(@"/c ""docker start 1a9 281 ca2 74e 3b2 64d""");
            MessageBox.Show("Start thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Viết câu truy vấn";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Viết câu truy vấn")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem == "Những VDV lớn tuổi nhất giành được HCV")
            {
                textBox1.ForeColor = Color.Black;
                textBox1.Text = @"select distinct Name,tuoi,gam,team,sport from (select max(age) as tuoi from athlete where medal=""Gold"")a ,athlete b where a.tuoi=b.age";
            }
            if (comboBox3.SelectedItem == "Top 5 quốc gia giành nhiều huy chường vàng nhất")
            {
                textBox1.ForeColor = Color.Black;
                textBox1.Text = @"select region,count(Name) as sl from athlete INNER JOIN region on(athlete.NOC= region.NOC) where Medal= ""Gold"" group by region order by sl desc limit 5";
            }
            if (comboBox3.SelectedItem == "Top 10 quốc gia có nhiều vận động viên tham dự olympic")
            {
                textBox1.ForeColor = Color.Black;
                textBox1.Text = @"select region,count(Name) as sl from athlete INNER JOIN region on (athlete.noc=region.noc) group by region order by sl desc limit 10";
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string batFileName = @"E:\DienToanDamMay\CuoiKy\docker-hive" + @"\" + Guid.NewGuid() + ".bat";
            using (StreamWriter batFile = new StreamWriter(batFileName))
            {
                batFile.WriteLine($"C:");
                batFile.WriteLine($@" C:\Users\HOME\anaconda3\Scripts\activate.bat C:\Users\HOME\anaconda3 & conda activate tensorflow1");
                //batFile.WriteLine($"conda activate tensorflow1");
                batFile.WriteLine($@"cd C:\Tensorflow2\workspace\training_demo");
                batFile.WriteLine($"python Object_detection_image.py");
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + batFileName);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process p = new Process();
            p.StartInfo = processStartInfo;
            p.Start();
            p.WaitForExit();
            File.Delete(batFileName);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var workingDirectory = Path.GetFullPath("Scripts");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            process.WaitForExit();
            // Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    // Vital to activate Anaconda
                    sw.WriteLine("C:\\Users\\HOME\\anaconda3\\Scripts\\activate.bat");
                    // Activate your environment
                    sw.WriteLine("activate tensorflow1");
                    // Any other commands you want to run
                    sw.WriteLine("cd C:\\Tensorflow2\\workspace\\training_demo");
                    // run your script. You can also pass in arguments
                    sw.WriteLine("python Object_detection_image.py");
                }
            }

            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
        }
    }
}
