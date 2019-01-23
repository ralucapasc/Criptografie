using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;

namespace Criptografie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] encKey = null;
        byte[] encIV = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void writeBuffer(string fname,byte[] buffer)
        {
            FileStream fout = new FileStream(fname, FileMode.Create, FileAccess.Write);
            fout.Write(buffer,0,buffer.Length);
            fout.Close();
        }
        private byte[] readBuffer(string fname)
        {
            //FileStream fin = new FileStream(fname, FileMode.Open, FileAccess.Read);
            //fin.Close();
            return File.ReadAllBytes(fname);
        }        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            string newkey = " ";
            string newIV = " ";
            FileStream fin = new FileStream("D:/abcbi.txt",FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream("D:/dest.enc",FileMode.OpenOrCreate, FileAccess.Write);
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            ICryptoTransform encryptor = cryptoProvider.CreateEncryptor();
            CryptoStream stream = new CryptoStream(fout, encryptor, CryptoStreamMode.Write);
            byte[] input = new byte[128];
            int inLen = -1;
            while ((inLen = fin.Read(input, 0, 128)) > 0)
            {
                stream.Write(input, 0, inLen);
            }
            encKey = cryptoProvider.Key;
            encIV = cryptoProvider.IV;
            for (int i = 0; i < encKey.Length; i++)
                newkey += encKey[i].ToString();
            TextBlockKey.Text = newkey + "";
            writeBuffer("D:/fkey.key", cryptoProvider.Key);
            for (int i = 0; i < encIV.Length; i++)
                newIV += encIV[i].ToString();
            TextBlockIV.Text = newIV + "";
            writeBuffer("D:/fIV.iv", cryptoProvider.IV);
            stream.Close(); fout.Close(); fin.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            FileStream fin = new FileStream("D:/dest.enc",FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream("D:/abcbi.dec.txt", FileMode.OpenOrCreate, FileAccess.Write);
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            encKey = readBuffer("D:/fkey.key");
            encIV = readBuffer("D:/fIV.iv");
            ICryptoTransform decryptor = cryptoProvider.CreateDecryptor(encKey, encIV);
            CryptoStream stream = new CryptoStream(fout, decryptor, CryptoStreamMode.Write);
            byte[] input = new byte[128];
            int inLen = -1;
            while ((inLen = fin.Read(input, 0, 128)) > 0)
            {
                stream.Write(input, 0, inLen);
            }
            stream.Close();  fout.Close(); fin.Close();
        }

    }
}
