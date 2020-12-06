using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Logique d'interaction pour frmInscription.xaml
    /// </summary>
    public partial class frmInscription : Window
    {
        sncfEntities gst;
        public frmInscription()
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
            lstAgents.ItemsSource = from a in gst.agent.ToList()
                                    where !gst.inscription.ToList().FindAll(i => i.numFormation == (lstFormations.SelectedItem as formation).idFormation).Any(agent => agent.numAgent == a.idAgent)
                                    select a;
        }

        private async void lstAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HttpClient ws = new HttpClient(); // ws pour WebService
            string reponse = await ws.GetStringAsync("https://fr.distance24.org/route.json?stops=" + (lstFormations.SelectedItem as formation).lieuFormation + "|" + (lstAgents.SelectedItem as agent).villeAgent); // '%20' = ' ' = espace
            var json = JsonConvert.DeserializeObject<dynamic>(reponse);
            txtKm.Text = json.distance;
            var laFormation = lstFormations.SelectedItem as formation;
            var total = laFormation.prixFormation * laFormation.dureeFormation + Convert.ToInt16(json.distance) * 1.36;
            txtTotal.Text = total.ToString();
        }

        private void btnInscrire_Click(object sender, RoutedEventArgs e)
        {
            if (lstFormations.SelectedItem == null)
            {
                MessageBox.Show("Une formation !");
            }
            else if (lstAgents.SelectedItem == null)
            {
                MessageBox.Show("Un agent !");
            }
            else
            {
                gst.inscription.Add(new inscription() { nbKm = Convert.ToInt16(txtKm.Text), numAgent = (lstAgents.SelectedItem as agent).idAgent, numFormation = (lstFormations.SelectedItem as formation).idFormation, presence = 0 });
                MessageBox.Show("Votre inscription a bien été enregistrée", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
