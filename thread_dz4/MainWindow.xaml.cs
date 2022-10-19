using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace thread_dz4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int i = 0;
        int value = 0;
        ListBox lb2 = new ListBox();
        Mutex mutexObj = new Mutex();
        ListBox lb3 = new ListBox();
        public MainWindow()
        {
            InitializeComponent();        
        }

        //dz1 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (i < 2)
            {
                MainWindow mn = new MainWindow();         
                i++;
                mn.Show();
                i += mn.i;
                mn.i = i;
            }
            else
            {
                MessageBox.Show("Открыто больше 3 окон");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var th = new Thread(() => create_list());
            th.Start();
            var th2 = new Thread(() => create_prost_number());
            th2.Start();
            var th3 = new Thread(() => create_seven());
            th3.Start();
            var th4 = new Thread(() => create_report());
            th4.Start();
        }
        private void create_list()
        {
            mutexObj.WaitOne();
            string path = @"C:\Users\karas\source\repos\thread_dz4\text.txt";
            File.WriteAllText(path, " ");
            Random rnd = new Random();
            value = rnd.Next(10, 30);
            for (int i = 0; i < value; i++)
            {
                Dispatcher.Invoke(new ThreadStart(delegate { lb1.Items.Add(rnd.Next(1, 50)); }));
                Dispatcher.Invoke(new ThreadStart(delegate { lb1.Items.Add(" "); }));                         
            }
            for (int i = 0; i < lb1.Items.Count; i++)
            {
                string s = (string)Dispatcher.Invoke(new ThreadStart(delegate { Convert.ToString(lb1.Items[i]); }));
                File.AppendAllText(path, Convert.ToString(lb1.Items[i]));
            }
            mutexObj.ReleaseMutex();
        }

        private void create_prost_number()
        {
            mutexObj.WaitOne();
            string path = @"C:\Users\karas\source\repos\thread_dz4\prost_number.txt";
            File.WriteAllText(path, " ");
            int count = 0;
            int number = 1; 
            while (count < value)
            {
                number++;
                try
                {
                    if (IsPrime(Convert.ToInt32(lb1.Items[count])))
                    {
                        Dispatcher.Invoke(new ThreadStart(delegate { lb2.Items.Add(lb1.Items[count]); ; }));                        
                        File.AppendAllText(path, Convert.ToString(lb1.Items[count]));
                        File.AppendAllText(path, " ");

                    }
                }
                catch
                {
                    
                }
                count++;
            }
            MessageBox.Show("Простые числа записаны в файл");
            mutexObj.ReleaseMutex();
        }
        
        private void create_seven()
        {
            mutexObj.WaitOne();
            string path2 = @"C:\Users\karas\source\repos\thread_dz4\seven.txt";          
            int a = 0;
            File.WriteAllText(path2, " ");
            for (int i = 0; i < lb2.Items.Count; i++)
            {
                Dispatcher.Invoke(new ThreadStart(delegate { lb3.Items.Add(lb2.Items[i]); }));
               
            }
            for (int i = 0; i < lb3.Items.Count; i++)
            {
                try
                {
                    a = Convert.ToInt32(lb2.Items[i]);
                    a -= 7;
                    if (a % 10 == 0)
                    {
                        File.AppendAllText(path2, Convert.ToString(lb2.Items[i]));
                        File.AppendAllText(path2, " ");
                    }
                }
                catch { }
            }
            MessageBox.Show("Числа записаны");
            mutexObj.ReleaseMutex();
        }

        public static bool IsPrime(int number)
        {
            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;        }
        private static bool isSimple(int N)
        {
            bool tf = false;
            for (int i = 2; i < (int)(N / 2); i++)
            {
                if (N % i == 0)
                {
                    tf = false;
                    break;
                }
                else
                {
                    tf = true;
                }
            }
            return tf;
        }

        private void create_report()
        {
            mutexObj.WaitOne();
            string path = @"C:\Users\karas\source\repos\thread_dz4\text.txt";
            string path2 = @"C:\Users\karas\source\repos\thread_dz4\prost_number.txt";
            string path3 = @"C:\Users\karas\source\repos\thread_dz4\seven.txt";
            report(path);
            report(path2);
            report(path3);
            mutexObj.ReleaseMutex();
        }
        private void report(string path)
        {
            string path4 = @"C:\Users\karas\source\repos\thread_dz4\report.txt";
            string s1 = File.ReadAllText(path);
            int c = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] == ' ')
                {
                    c++;
                }
            }
            File.AppendAllText(path4, " ");
            File.AppendAllText(path4, Convert.ToString(c - 1));          
            MessageBox.Show(Convert.ToString(c - 1));          
        }
    }
}
