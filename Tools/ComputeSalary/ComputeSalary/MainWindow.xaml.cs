using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputeSalary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCompute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var salaryBeforeTaxes = int.Parse(txtSalaryBeforeTaxes.Text.Trim());
                var socialSecurityBase = int.Parse(txtSocialSecurityBase.Text.Trim());
                var medicalBase = int.Parse(txtMedicalBase.Text.Trim());
                var housingFundBase = int.Parse(txtHousingFundBase.Text.Trim());

                var socialSecurityPersonalRatio = double.Parse(txtSocialSecurityPersonalRatio.Text.Trim()) / 100;
                var socialSecurityCompanyRatio = double.Parse(txtSocialSecurityCompanyRatio.Text.Trim()) / 100;

                var medicalPersonalRatio = double.Parse(txtMedicalPersonalRatio.Text.Trim()) / 100;
                var medicalCompanylRatio = double.Parse(txtMedicalCompanyRatio.Text.Trim()) / 100;

                var unemploymentPersonalRatio = double.Parse(txtUnemploymentPersonalRatio.Text.Trim()) / 100;
                var unemploymentCompanyRatio = double.Parse(txtUnemploymentCompanyRatio.Text.Trim()) / 100;

                var occupationalInjuryPersonalRatio = double.Parse(txtOccupationalInjuryPersonalRatio.Text.Trim()) / 100;
                var occupationalInjuryCompanyRatio = double.Parse(txtOccupationalInjuryCompanyRatio.Text.Trim()) / 100;

                var maternityPersonalRatio = double.Parse(txtMaternityPersonalRatio.Text.Trim()) / 100;
                var maternityCompanyRatio = double.Parse(txtMaternityCompanyRatio.Text.Trim()) / 100;

                var criticalIllnessPersonalRatio = double.Parse(txtCriticalIllnessPersonalRatio.Text.Trim()) / 100;
                var criticalIllnessCompanyRatio = double.Parse(txtCriticalIllnessCompanyRatio.Text.Trim()) / 100;

                var housingFundPersonalRatio = double.Parse(txtHousingFundPersonalRatio.Text.Trim()) / 100;
                var housingFundCompanyRatio = double.Parse(txtHousingFundCompanyRatio.Text.Trim()) / 100;

                var personalSum = socialSecurityBase * socialSecurityPersonalRatio +
                                  medicalBase * (medicalPersonalRatio + unemploymentPersonalRatio + occupationalInjuryPersonalRatio + maternityPersonalRatio + criticalIllnessPersonalRatio) +
                                  housingFundBase * housingFundPersonalRatio;

                var companySum = socialSecurityBase * socialSecurityCompanyRatio +
                                  medicalBase * (medicalCompanylRatio + unemploymentCompanyRatio + occupationalInjuryCompanyRatio + maternityCompanyRatio + criticalIllnessCompanyRatio) +
                                  housingFundBase * housingFundCompanyRatio;

                var taxSalary = salaryBeforeTaxes - personalSum;


                txtPersonalSum.Content = Math.Round(personalSum, 2);
                txtCompanySum.Content = Math.Round(companySum, 2);

                var salaryAfterTax = GetSalaryAfterTax(taxSalary);
                txtSalaryAfterTaxes.Content = Math.Round(salaryAfterTax, 2);
                txtSalaryTaxes.Content = Math.Round(taxSalary - salaryAfterTax, 2);
                txtCompanyPay.Content = salaryBeforeTaxes + companySum;

                txtHouseFundingIncome.Content = housingFundBase * (housingFundCompanyRatio + housingFundPersonalRatio);

            }
            catch (Exception ex)
            {
                //
            }
        }

        private double GetSalaryAfterTax(double taxSalary)
        {
            var salaryAfterTax = taxSalary;

            var taxSalarySubedBase = taxSalary - 3500;
            if (taxSalarySubedBase <= 0)
                return salaryAfterTax;

            var taxSalary1500 = taxSalarySubedBase - 1500;
            var taxSalary4500 = taxSalarySubedBase - 4500;
            var taxSalary9000 = taxSalarySubedBase - 9000;
            var taxSalary35000 = taxSalarySubedBase - 35000;
            var taxSalary55000 = taxSalarySubedBase - 55000;
            var taxSalary80000 = taxSalarySubedBase - 80000;

            if (taxSalary80000 > 0)
            {
                //超过8W部分，交税45%
                salaryAfterTax = salaryAfterTax - taxSalary80000 * 0.45;
            }

            //超过5.5W到8W之间，交税35%
            if (taxSalary55000 > 0 && taxSalary80000 <= 0)
            {                
                salaryAfterTax = salaryAfterTax - taxSalary55000 * 0.35;
            }
            else if (taxSalary55000 > 0 && taxSalary80000 > 0)
            {
                salaryAfterTax = salaryAfterTax - (80000 - 55000) * 0.35;
            }

            if (taxSalary35000 > 0 && taxSalary55000 <= 0)
            {
                salaryAfterTax = salaryAfterTax - taxSalary35000 * 0.30;
            }
            else if (taxSalary35000 > 0 && taxSalary55000 > 0)
            {
                salaryAfterTax = salaryAfterTax - (55000 - 35000) * 0.30;
            }

            if (taxSalary9000 > 0 && taxSalary35000 <= 0)
            {
                salaryAfterTax = salaryAfterTax - taxSalary9000 * 0.25;
            }
            else if (taxSalary9000 > 0 && taxSalary35000 > 0)
            {
                salaryAfterTax = salaryAfterTax - (35000 - 9000) * 0.25;
            }

            if (taxSalary4500 > 0 && taxSalary9000 <= 0)
            {
                salaryAfterTax = salaryAfterTax - taxSalary4500 * 0.20;
            }
            else if (taxSalary4500 > 0 && taxSalary9000 > 0)
            {
                salaryAfterTax = salaryAfterTax - (9000-4500) * 0.20;
            }

            if (taxSalary1500 > 0 && taxSalary4500 <= 0)
            {
                salaryAfterTax = salaryAfterTax - taxSalary1500 * 0.10;
            }
            else if (taxSalary1500 > 0 && taxSalary4500 > 0)
            {
                salaryAfterTax = salaryAfterTax - (4500 - 1500) * 0.10;
            }

            if (taxSalarySubedBase > 0 && taxSalary1500 <= 0)
            {
                salaryAfterTax = salaryAfterTax - taxSalarySubedBase * 0.03;
            }
            else if (taxSalarySubedBase > 0 && taxSalary1500 > 0)
            {
                salaryAfterTax = salaryAfterTax - 1500 * 0.03;
            }

            return salaryAfterTax;
        }

        private void MaxRatio()
        {
            txtSocialSecurityBase.Text = "15130";
            txtMedicalBase.Text = "14370";
            txtHousingFundBase.Text = "13500";

            txtSocialSecurityPersonalRatio.Text = "8";
            txtSocialSecurityCompanyRatio.Text = "19";

            txtMedicalPersonalRatio.Text = "2";
            txtMedicalCompanyRatio.Text = "6.5";

            txtUnemploymentPersonalRatio.Text = "0.40";
            txtUnemploymentCompanyRatio.Text = "0.60";

            txtOccupationalInjuryPersonalRatio.Text = "0";
            txtOccupationalInjuryCompanyRatio.Text = "0.224";

            txtMaternityPersonalRatio.Text = "0";
            txtMaternityCompanyRatio.Text = "0.50";

            txtCriticalIllnessPersonalRatio.Text = "0";
            txtCriticalIllnessCompanyRatio.Text = "1";

            txtHousingFundPersonalRatio.Text = "12";
            txtHousingFundCompanyRatio.Text = "12";
        }

        private void MinRatio()
        {
            txtSocialSecurityBase.Text = "2017";
            txtMedicalBase.Text = "2874";
            txtHousingFundBase.Text = "1500";

            txtSocialSecurityPersonalRatio.Text = "8";
            txtSocialSecurityCompanyRatio.Text = "19";

            txtMedicalPersonalRatio.Text = "2";
            txtMedicalCompanyRatio.Text = "6.5";

            txtUnemploymentPersonalRatio.Text = "0.40";
            txtUnemploymentCompanyRatio.Text = "0.60";

            txtOccupationalInjuryPersonalRatio.Text = "0";
            txtOccupationalInjuryCompanyRatio.Text = "0.224";

            txtMaternityPersonalRatio.Text = "0";
            txtMaternityCompanyRatio.Text = "0.50";

            txtCriticalIllnessPersonalRatio.Text = "0";
            txtCriticalIllnessCompanyRatio.Text = "1";

            txtHousingFundPersonalRatio.Text = "6";
            txtHousingFundCompanyRatio.Text = "6";
        }

        private void btnUpLimit_Click(object sender, RoutedEventArgs e)
        {
            MaxRatio();
        }

        private void btnDownLimit_Click(object sender, RoutedEventArgs e)
        {
            MinRatio();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MaxRatio();
            btnUpLimit.IsChecked = true;
        }
    }
}
