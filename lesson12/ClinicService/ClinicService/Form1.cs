using ClinicService.Models;
using ClinicService.Models.Requests;
using System.Drawing;
using System.Reflection.Emit;

namespace ClinicService
{
    public partial class Form1 : Form
    {
        private ClinicClient _clinicClient;

        public Form1()
        {
            InitializeComponent();
            InitializeClient();
        }

        private void InitializeClient()
        {
            _clinicClient = new ClinicClient("http://localhost:5076/", new HttpClient());
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var clients = await _clinicClient.GetAllClientsAsync();

                listViewClients.Items.Clear();

                foreach (var client in clients)
                {
                    ListViewItem item = new ListViewItem(client.ClientId.ToString());
                    item.SubItems.Add(client.SurName ?? "");
                    item.SubItems.Add(client.FirstName ?? "");
                    item.SubItems.Add(client.Patronymic ?? "");
                    item.SubItems.Add(client.Birthday.ToShortDateString());
                    item.SubItems.Add(client.Document ?? "");

                    item.Tag = client;

                    listViewClients.Items.Add(item);
                }

                if (listViewClients.Columns.Count == 0)
                {
                    listViewClients.Columns.Add("ID", 50);
                    listViewClients.Columns.Add("Фамилия", 100);
                    listViewClients.Columns.Add("Имя", 100);
                    listViewClients.Columns.Add("Отчество", 100);
                    listViewClients.Columns.Add("Дата рождения", 100);
                    listViewClients.Columns.Add("Документ", 150);
                }

                statusStrip1.Items[0].Text = $"Загружено {clients.Count} клиентов";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var createForm = new CreateClientForm();
                if (createForm.ShowDialog() == DialogResult.OK)
                {
                    var request = createForm.GetCreateRequest();
                    var result = await _clinicClient.CreateClientAsync(request);

                    if (result > 0)
                    {
                        MessageBox.Show("Клиент успешно создан!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshClientsList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdateClient_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedClient = (Client)listViewClients.SelectedItems[0].Tag;

                var updateForm = new UpdateClientForm(selectedClient);
                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    var request = updateForm.GetUpdateRequest();
                    var result = await _clinicClient.UpdateClientAsync(request);

                    if (result > 0)
                    {
                        MessageBox.Show("Клиент успешно обновлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshClientsList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDeleteClient_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedClient = (Client)listViewClients.SelectedItems[0].Tag;

            var result = MessageBox.Show($"Удалить клиента {selectedClient.SurName} {selectedClient.FirstName}?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var deleteResult = await _clinicClient.DeleteClientAsync(selectedClient.ClientId);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Клиент успешно удален!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshClientsList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task RefreshClientsList()
        {
            btnUpdate_Click(null, null);
        }
    }

    public partial class CreateClientForm : Form
    {
        private TextBox txtDocument;
        private TextBox txtSurName;
        private TextBox txtFirstName;
        private TextBox txtPatronymic;
        private DateTimePicker dtpBirthday;
        private Button btnOk;
        private Button btnCancel;

        public CreateClientForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Создание клиента";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10)
            };

            mainPanel.Controls.Add(new Label { Text = "Документ:", TextAlign = ContentAlignment.MiddleRight }, 0, 0);
            txtDocument = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtDocument, 1, 0);

            mainPanel.Controls.Add(new Label { Text = "Фамилия:", TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            txtSurName = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtSurName, 1, 1);

            mainPanel.Controls.Add(new Label { Text = "Имя:", TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            txtFirstName = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtFirstName, 1, 2);

            mainPanel.Controls.Add(new Label { Text = "Отчество:", TextAlign = ContentAlignment.MiddleRight }, 0, 3);
            txtPatronymic = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtPatronymic, 1, 3);

            mainPanel.Controls.Add(new Label { Text = "Дата рождения:", TextAlign = ContentAlignment.MiddleRight }, 0, 4);
            dtpBirthday = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
            mainPanel.Controls.Add(dtpBirthday, 1, 4);

            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel };
            buttonPanel.Controls.Add(btnOk);
            buttonPanel.Controls.Add(btnCancel);
            mainPanel.Controls.Add(buttonPanel, 1, 5);

            this.Controls.Add(mainPanel);
        }

        public CreateClientRequest GetCreateRequest()
        {
            return new CreateClientRequest
            {
                Document = txtDocument.Text,
                SurName = txtSurName.Text,
                FirstName = txtFirstName.Text,
                Patronymic = txtPatronymic.Text,
                Birthday = dtpBirthday.Value
            };
        }
    }

    public partial class UpdateClientForm : Form
    {
        private TextBox txtDocument;
        private TextBox txtSurName;
        private TextBox txtFirstName;
        private TextBox txtPatronymic;
        private DateTimePicker dtpBirthday;
        private Button btnOk;
        private Button btnCancel;
        private int _clientId;

        public UpdateClientForm(Client client)
        {
            _clientId = client.ClientId;
            InitializeComponents();
            LoadClientData(client);
        }

        private void InitializeComponents()
        {
            this.Text = "Редактирование клиента";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10)
            };

            mainPanel.Controls.Add(new Label { Text = "Документ:", TextAlign = ContentAlignment.MiddleRight }, 0, 0);
            txtDocument = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtDocument, 1, 0);

            mainPanel.Controls.Add(new Label { Text = "Фамилия:", TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            txtSurName = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtSurName, 1, 1);

            mainPanel.Controls.Add(new Label { Text = "Имя:", TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            txtFirstName = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtFirstName, 1, 2);

            mainPanel.Controls.Add(new Label { Text = "Отчество:", TextAlign = ContentAlignment.MiddleRight }, 0, 3);
            txtPatronymic = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(txtPatronymic, 1, 3);

            mainPanel.Controls.Add(new Label { Text = "Дата рождения:", TextAlign = ContentAlignment.MiddleRight }, 0, 4);
            dtpBirthday = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
            mainPanel.Controls.Add(dtpBirthday, 1, 4);

            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel };
            buttonPanel.Controls.Add(btnOk);
            buttonPanel.Controls.Add(btnCancel);
            mainPanel.Controls.Add(buttonPanel, 1, 5);

            this.Controls.Add(mainPanel);
        }

        private void LoadClientData(Client client)
        {
            txtDocument.Text = client.Document;
            txtSurName.Text = client.SurName;
            txtFirstName.Text = client.FirstName;
            txtPatronymic.Text = client.Patronymic;
            dtpBirthday.Value = client.Birthday;
        }

        public UpdateClientRequest GetUpdateRequest()
        {
            return new UpdateClientRequest
            {
                ClientId = _clientId,
                Document = txtDocument.Text,
                SurName = txtSurName.Text,
                FirstName = txtFirstName.Text,
                Patronymic = txtPatronymic.Text,
                Birthday = dtpBirthday.Value
            };
        }
    }
}