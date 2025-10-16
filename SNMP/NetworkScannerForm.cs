using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System.Net.NetworkInformation;

namespace SNMP
{
    public partial class NetworkScannerForm : Form
    {
        public string? SelectedIP { get; private set; }
        private CancellationTokenSource? _cancellationTokenSource;

        public NetworkScannerForm()
        {
            InitializeComponent();
            InitializeEvents();
            DetectLocalNetwork();
        }

        private void InitializeEvents()
        {
            buttonStartScan.Click += ButtonStartScan_Click;
            buttonSelect.Click += ButtonSelect_Click;
            buttonCancel.Click += ButtonCancel_Click;
            listViewDevices.SelectedIndexChanged += ListViewDevices_SelectedIndexChanged;
        }

        private void DetectLocalNetwork()
        {
            try
            {
                var localIP = GetLocalIPAddress();
                if (!string.IsNullOrEmpty(localIP))
                {
                    var parts = localIP.Split('.');
                    if (parts.Length == 4)
                    {
                        var networkBase = $"{parts[0]}.{parts[1]}.{parts[2]}";
                        textBoxRange.Text = $"{networkBase}.1-{networkBase}.254";
                    }
                }
            }
            catch
            {
                // Garder la valeur par défaut si erreur
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                // Méthode plus fiable pour obtenir l'IP locale
                using var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                    System.Net.Sockets.SocketType.Dgram, 0);
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint?.Address.ToString() ?? "";
            }
            catch
            {
                // Fallback vers l'ancienne méthode
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                    {
                        return ip.ToString();
                    }
                }
                return "";
            }
        }

        private async void ButtonStartScan_Click(object? sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                return;
            }

            var range = textBoxRange.Text.Trim();
            if (string.IsNullOrEmpty(range) || !range.Contains('-'))
            {
                MessageBox.Show("Format invalide. Utilisez: 192.168.1.1-192.168.1.254", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var parts = range.Split('-');
            if (parts.Length != 2)
            {
                MessageBox.Show("Format invalide. Utilisez: 192.168.1.1-192.168.1.254", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IPAddress.TryParse(parts[0].Trim(), out var startIP) || !IPAddress.TryParse(parts[1].Trim(), out var endIP))
            {
                MessageBox.Show("Adresses IP invalides.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            buttonStartScan.Text = "Arrêter";
            buttonSelect.Enabled = false;
            listViewDevices.Items.Clear();

            try
            {
                await ScanNetwork(startIP, endIP, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                labelStatus.Text = "Scan annulé";
            }
            finally
            {
                _cancellationTokenSource = null;
                buttonStartScan.Text = "Scanner";
                progressBarScan.Value = 0;
            }
        }

        private async Task ScanNetwork(IPAddress startIP, IPAddress endIP, CancellationToken cancellationToken)
        {
            var startBytes = startIP.GetAddressBytes();
            var endBytes = endIP.GetAddressBytes();

            var startInt = BitConverter.ToInt32(startBytes.Reverse().ToArray(), 0);
            var endInt = BitConverter.ToInt32(endBytes.Reverse().ToArray(), 0);

            var totalIPs = endInt - startInt + 1;
            progressBarScan.Maximum = totalIPs;
            progressBarScan.Value = 0;

            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(10); // 10 scans simultanés

            for (int i = startInt; i <= endInt; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var ipBytes = BitConverter.GetBytes(i).Reverse().ToArray();
                var currentIP = new IPAddress(ipBytes);

                tasks.Add(ScanSingleIP(currentIP, semaphore, cancellationToken));

                // Petite pause pour éviter la surcharge
                if (tasks.Count % 50 == 0)
                {
                    await Task.Delay(10, cancellationToken);
                }
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                this.Invoke(() =>
                {
                    labelStatus.Text = $"Erreur lors du scan: {ex.Message}";
                });
            }
            finally
            {
                this.Invoke(() =>
                {
                    labelStatus.Text = $"Scan terminé - {listViewDevices.Items.Count} périphériques SNMP trouvés";
                });
            }
        }

        private async Task ScanSingleIP(IPAddress ip, SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);

            try
            {
                // Éviter de surcharger l'UI
                if (progressBarScan.Value % 10 == 0)
                {
                    this.Invoke(() =>
                    {
                        labelStatus.Text = $"Scan de {ip}...";
                    });
                }

                this.Invoke(() =>
                {
                    progressBarScan.Value++;
                });

                // Test SNMP directement (certains périphériques ne répondent pas au ping)
                var snmpResult = await TestSNMP(ip, cancellationToken);
                if (!string.IsNullOrEmpty(snmpResult))
                {
                    var hostname = await GetHostname(ip);

                    this.Invoke(() =>
                    {
                        var item = new ListViewItem(ip.ToString());
                        item.SubItems.Add(hostname);
                        item.SubItems.Add(snmpResult);
                        listViewDevices.Items.Add(item);
                    });
                }
            }
            catch (OperationCanceledException)
            {
                // Scan annulé
                throw;
            }
            catch (Exception)
            {
                // Ignorer les autres erreurs pour continuer le scan
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<string> TestSNMP(IPAddress ip, CancellationToken cancellationToken)
        {
            try
            {
                var endpoint = new IPEndPoint(ip, 161);
                var community = new OctetString("public");
                var oid = new ObjectIdentifier("1.3.6.1.2.1.1.1.0"); // System description

                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(1500); // Timeout réduit à 1.5 secondes

                var result = await Messenger.GetAsync(VersionCode.V2, endpoint, community,
                    new List<Variable> { new Variable(oid) }, cts.Token);

                if (result.Count > 0)
                {
                    var description = result[0].Data.ToString();

                    // Vérifier si c'est une vraie réponse SNMP
                    if (!string.IsNullOrEmpty(description) &&
                        !description.Contains("NoSuchObject") &&
                        !description.Contains("noSuchObject"))
                    {
                        return description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Timeout ou annulation
                return "";
            }
            catch (Exception)
            {
                // Autres erreurs SNMP
                return "";
            }

            return "";
        }

        private async Task<string> GetHostname(IPAddress ip)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                var hostEntry = await Dns.GetHostEntryAsync(ip).WaitAsync(cts.Token);
                return hostEntry.HostName.Split('.')[0]; // Prendre seulement le nom court
            }
            catch
            {
                return "Inconnu";
            }
        }

        private void ListViewDevices_SelectedIndexChanged(object? sender, EventArgs e)
        {
            buttonSelect.Enabled = listViewDevices.SelectedItems.Count > 0;
        }

        private void ButtonSelect_Click(object? sender, EventArgs e)
        {
            if (listViewDevices.SelectedItems.Count > 0)
            {
                SelectedIP = listViewDevices.SelectedItems[0].Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ButtonCancel_Click(object? sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonStartScan_Click_1(object sender, EventArgs e)
        {

        }
    }
}