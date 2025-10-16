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
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                using var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                    System.Net.Sockets.SocketType.Dgram, 0);
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint?.Address.ToString() ?? "";
            }
            catch
            {
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
            catch (Exception ex)
            {
                labelStatus.Text = $"Erreur de scan: {ex.Message}";
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
            var ips = GenerateIPRange(startIP, endIP);
            progressBarScan.Maximum = ips.Count;
            progressBarScan.Value = 0;

            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(20);

            foreach (var ip in ips)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                tasks.Add(ScanSingleIP(ip, semaphore));
            }

            await Task.WhenAll(tasks);

            this.Invoke(() =>
            {
                labelStatus.Text = $"Scan terminé - {listViewDevices.Items.Count} périphériques trouvés";
            });
        }

        private List<IPAddress> GenerateIPRange(IPAddress startIP, IPAddress endIP)
        {
            var ips = new List<IPAddress>();
            var startBytes = startIP.GetAddressBytes();
            var endBytes = endIP.GetAddressBytes();

            for (int a = startBytes[0]; a <= endBytes[0]; a++)
            {
                for (int b = startBytes[1]; b <= endBytes[1]; b++)
                {
                    for (int c = startBytes[2]; c <= endBytes[2]; c++)
                    {
                        for (int d = startBytes[3]; d <= endBytes[3]; d++)
                        {
                            ips.Add(new IPAddress(new byte[] { (byte)a, (byte)b, (byte)c, (byte)d }));
                        }
                    }
                }
            }
            return ips;
        }

        private async Task ScanSingleIP(IPAddress ip, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();

            try
            {
                this.Invoke(() =>
                {
                    progressBarScan.Value++;
                    if (progressBarScan.Value % 20 == 0)
                    {
                        labelStatus.Text = $"Scan {ip} ({progressBarScan.Value}/{progressBarScan.Maximum})";
                    }
                });

                var snmpResult = await TestSNMP(ip);
                if (!string.IsNullOrEmpty(snmpResult))
                {
                    this.Invoke(() =>
                    {
                        var item = new ListViewItem(ip.ToString());
                        item.SubItems.Add("Détecté");
                        item.SubItems.Add(snmpResult);
                        listViewDevices.Items.Add(item);
                    });
                }
            }
            catch
            {
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<string> TestSNMP(IPAddress ip)
        {
            try
            {
                var endpoint = new IPEndPoint(ip, 161);
                var community = new OctetString("public");
                var oid = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");

                using var cts = new CancellationTokenSource();
                cts.CancelAfter(1000);

                var result = await Messenger.GetAsync(VersionCode.V2, endpoint, community,
                    new List<Variable> { new Variable(oid) }, cts.Token);

                if (result.Count > 0)
                {
                    var description = result[0].Data.ToString();
                    if (!string.IsNullOrEmpty(description) && !description.Contains("NoSuch"))
                    {
                        return description.Length > 50 ? description.Substring(0, 50) + "..." : description;
                    }
                }
            }
            catch
            {
            }

            return "";
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

    }
}