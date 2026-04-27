using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Form1 : Form  // ✅ Додано 'partial'
    {
        private TextBox _textBoxOutput;  // ✅ Змінено ім'я (починається з _)
        private Button _btnSearch;       // ✅ Змінено ім'я
        private Software[] _softwareBase; // ✅ Змінено ім'я + ініціалізація

        public Form1()
        {
            InitializeComponent();  // ✅ Додано (для Designer)
            
            this.Text = "Програмне забезпечення";
            this.Size = new Size(950, 700);
            
            _textBoxOutput = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };
            this.Controls.Add(_textBoxOutput);
            
            _btnSearch = new Button
            {
                Text = "🔍 Показати дозволені до використання",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            _btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(_btnSearch);
            
            InitializeSoftware();
        }

        private void InitializeSoftware()
        {
            string output = $"📅 Сьогодні: {DateTime.Now.ToShortDateString()}\n";
            output += new string('=', 50) + "\n\n";

            // Створення бази програмного забезпечення
            _softwareBase = new Software[]  // ✅ Ініціалізація поля
            {
                // Вільне ПЗ
                new FreeSoftware("Linux Ubuntu", "Canonical"),
                new FreeSoftware("Mozilla Firefox", "Mozilla Foundation"),
                
                // Умовно-безкоштовне
                new Freeware("WinRAR", "RARLAB", DateTime.Now.AddDays(-10), 30),   // ✅ Доступне
                new Freeware("WinZip", "Corel", DateTime.Now.AddDays(-40), 30),    // ❌ Прострочено
                
                // Комерційне
                new CommercialSoftware("MS Office 365", "Microsoft", 299.99m, DateTime.Now.AddDays(-5), 365),   // ✅
                new CommercialSoftware("Adobe Photoshop", "Adobe", 19.99m, DateTime.Now.AddDays(-400), 365)     // ❌
            };

            output += "📋 ВСЕ ПЗ З БАЗИ:\n" + new string('-', 50) + "\n\n";
            foreach (var sw in _softwareBase)
            {
                sw.Show(ref output);
                string status = sw.IsUsable() ? "✅ ДОЗВОЛЕНО" : "❌ ЗАБОРОНЕНО";
                output += $"   ➤ Статус: {status}\n\n";
            }

            _textBoxOutput.Text = output;
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            string output = $"🔍 РЕЗУЛЬТАТИ ПОШУКУ (дозволені на {DateTime.Now.ToShortDateString()}):\n";
            output += new string('=', 50) + "\n\n";

            bool found = false;
            foreach (var sw in _softwareBase)
            {
                if (sw.IsUsable())
                {
                    sw.Show(ref output);
                    output += "\n";
                    found = true;
                }
            }

            if (!found)
            {
                output += "⚠️ Немає програм, дозволених до використання.\n";
            }

            _textBoxOutput.Text = output;
        }
    }

    // ========== АБСТРАКТНИЙ БАЗОВИЙ КЛАС ==========
    public abstract class Software
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }

        protected Software(string name, string manufacturer)
        {
            Name = name;
            Manufacturer = manufacturer;
        }

        public abstract void Show(ref string output);
        public abstract bool IsUsable();
    }

    // ========== ВІЛЬНЕ ПЗ ==========
    public class FreeSoftware : Software
    {
        public FreeSoftware(string name, string manufacturer) : base(name, manufacturer) { }

        public override void Show(ref string output)
        {
            output += $"[🆓 Вільне] {Name}\n   Виробник: {Manufacturer}\n";
        }

        public override bool IsUsable() => true;
    }

    // ========== УМОВНО-БЕЗКОШТОВНЕ ==========
    public class Freeware : Software
    {
        public DateTime InstallDate { get; set; }
        public int TrialPeriodDays { get; set; }

        public Freeware(string name, string manufacturer, DateTime installDate, int trialDays) 
            : base(name, manufacturer)
        {
            InstallDate = installDate;
            TrialPeriodDays = trialDays;
        }

        public override void Show(ref string output)
        {
            DateTime expiry = InstallDate.AddDays(TrialPeriodDays);
            output += $"[🧪 Trial] {Name}\n   Виробник: {Manufacturer}\n" +
                     $"   Встановлено: {InstallDate.ToShortDateString()}\n" +
                     $"   Термін: {TrialPeriodDays} днів (до {expiry.ToShortDateString()})\n";
        }

        public override bool IsUsable()
        {
            return DateTime.Now <= InstallDate.AddDays(TrialPeriodDays);
        }
    }

    // ========== КОМЕРЦІЙНЕ ==========
    public class CommercialSoftware : Software
    {
        public decimal Price { get; set; }
        public DateTime InstallDate { get; set; }
        public int LicensePeriodDays { get; set; }

        public CommercialSoftware(string name, string manufacturer, decimal price, 
                                 DateTime installDate, int licenseDays) 
            : base(name, manufacturer)
        {
            Price = price;
            InstallDate = installDate;
            LicensePeriodDays = licenseDays;
        }

        public override void Show(ref string output)
        {
            DateTime expiry = InstallDate.AddDays(LicensePeriodDays);
            output += $"[💰 Комерційне] {Name}\n   Виробник: {Manufacturer}\n" +
                     $"   Ціна: {Price:C}\n" +
                     $"   Встановлено: {InstallDate.ToShortDateString()}\n" +
                     $"   Ліцензія: {LicensePeriodDays} днів (до {expiry.ToShortDateString()})\n";
        }

        public override bool IsUsable()
        {
            return DateTime.Now <= InstallDate.AddDays(LicensePeriodDays);
        }
    }
}
