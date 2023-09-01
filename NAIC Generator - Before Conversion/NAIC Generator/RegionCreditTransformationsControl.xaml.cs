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
    /// Interaction logic for RegionCreditTransformationsControl.xaml
    /// </summary>
    public partial class RegionCreditTransformationsControl : UserControl
    {
        /// Bindable property for requirement
        public static readonly DependencyProperty TransformationProperty = DependencyProperty.Register("Transformation", typeof(CreditTransformation), typeof(RegionCreditTransformationsControl));

        public CreditTransformation Transformation
        {
            get
            {
                return (CreditTransformation)GetValue(TransformationProperty);
            }
            set
            {
                SetValue(TransformationProperty, value);
            }
        }

        public RegionCreditTransformationsControl()
        {
            InitializeComponent();

            this.cbActionType.DataContext      = this;
            this.cbCreditType.DataContext      = this;
            this.cbDestination.DataContext     = this;
            this.cbDestinationType.DataContext = this;
            this.cbCreditAmount.DataContext    = this;
        }
    }
}
