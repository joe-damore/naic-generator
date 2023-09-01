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

namespace naic
{

    /// <summary>
    /// Interaction logic for RegionCreditRequirementsControl.xaml
    /// </summary>
    public partial class RegionCreditRequirementsControl : UserControl
    {
        /// Bindable property for requirement
        public static readonly DependencyProperty RequirementProperty = DependencyProperty.Register("Requirement", typeof(CreditRequirement), typeof(RegionCreditRequirementsControl));

        public CreditRequirement Requirement
        {
            get
            {
                return (CreditRequirement)GetValue(RequirementProperty);
            }
            set
            {
                SetValue(RequirementProperty, value);
            }
        }

        public RegionCreditRequirementsControl()
        {
            InitializeComponent();

            this.tbCreditAmount.DataContext = this;
            this.cbCreditType.DataContext = this;
            this.cbCreditCondition.DataContext = this;
        }
    }
}
