using System;
using System.Windows.Forms;

namespace WinFormsApp2  // ✅ Змінили namespace на правильний
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // ❌ Видалили InitializeComponent() - він не потрібен для цього завдання
            RunTask(); 
        }

        private void RunTask()
        {
            // Створення масиву об'єктів
            Organization[] companies = new Organization[]
            {
                new InsuranceCompany("Альфа-Страх", 1998, "Київ", 15000000, 25000, "автомобільне"),
                new OilGasCompany("УкрНафтоГаз", 1991, "Львів", 85000000, 45.2, "нафта"),
                new Factory("Прометей-Метал", 2005, "Дніпро", 32000000, 12, "стальні конструкції"),
                new InsuranceCompany("Бета-Гарант", 2012, "Одеса", 8500000, 11000, "медичне"),
                new OilGasCompany("ГазТранс", 1985, "Полтава", 120000000, 67.8, "природний газ"),
                new Factory("ЗапоріжСталь", 1970, "Запоріжжя", 95000000, 25, "прокат"),
                new InsuranceCompany("Грін-Страх", 2001, "Харків", 12000000, 18000, "майнове"),
                new Factory("КиївМаш", 1965, "Київ", 45000000, 18, "промислове обладнання")
            };

            // Сортування за роком заснування
            Array.Sort(companies, (a, b) => a.FoundedYear.CompareTo(b.FoundedYear));

            // Формуємо текст для виводу
            string output = "=== Відсортовано за роком заснування ===\n\n";
            foreach (var org in companies)
            {
                output += org.GetInfo() + "\n";
            }

            // Показуємо результат
            MessageBox.Show(output, "Результат");
        }
    }

    // Абстрактний базовий клас
    public abstract class Organization
    {
        public string Name { get; set; }
        public int FoundedYear { get; set; }
        public string City { get; set; }
        public decimal AnnualRevenue { get; set; }

        protected Organization(string name, int foundedYear, string city, decimal revenue)
        {
            Name = name;
            FoundedYear = foundedYear;
            City = city;
            AnnualRevenue = revenue;
        }

        public abstract string GetInfo();
    }

    // Страхова компанія
    public class InsuranceCompany : Organization
    {
        public int PolicyCount { get; set; }
        public string MainInsuranceType { get; set; }

        public InsuranceCompany(string name, int year, string city, decimal revenue, int policies, string type)
            : base(name, year, city, revenue)
        {
            PolicyCount = policies;
            MainInsuranceType = type;
        }

        public override string GetInfo()
        {
            return $"[Страхова] {Name} ({City}, {FoundedYear}) | Полісів: {PolicyCount} | Тип: {MainInsuranceType}";
        }
    }

    // Нафтогазова компанія
    public class OilGasCompany : Organization
    {
        public double DailyProduction { get; set; }
        public string MainProduct { get; set; }

        public OilGasCompany(string name, int year, string city, decimal revenue, double production, string product)
            : base(name, year, city, revenue)
        {
            DailyProduction = production;
            MainProduct = product;
        }

        public override string GetInfo()
        {
            return $"[Нафтогаз] {Name} ({City}, {FoundedYear}) | Добуток: {DailyProduction} тис. од. | Продукт: {MainProduct}";
        }
    }

    // Завод
    public class Factory : Organization
    {
        public int ProductionLines { get; set; }
        public string ManufacturedGoods { get; set; }

        public Factory(string name, int year, string city, decimal revenue, int lines, string goods)
            : base(name, year, city, revenue)
        {
            ProductionLines = lines;
            ManufacturedGoods = goods;
        }

        public override string GetInfo()
        {
            return $"[Завод] {Name} ({City}, {FoundedYear}) | Ліній: {ProductionLines} | Продукція: {ManufacturedGoods}";
        }
    }
}
