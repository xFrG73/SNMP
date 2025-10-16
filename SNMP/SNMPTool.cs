using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;

namespace SNMP
{
    public partial class SNMPTool : Form
    {
        private Dictionary<string, string> predefinedOids = new Dictionary<string, string>
        {
            { "Description du système", "1.3.6.1.2.1.1.1.0" },
            { "Nom de l'ordinateur", "1.3.6.1.2.1.1.5.0" },
            { "Temps de fonctionnement", "1.3.6.1.2.1.1.3.0" },
            { "Contact", "1.3.6.1.2.1.1.4.0" },
            { "Localisation", "1.3.6.1.2.1.1.6.0" },
            { "──── MÉMOIRE ────", "" },
            { "Mémoire physique totale", "1.3.6.1.2.1.25.2.2.0" },
            { "──── DISQUES ────", "" },
            { "Disque C: - Taille totale", "1.3.6.1.2.1.25.2.3.1.5.1" },
            { "Disque C: - Espace utilisé", "1.3.6.1.2.1.25.2.3.1.6.1" },
            { "Disque C: - Type système fichiers", "1.3.6.1.2.1.25.2.3.1.3.1" },
            { "Disque C: - Unité allocation", "1.3.6.1.2.1.25.2.3.1.4.1" },
            { "──── RÉSEAU ────", "" },
            { "Nombre d'interfaces réseau", "1.3.6.1.2.1.2.1.0" },
            { "Interface 1 - Description", "1.3.6.1.2.1.2.2.1.2.1" },
            { "Interface 1 - Vitesse", "1.3.6.1.2.1.2.2.1.5.1" },
            { "Interface 1 - Statut", "1.3.6.1.2.1.2.2.1.8.1" },
            { "Interface 1 - Octets reçus", "1.3.6.1.2.1.2.2.1.10.1" },
            { "Interface 1 - Octets envoyés", "1.3.6.1.2.1.2.2.1.16.1" },
            { "──── SERVICES ────", "" },
            { "Nombre de processus", "1.3.6.1.2.1.25.1.6.0" },
        };

        public SNMPTool()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            comboBoxOIDs.Items.Clear();
            comboBoxOIDs.Items.Add("Sélectionner un OID prédéfini");
            foreach (var oid in predefinedOids)
            {
                comboBoxOIDs.Items.Add(oid.Key);
            }
            comboBoxOIDs.SelectedIndex = 0;

            listViewResults.View = View.Details;
            listViewResults.FullRowSelect = true;
            listViewResults.GridLines = true;

            comboBoxOIDs.SelectedIndexChanged += SelectOid_SelectedIndexChanged;
        }

        private void SelectOid_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxOIDs.SelectedIndex > 0)
            {
                string selectedKey = comboBoxOIDs.SelectedItem?.ToString() ?? "";
                if (predefinedOids.ContainsKey(selectedKey) && !string.IsNullOrEmpty(predefinedOids[selectedKey]))
                {
                    textBoxOID.Text = predefinedOids[selectedKey];
                }
                else if (selectedKey.Contains("────"))
                {
                }
            }
        }


        private async Task PerformSnmpGet()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(textBoxIP.Text), 161);
            var community = new OctetString("public");
            var objectId = new ObjectIdentifier(textBoxOID.Text);

            var result = await Messenger.GetAsync(VersionCode.V2, endpoint, community, new List<Variable> { new Variable(objectId) });

            if (result.Count > 0)
            {
                var variable = result[0];
                string resultValue = variable.Data.ToString();

                if (resultValue.Contains("NoSuchObject") || resultValue.Contains("noSuchObject"))
                {
                    resultValue = "OID non supporté par cet équipement";
                }
                else if (resultValue.Contains("NoSuchInstance") || resultValue.Contains("noSuchInstance"))
                {
                    resultValue = "Instance inexistante";
                }
                else if (resultValue.Contains("EndOfMibView") || resultValue.Contains("endOfMibView"))
                {
                    resultValue = "Fin de la table MIB";
                }

                AddResultToList(variable.Id.ToString(), resultValue);
            }
            else
            {
                AddResultToList(textBoxOID.Text, "Aucune réponse");
            }
        }

        private void AddResultToList(string oid, string result)
        {
            var item = new ListViewItem((listViewResults.Items.Count + 1).ToString());
            item.SubItems.Add(oid);
            item.SubItems.Add(result);
            listViewResults.Items.Add(item);
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxIP.Text) || string.IsNullOrWhiteSpace(textBoxOID.Text))
            {
                MessageBox.Show("Veuillez saisir l'IP et l'OID", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            buttonStart.Enabled = false;
            buttonStart.Text = "En cours...";

            try
            {
                await PerformSnmpGet();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur SNMP: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonStart.Enabled = true;
                buttonStart.Text = "Démarrer";
            }
        }

        private void buttonScan_Click_1(object sender, EventArgs e)
        {
            using var scannerForm = new NetworkScannerForm();
            if (scannerForm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(scannerForm.SelectedIP))
            {
                textBoxIP.Text = scannerForm.SelectedIP;
            }
        }
    }
}