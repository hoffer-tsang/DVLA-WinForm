 /*==============================================================================
 *
 * Car Details Screen Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AddtionalModelsOrBusinessClass.Data;
using AddtionalModelsOrBusinessClass.Task_7.Sightings;
using AddtionalModelsOrBusinessClass.Task_7.CarScreen;
using EntityFrameWorkModel;

namespace Task_7
{
    /// <summary>
    /// Car Details Screen Class
    /// </summary>
    public partial class CarDetailsScreen : Form
    {
        /// <summary>
        /// Occurs when a car has been saved.
        /// </summary>
        public event EventHandler CarSaved;
        private CarModelDetails _Car;
        private bool _InputNewCarOrNot = false;
        private bool _IsModelChanged = false;
        private int _ColumnIndex = 0;
        /// <summary>
        /// Car Details Constructor 
        /// </summary>
        public CarDetailsScreen()
        {
            InitializeComponent();
            GenerateComboBoxOption carDetailDisplay = new GenerateComboBoxOption();
            List<string> models = carDetailDisplay.AvaliableModel();
            List<string> makes = carDetailDisplay.AvaliableMake();
            List<string> colours = carDetailDisplay.AvaliableColour();
            this.modelComboBox.DataSource = models;
            this.makeComboBox.DataSource = makes;
            this.colourComboBox.DataSource = colours;
            this.modelComboBox.SelectedItem = null;
            this.colourComboBox.SelectedItem = null;
            this.makeComboBox.SelectedItem = null;
            this.makeComboBox.SelectedIndexChanged += new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            _InputNewCarOrNot = true;
            this.registrationNumberTextBox.ReadOnly = false;
            this.modelComboBox.Enabled = true;
            this.makeComboBox.Enabled = true;
            this.colourComboBox.Enabled = true;
            this.registrationDateTimePicker.Enabled = true;
            this.firstNameTextBox.ReadOnly = false;
            this.lastNameTextBox.ReadOnly = false;
            this.editButton.Enabled = false;
            this.saveButton.Enabled = true;
            this.cancelButton.Enabled = true;
        }
        /// <summary>
        /// Over ride Car Details Construcotr to construct the form with car details
        /// </summary>
        /// <param name="car"> Car Model in Task 2 with details </param>
        public CarDetailsScreen(CarModelDetails car)
        {
            InitializeComponent();
            GenerateComboBoxOption carDetailDisplay = new GenerateComboBoxOption();
            List<string> models = carDetailDisplay.AvaliableModel();
            List<string> makes = carDetailDisplay.AvaliableMake();
            List<string> colours = carDetailDisplay.AvaliableColour();
            this.modelComboBox.DataSource = models;
            this.makeComboBox.DataSource = makes;
            this.colourComboBox.DataSource = colours;
            _Car = car;
            this.registrationNumberTextBox.Text = car.RegistrationNumber;
            this.modelComboBox.SelectedItem = car.Model;
            this.colourComboBox.SelectedItem = car.Colour;
            this.makeComboBox.SelectedItem = car.Make;
            this.registrationDateTimePicker.Value = car.RegistrationDate;
            this.firstNameTextBox.Text = car.OwnerFirstName;
            this.lastNameTextBox.Text = car.OwnerLastName;
            var generateSightings = new SightingsListDisplay();
            this.sightingsDataGridView.DataSource = generateSightings.GetSightingsList(_Car.CarId, 1, true, 0);
            this.pageNumberComboBox.DataSource = generateSightings.TotalSightingsPageNumber(_Car.CarId, true);
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// get the other page selected to display in data grid view
        /// </summary>
        /// <param name="sender"> selected combo box </param>
        /// <param name="e"> clicked item in combo box </param>
        private void PageNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pageNumber = this.pageNumberComboBox.SelectedItem;
            if (pageNumber != null)
            {
                var generateSightings = new SightingsListDisplay();
                var sightingList = generateSightings.GetSightingsList(_Car.CarId, (int)pageNumber, true, _ColumnIndex);
                this.sightingsDataGridView.DataSource = sightingList;
            }
        }
        /// <summary>
        /// Cancel all field input when editing
        /// </summary>
        /// <param name="sender"> cancel button </param>
        /// <param name="e"> button click </param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.registrationNumberTextBox.Text = null;
            this.colourComboBox.SelectedItem = null;
            this.firstNameTextBox.Text = null;
            this.lastNameTextBox.Text = null;
            this.registrationDateTimePicker.Value = DateTime.Now;
            this.makeComboBox.SelectedIndexChanged -= new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            this.modelComboBox.SelectedIndexChanged -= new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            GenerateComboBoxOption carDetailDisplay = new GenerateComboBoxOption();
            List<string> models = carDetailDisplay.AvaliableModel();
            List<string> makes = carDetailDisplay.AvaliableMake();
            _IsModelChanged = false;
            this.modelComboBox.DataSource = models;
            this.makeComboBox.DataSource = makes;
            this.modelComboBox.SelectedItem = null;
            this.makeComboBox.SelectedItem = null;
            this.makeComboBox.SelectedIndexChanged += new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// Enable editing in details page 
        /// </summary>
        /// <param name="sender"> edit button </param>
        /// <param name="e"> button click </param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            this.editButton.Enabled = false;
            this.saveButton.Enabled = true;
            this.cancelButton.Enabled = true;
            this.registrationNumberTextBox.ReadOnly = false;
            this.modelComboBox.Enabled = true;
            this.makeComboBox.Enabled = true;
            this.colourComboBox.Enabled = true;
            this.firstNameTextBox.ReadOnly = false;
            this.lastNameTextBox.ReadOnly = false;
            this.registrationDateTimePicker.Enabled = true;
            this.makeComboBox.SelectedIndexChanged += new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// Save the edit or new entry of car
        /// </summary>
        /// <param name="sender"> save btuton </param>
        /// <param name="e"> button click </param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (InputCheck() == true)
            {
                if (_InputNewCarOrNot == true)
                {
                    var dummycar = new CarModelDetails { CarId = -1 };
                    _Car = dummycar;
                }
                var updateDetails = new CarModelDetails();
                int saveStatus = updateDetails.SaveCarDetails(_InputNewCarOrNot,
                    this.registrationNumberTextBox.Text,
                    (string)this.modelComboBox.SelectedItem,
                    (string)this.colourComboBox.SelectedItem,
                    this.registrationDateTimePicker.Value,
                    this.firstNameTextBox.Text,
                    this.lastNameTextBox.Text,
                    _Car.CarId);
                if (saveStatus == -1)
                {
                    var result = MessageBox.Show("Owner name does not exist in the database. Please Add Owner!", "Error", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        using (var detailsWindow = new OwnerDetailsScreen(
                            this.firstNameTextBox.Text, this.lastNameTextBox.Text))
                        {
                            detailsWindow.ShowDialog();
                        }
                    }
                }
                else
                {
                    this.OnCarSaved();
                    this.Close();
                }
            }
        }
        /// <summary>
        /// Raises the <see cref="CarSaved"/> event.
        /// </summary>
        protected virtual void OnCarSaved()
        {
            var handler = this.CarSaved;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Check the input in search field is valid or not
        /// </summary>
        /// <returns> true or false base on validity </returns>
        private bool InputCheck()
        {
            if (this.registrationNumberTextBox.Text == "" ||
                this.colourComboBox.SelectedItem == null ||
                this.modelComboBox.SelectedItem == null ||
                this.makeComboBox.SelectedItem == null ||
                this.firstNameTextBox.Text == "" ||
                this.lastNameTextBox.Text == "")
            {
                MessageBox.Show("All Fields have to be filled in!", "Error");
                return false;
            }
            else if (this.registrationNumberTextBox.Text.Length > 7 ||
               !this.registrationNumberTextBox.Text.All(Char.IsLetterOrDigit))
            {
                MessageBox.Show("Registration number input is not in the correct format!", "Error");
                return false;
            }
            else if (this.firstNameTextBox.Text.Length > 40 ||
               !this.firstNameTextBox.Text.All(Char.IsLetter))
            {
                MessageBox.Show("First name input is not in the correct format!", "Error");
                return false;
            }
            else if (this.lastNameTextBox.Text.Length > 40 ||
               !this.lastNameTextBox.Text.All(Char.IsLetter))
            {
                MessageBox.Show("Last name input is not in the correct format!", "Error");
            }
            else if (this.registrationDateTimePicker.Value > DateTime.Now)
            {
                MessageBox.Show("Registration Date has to be in the past!", "Error");
                return false;
            }
            return true;
        }
        /// <summary>
        /// update avaliable model drop down for specific make 
        /// </summary>
        /// <param name="sender"> make drop box </param>
        /// <param name="e"> click item </param>
        private void MakeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.modelComboBox.SelectedIndexChanged -= new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);

            var makeSearch = this.makeComboBox.SelectedItem;
            var generate = new GenerateComboBoxOption();
            if (makeSearch != null)
            {
                _IsModelChanged = true;
                this.modelComboBox.DataSource = generate.ModelValueUpdate((string)makeSearch);
                this.modelComboBox.SelectedItem = null;
            }
            else
            {
                this.modelComboBox.DataSource = generate.AvaliableModel();
                this.modelComboBox.SelectedItem = null;
            }
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// update avalibale make drop down for specific model
        /// </summary>
        /// <param name="sender"> model drop box</param>
        /// <param name="e"> click item </param>
        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.makeComboBox.SelectedIndexChanged -= new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            var modelSearch = this.modelComboBox.SelectedItem;
            var generate = new GenerateComboBoxOption();
            if (modelSearch != null)
            {
                List<string> make = generate.MakeValueUpdate((string)modelSearch, _IsModelChanged);
                if (make != null)
                {
                    this.makeComboBox.DataSource = make;
                }
            }
            else
            {
                this.makeComboBox.DataSource = generate.AvaliableMake();
                this.makeComboBox.SelectedItem = null;
            }
            this.makeComboBox.SelectedIndexChanged += new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// when sighting grid column title is click, sort by the specific column
        /// </summary>
        /// <param name="sender"> sighting data grid view </param>
        /// <param name="e"> click event </param>
        private void SightingsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == -1)
            {
                _ColumnIndex = e.ColumnIndex;
                var generateSightings = new SightingsListDisplay();
                this.sightingsDataGridView.DataSource = generateSightings.GetSightingsList(_Car.CarId, 1, true, _ColumnIndex);
                this.pageNumberComboBox.SelectedIndex = 0;
            }
        }
    }
}
