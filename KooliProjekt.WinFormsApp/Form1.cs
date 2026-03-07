using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using KooliProjekt.WinFormsApp.API;
using KooliProjekt.WinFormsApp.Models;
using KooliProjekt.WinFormsApp.Presenters;
using KooliProjekt.WinFormsApp.Views;

namespace KooliProjekt.WinFormsApp;

/// <summary>
/// Form1 - implements ITeamView interface for MVP pattern
/// All business logic is in TeamPresenter
/// </summary>
public partial class Form1 : Form, ITeamView
{
    private readonly TeamPresenter _presenter;
    private List<Team> _teams = new List<Team>();
    private Team? _selectedTeam;

    #region ITeamView Implementation

    // Properties - exposed to Presenter
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<Team> Teams
    {
        get => _teams;
        set
        {
            _teams = value;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _teams;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Team? SelectedTeam
    {
        get => _selectedTeam;
        set
        {
            _selectedTeam = value;
            if (value != null)
            {
                TeamId = value.Id;
                TeamName = value.Name;
            }
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int TeamId
    {
        get => int.TryParse(idTextBox.Text, out var id) ? id : 0;
        set => idTextBox.Text = value.ToString();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string TeamName
    {
        get => nameTextBox.Text;
        set => nameTextBox.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string StatusMessage
    {
        get => statusLabel.Text;
        set => statusLabel.Text = value;
    }

    // Events - raised by user actions
    public event EventHandler? LoadRequested;
    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? DeleteRequested;

    // Methods - called by Presenter
    public void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowSuccess(string message)
    {
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ClearForm()
    {
        idTextBox.Text = string.Empty;
        nameTextBox.Text = string.Empty;
        _selectedTeam = null;
    }

    #endregion

    public Form1()
    {
        InitializeComponent();

        // Create Presenter
        var apiClient = new ApiClient();
        _presenter = new TeamPresenter(this, apiClient);
    }

    // Form load - trigger LoadRequested event
    private void Form1_Load(object sender, EventArgs e)
    {
        LoadRequested?.Invoke(this, EventArgs.Empty);
    }

    // DataGridView selection changed
    private void DataGridView1_SelectionChanged(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Team team)
        {
            SelectedTeam = team;
            StatusMessage = $"Selected: {team.Name}";
        }
    }

    // NEW BUTTON: Trigger NewRequested event (Presenter handles logic)
    private void NewButton_Click(object sender, EventArgs e)
    {
        NewRequested?.Invoke(this, EventArgs.Empty);
    }

    // SAVE BUTTON: Trigger SaveRequested event (Presenter handles logic)
    private void SaveButton_Click(object sender, EventArgs e)
    {
        SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    // DELETE BUTTON: Ask confirmation, then trigger DeleteRequested event
    private void DeleteButton_Click(object sender, EventArgs e)
    {
        if (_selectedTeam == null || _selectedTeam.Id == 0)
        {
            ShowError("Please select a team from the list!");
            return;
        }

        // Ask for confirmation (View responsibility)
        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete team '{_selectedTeam.Name}'?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes)
            return;

        // Trigger event - Presenter handles the actual deletion
        DeleteRequested?.Invoke(this, EventArgs.Empty);
    }

    // Refresh button - trigger LoadRequested event
    private void RefreshButton_Click(object sender, EventArgs e)
    {
        LoadRequested?.Invoke(this, EventArgs.Empty);
    }
}
