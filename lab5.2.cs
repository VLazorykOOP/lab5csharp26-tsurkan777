using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        private TextBox textBoxOutput;
        private Button btnCollect;
        private bool disposed = false;

        public Form1()
        {
            this.Text = "Демонстрація конструкторів та деструкторів";
            this.Size = new Size(900, 700);

            textBoxOutput = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(textBoxOutput);

            btnCollect = new Button
            {
                Text = "Викликати GC.Collect() (знищити об'єкти)",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnCollect.Click += BtnCollect_Click;
            this.Controls.Add(btnCollect);

            DemonstrateConstructorsDestructors();
        }

        private void BtnCollect_Click(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            textBoxOutput.Text += "\n========== GC ВИКЛИКАНО ==========\n";
            textBoxOutput.Text += "Перевірте Output (Debug) для повідомлень деструкторів\n";
        }

        private void DemonstrateConstructorsDestructors()
        {
            string output = "========== ПОЧАТОК СТВОРЕННЯ ОБ'ЄКТІВ ==========\n\n";

            Organization[] companies = new Organization[]
            {
                new InsuranceCompany("Альфа-Страх", 1998, "Київ", 15000000, 25000, "автомобільне"),
                new InsuranceCompany("Бета-Гарант", 2012, "Одеса", 8500000),
                new InsuranceCompany(),
                new OilGasCompany("УкрНафтоГаз", 1991, "Львів", 85000000, 45.2, "нафта"),
                new OilGasCompany("ГазТранс", 1985, "Полтава", 120000000),
                new OilGasCompany(),
                new Factory("Прометей-Метал", 2005, "Дніпро", 32000000, 12, "стальні конструкції"),
                new Factory("ЗапоріжСталь", 1970, "Запоріжжя", 95000000),
                new Factory()
            };

            output += $"\n========== ОБ'ЄКТИ СТВОРЕНО ==========\n\n";
            output += $"Загальна кількість: {Organization.TotalCreated}\n";
            output += $"Страхових: {InsuranceCompany.TotalCreated}\n";
            output += $"Нафтогазових: {OilGasCompany.TotalCreated}\n";
            output += $"Заводів: {Factory.TotalCreated}\n\n";

            output += "========== ІНФОРМАЦІЯ ==========\n\n";
            foreach (var org in companies)
            {
                output += org.GetInfo() + "\n\n";
            }

            textBoxOutput.Text = output;
        }

        public abstract class Organization
        {
            public string Name { get; set; }
            public int FoundedYear { get; set; }
            public string City { get; set; }
            public decimal AnnualRevenue { get; set; }

            protected static int totalCreated = 0;
            public static int TotalCreated => totalCreated;

            protected Organization(string name, int foundedYear, string city, decimal revenue)
            {
                Name = name;
                FoundedYear = foundedYear;
                City = city;
                AnnualRevenue = revenue;
                totalCreated++;
                Debug.WriteLine($"✓ Organization (повний): '{Name}'");
            }

            protected Organization(string name, int foundedYear, string city)
            {
                Name = name;
                FoundedYear = foundedYear;
                City = city;
                AnnualRevenue = 0;
                totalCreated++;
                Debug.WriteLine($"✓ Organization (без доходу): '{Name}'");
            }

            protected Organization()
            {
                Name = "Без назви";
                FoundedYear = DateTime.Now.Year;
                City = "Невідомо";
                AnnualRevenue = 0;
                totalCreated++;
                Debug.WriteLine($"✓ Organization (default): '{Name}'");
            }

            ~Organization()
            {
                Debug.WriteLine($"✗ Organization: Знищено '{Name}'");
            }

            public abstract string GetInfo();
        }

        public class InsuranceCompany : Organization
        {
            public int PolicyCount { get; set; }
            public string MainInsuranceType { get; set; }

            protected static new int totalCreated = 0;
            public static new int TotalCreated => totalCreated;

            public InsuranceCompany(string name, int year, string city, decimal revenue, int policies, string type)
                : base(name, year, city, revenue)
            {
                PolicyCount = policies;
                MainInsuranceType = type;
                totalCreated++;
                Debug.WriteLine($"  → InsuranceCompany (повний): '{Name}'");
            }

            public InsuranceCompany(string name, int year, string city, decimal revenue)
                : base(name, year, city, revenue)
            {
                PolicyCount = 0;
                MainInsuranceType = "універсальне";
                totalCreated++;
                Debug.WriteLine($"  → InsuranceCompany (базовий): '{Name}'");
            }

            public InsuranceCompany() : base()
            {
                PolicyCount = 0;
                MainInsuranceType = "не вказано";
                totalCreated++;
                Debug.WriteLine($"  → InsuranceCompany (default): '{Name}'");
            }

            ~InsuranceCompany()
            {
                Debug.WriteLine($"  ← InsuranceCompany: Знищено '{Name}'");
            }

            public override string GetInfo()
            {
                return $"[Страхова] {Name} ({City}, {FoundedYear}) | Полісів: {PolicyCount}";
            }
        }

        public class OilGasCompany : Organization
        {
            public double DailyProduction { get; set; }
            public string MainProduct { get; set; }

            protected static new int totalCreated = 0;
            public static new int TotalCreated => totalCreated;

            public OilGasCompany(string name, int year, string city, decimal revenue, double production, string product)
                : base(name, year, city, revenue)
            {
                DailyProduction = production;
                MainProduct = product;
                totalCreated++;
                Debug.WriteLine($"  → OilGasCompany (повний): '{Name}'");
            }

            public OilGasCompany(string name, int year, string city, decimal revenue)
                : base(name, year, city, revenue)
            {
                DailyProduction = 0;
                MainProduct = "не вказано";
                totalCreated++;
                Debug.WriteLine($"  → OilGasCompany (базовий): '{Name}'");
            }

            public OilGasCompany() : base()
            {
                DailyProduction = 0;
                MainProduct = "не вказано";
                totalCreated++;
                Debug.WriteLine($"  → OilGasCompany (default): '{Name}'");
            }

            ~OilGasCompany()
            {
                Debug.WriteLine($"  ← OilGasCompany: Знищено '{Name}'");
            }

            public override string GetInfo()
            {
                return $"[Нафтогаз] {Name} ({City}, {FoundedYear}) | Добуток: {DailyProduction}";
            }
        }

        public class Factory : Organization
        {
            public int ProductionLines { get; set; }
            public string ManufacturedGoods { get; set; }

            protected static new int totalCreated = 0;
            public static new int TotalCreated => totalCreated;

            public Factory(string name, int year, string city, decimal revenue, int lines, string goods)
                : base(name, year, city, revenue)
            {
                ProductionLines = lines;
                ManufacturedGoods = goods;
                totalCreated++;
                Debug.WriteLine($"  → Factory (повний): '{Name}'");
            }

            public Factory(string name, int year, string city, decimal revenue)
                : base(name, year, city, revenue)
            {
                ProductionLines = 0;
                ManufacturedGoods = "не вказано";
                totalCreated++;
                Debug.WriteLine($"  → Factory (базовий): '{Name}'");
            }

            public Factory() : base()
            {
                ProductionLines = 0;
                ManufacturedGoods = "не вказано";
                totalCreated++;
                Debug.WriteLine($"  → Factory (default): '{Name}'");
            }

            ~Factory()
            {
                Debug.WriteLine($"  ← Factory: Знищено '{Name}'");
            }

            public override string GetInfo()
            {
                return $"[Завод] {Name} ({City}, {FoundedYear}) | Ліній: {ProductionLines}";
            }
        }
    }
