using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string fileName = null;
        public string[] files = null;    //获得的所有文件路径
        public string[] pattern = { "*.gif", "*.jpg", "*.png", "*.bmp" };//定义搜寻的图片格式
        public string outfolderpath = null; //输出文件夹路径
        public static Bitmap ReadImageFile(string path) //返回图片流
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            Bitmap bit=null;
            try
            {
                fs.Read(image, 0, filelength); //按字节流读取 
                System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
                fs.Close();
                bit = new Bitmap(result);

            }
            catch (ArgumentException)
            {
                MessageBox.Show("There was an error." +
                    "Check the path to the image file.");
            }
            return bit;
        }

        private void button3_Click(object sender, EventArgs e) //选择图片文件夹按钮
        {
            string folderpath = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择图片所在文件夹";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderpath = dialog.SelectedPath;
                if (string.IsNullOrEmpty(folderpath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                textBox2.Text = Convert.ToString(folderpath);
                textBox1.Text = Convert.ToString(folderpath);
                files = GetImages(folderpath, pattern);
                outfolderpath = folderpath;
                //foreach (string file in files)
                //{
                //    MessageBox.Show(file);
                //}
            }
        }
        private void button1_Click_1(object sender, EventArgs e) //选择输出文件夹按钮
        {
            string folderpath = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择图片输出目录";
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderpath = dialog.SelectedPath;
                if (string.IsNullOrEmpty(folderpath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                textBox1.Text = Convert.ToString(folderpath);
                outfolderpath = folderpath;
            }
        }
        private void trans(string format) //转换并存储函数
        {
            if ( files==null || files.Length==0)
            {
                MessageBox.Show("请选择含有图片的文件夹！");
                return;
            }
            foreach (string file in files)
            {
                Bitmap bit = ReadImageFile(file);
                string nfile = Path.ChangeExtension(file, "." + format);
                string nnfile = Path.GetFileName(nfile); //文件名和后缀
                Random rnd = new Random();
                string ran = rnd.Next(1, 100).ToString();
                string nnnfile = outfolderpath + "\\" + ran + nnfile; //文件绝对路径
                try
                {
                    switch (format)
                        {
                            case "gif":
                                bit.Save(nnnfile, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                            case "png":
                                bit.Save(nnnfile, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                            case "bmp":
                                bit.Save(nnnfile, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                            case "ico":
                                bit.Save(nnnfile, System.Drawing.Imaging.ImageFormat.Icon);
                            break;
                            default:
                                MessageBox.Show("There was an error." +
                                "Check the " + file);
                            break;
                        }
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error." +
                        "Check the "+ file);
                    return;
                }
            }
            MessageBox.Show("转换"+ format +"完成！");
        }
        private string[] GetImages(string dirPath, params string[] searchPatterns) //https://blog.csdn.net/wulex/article/details/78796507 getfiles()获取多种后缀的文件路径
        {
            if (searchPatterns.Length <= 0)
            {
                return null;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                FileInfo[][] fis = new FileInfo[searchPatterns.Length][];
                int count = 0;
                for (int i = 0; i < searchPatterns.Length; i++)
                {
                    FileInfo[] fileInfos = di.GetFiles(searchPatterns[i]);
                    fis[i] = fileInfos;
                    count += fileInfos.Length;
                }
                string[] files = new string[count];
                int n = 0;
                for (int i = 0; i <= fis.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < fis[i].Length; j++)
                    {
                        string temp = fis[i][j].FullName;
                        files[n] = temp;
                        n++;
                    }
                }
                return files;
            }
        }
        private void button4_Click(object sender, EventArgs e) //转换成Gif
        {
            trans("gif");
        }
        private void button2_Click(object sender, EventArgs e) //转换成png
        {
            trans("png");
        }

        private void button5_Click(object sender, EventArgs e) //转换成bmp
        {
            trans("bmp");
        }

        private void button6_Click(object sender, EventArgs e) //转换成icon
        {
            trans("ico");
        }
    }
}
