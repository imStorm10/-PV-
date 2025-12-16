using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace PVMonitorWinForms
{
    public partial class Form1 : Form
    {
        private AppDbContext _db;

        public Form1()
        {
            InitializeComponent();

            // Инициализация на базата данни (In-Memory)
            _db = new AppDbContext();
            _db.Database.EnsureCreated();

            // Зареждане на празна таблица (в началото)
            LoadHistory();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Вземане на данни от текстови полета
            double power = 0;
            double irradiation = 0;
            double temp = 0;

            try
            {
                power = double.Parse(txtPowerInput.Text);
                irradiation = double.Parse(txtIrradInput.Text);
                temp = double.Parse(txtTempInput.Text);
            }
            catch
            {
                MessageBox.Show("Моля въведете валидни числа (използвайте запетая или точка според настройките си)!");
                return;
            }

            // Модул за откриване на аномалии 
            // Логика: Висока радиация + Ниска мощност = Проблем
            string alarmStatus = "OK";
            Color statusColor = Color.Green;

            // Пример: Ако слънцето е силно (>900), а мощността е слаба (<4.0)
            if (irradiation > 900 && power < 4.0)
            {
                alarmStatus = "АНОМАЛИЯ: Частично засенчване"; 
                statusColor = Color.Red;
            }
            else if (temp > 65)
            {
                alarmStatus = "ВНИМАНИЕ: Прегряване"; 
                statusColor = Color.Orange;
            }

            // Обновяване на Интерфейса
            lblPower.Text = $"{power} kW";
            lblIrradiation.Text = $"{irradiation} W/m²";
            lblTemp.Text = $"{temp} °C";

            // Изчисляване на ефективност
            double efficiency = 0;
            if (irradiation > 0)
            {
                efficiency = (power * 1000 / (irradiation * 10 * 1.6)) * 100;   // 10 панела по 1.6 квадрата
            }
            lblEfficiency.Text = $"{efficiency:F1} %";

            lblStatus.Text = $"Alarm Статус: {alarmStatus}";
            lblStatus.ForeColor = statusColor;

            // 4. Запис в Базата Данни
            var newReading = new Reading
            {
                Time = DateTime.Now,
                Power = power,
                Irradiation = irradiation,
                Temperature = temp,
                Status = alarmStatus
            };

            _db.Readings.Add(newReading);
            _db.SaveChanges();

            // Обновяване на таблицата
            LoadHistory();
        }

        private void LoadHistory()
        {
            var history = _db.Readings
                             .OrderByDescending(r => r.Time)
                             .Take(15)
                             .ToList();

            dgvHistory.DataSource = history;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}