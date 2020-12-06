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
using System.Windows.Shapes;

namespace FormationWPF
{
    /// <summary>
    /// Logique d'interaction pour frmPresence.xaml
    /// </summary>
    public partial class frmPresence : Window
    {
        sncfEntities gst;
        public frmPresence()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gst = new sncfEntities();
            lstFormations.ItemsSource = gst.formation.ToList();
        }

        private void lstFormations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstInscriptions.ItemsSource = gst.inscription.ToList().FindAll(i => i.numFormation == (lstFormations.SelectedItem as formation).idFormation);
        }

        private void btnPresence_Click(object sender, RoutedEventArgs e)
        {
            gst.SaveChanges();
        }
    }
}
