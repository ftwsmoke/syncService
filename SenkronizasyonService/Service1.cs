using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace SenkronizasyonService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Sync();
            timer.Elapsed += new ElapsedEventHandler(onElapsedTime);
            timer.Interval = 60000;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void onElapsedTime(object source, ElapsedEventArgs e)
        {
            Sync();
        }
        private void Sync()
        {
            // WebSocket sunucusuna yeniden bağlanmayı başlat
            // PowerShell betiği dosya yolu
            string scriptPath = @"C:\xampp\htdocs\toy_otel\synchronization_service.ps1";

            // PowerShell'i başlatmak için gerekli olan bilgileri oluştur
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // PowerShell işlemini başlat
            using (Process process = Process.Start(psi))
            {
                // Çıktıyı al
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }
    }
}
