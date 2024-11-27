using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Vozila
{
    public partial class Form1 : Form
    {

        private List<Vozila> listaVozila = new List<Vozila>();
        public Form1()
        {
            InitializeComponent();
            // Dodaj opcije za sortiranje direktno u ComboBox
            comboBox1.Items.AddRange(new string[] { "Marka", "Model", "Godina proizvodnje", "Kilometraža" });

            // Postavi podrazumevani izbor
            comboBox1.SelectedIndex = 0;

            

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMarka.Text) || string.IsNullOrWhiteSpace(txtModel.Text) || !int.TryParse(txtGodinaProizvodnje.Text, out int godinaProizvodnje) || !int.TryParse(txtKilometraza.Text, out int kilometraza))
            {
                MessageBox.Show("Unesite ispravne podatke za vozilo!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kreiraj novo vozilo
            Vozila novoVozilo = new Vozila(
                txtMarka.Text,
                txtModel.Text,
                godinaProizvodnje,
                kilometraza
            );

            // Dodaj u listu
            listaVozila.Add(novoVozilo);

            // Prikaz u ListBox-u
            listbox.Items.Add(novoVozilo);

            // Očisti polja
            txtMarka.Clear();
            txtModel.Clear();
            txtGodinaProizvodnje.Clear();
            txtKilometraza.Clear();
        }

        private void btnSortiraj_Click(object sender, EventArgs e)
        {

            var opcija = comboBox1.SelectedItem.ToString();
            IOrderedEnumerable<Vozila> sortiranaVozila = null;

            
            switch (opcija)
            {
                case "Marka":
                    sortiranaVozila = listaVozila.OrderBy(v => v.Marka); 
                    break;

                case "Model":
                    sortiranaVozila = listaVozila.OrderBy(v => v.Model); 
                    break;

                case "Godina proizvodnje":
                    sortiranaVozila = listaVozila.OrderBy(v => v.GodinaProizvodnje); 
                    break;

                case "Kilometraža":
                    sortiranaVozila = listaVozila.OrderBy(v => v.Kilometraza);
                    break;

                default:
                    MessageBox.Show("Nepoznata opcija za sortiranje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }

            
            txtSortirano.Clear();
            foreach (var vozilo in sortiranaVozila)
            {
                txtSortirano.AppendText(vozilo.ToString() + Environment.NewLine);
            }

        }

        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                Vozila voziloZaBrisanje = (Vozila)listbox.SelectedItem;
                listaVozila.Remove(voziloZaBrisanje); 
                listbox.Items.Remove(voziloZaBrisanje); 
            }
            else
            {
                MessageBox.Show("Nema odabranog vozila za brisanje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SpremiUXml()
        {
            
            if (listaVozila.Count == 0)
            {
                MessageBox.Show("Nema vozila za spremanje!", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            var xmlDokument = new XDocument(
                new XElement("Vozila",
                    from vozilo in listaVozila
                    select new XElement("Vozilo",
                        new XElement("Marka", vozilo.Marka),
                        new XElement("Model", vozilo.Model),
                        new XElement("GodinaProizvodnje", vozilo.GodinaProizvodnje),
                        new XElement("Kilometraza", vozilo.Kilometraza)
                    )
                )
            );

            
            string putanja = "vozilo.xml";

            try
            {
                
                xmlDokument.Save(putanja);
                MessageBox.Show($"Podaci su uspešno sačuvani u datoteku: {putanja}", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške prilikom spremanja: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSpremiXml_Click(object sender, EventArgs e)
        {
            SpremiUXml();
        }
    }
}
