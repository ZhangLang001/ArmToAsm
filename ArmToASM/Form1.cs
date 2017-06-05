using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ArmToASM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private ProcessStartInfo compeler;
        private Process outText;
        public string outpotly;
        private void convert_Click(object sender, EventArgs e)
        {
            var instructionText = instruction.Text;
            if (string.IsNullOrEmpty(instructionText))
            {
                error_tip.Text = "请输入指令哦";
                return;
               
            }

            this.compeler = new ProcessStartInfo();
            var processStartInfo = this.compeler;
            processStartInfo.FileName = "as.exe";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;
            var streamWriter = new StreamWriter("tmp");
            streamWriter.Write(instructionText);
            streamWriter.Close();
            result_tip.Text = "正在努力执行中...";
            try
            {
                this.compeler.Arguments = "-mthumb tmp -al";
                this.outText = Process.Start(this.compeler);
                var process = this.outText;
                if (process != null && !process.HasExited)
                {
                    this.outpotly = process.StandardOutput.ReadToEnd();
                }
                if (this.outpotly.Contains("Error:"))
                {
                    error_tip.Text = "请检查指令是否输入正确哦";
                    File.Delete("tmp");
                    return;
                }
                thumb_text.Text = this.outpotly.Substring(38, 4);
            }
            catch (Exception exception)
            {
                // ignored
                File.Delete("tmp");
                error_tip.Text = "转换出现异常";
            }
            try
            {
                this.compeler.Arguments = "tmp -al";
                this.outText = Process.Start(this.compeler);
                var process = this.outText;
                if (process != null && !process.HasExited)
                {
                    this.outpotly = process.StandardOutput.ReadToEnd();
                }
                arm_text.Text = this.outpotly.Substring(38, 8);
            }
            catch (Exception exception)
            {
                // ignored
                error_tip.Text = "转换出现异常";
                File.Delete("tmp");
            }
            result_tip.Text = "执行完成";
            File.Delete("tmp");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("as.exe"))
            {
                error_tip.Text = "请检测根目录下是否包含as.exe程序";
                
            }
            this.FormBorderStyle=FormBorderStyle.FixedSingle;
           


        }

        private void clear_Click(object sender, EventArgs e)
        {
            thumb_text.Text = "";
            arm_text.Text = "";
            instruction.Text = "";
        }
    }
}
