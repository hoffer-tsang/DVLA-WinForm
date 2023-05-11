/*==============================================================================
 *
 * Car Screen Class
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityFrameWorkModel;
using AddtionalModelsOrBusinessClass.Task_7.CarScreen;

namespace Task_7
{
    /// <summary>
    /// Car Screen Class
    /// </summary>
    public partial class CarScreen : Form
    {
        private string _RegistrationNumberSearch;
        private object _MakeSearch;
        private object _ModelSearch;
        private string _FirstNameSearch;
        private string _LastNameSearch;
        private bool _IsModelChanged = false;
        private int _ColumnIndex = 0;
        /// <summary>
        /// Constructor of Car Screen that initalize the component and refresh 
        /// the datagrid
        /// </summary>
        public CarScreen()
        {
            InitializeComponent();
            GenerateComboBoxOption carSearchDisplay = new GenerateComboBoxOption();
            List<string> models = carSearchDisplay.AvaliableModel();
            List<string> makes = carSearchDisplay.AvaliableMake();
            this.modelComboBox.DataSource = models;
            this.modelComboBox.SelectedItem = null;
            this.makeComboBox.DataSource = makes;
            this.makeComboBox.SelectedItem = null;
            this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            this.makeComboBox.SelectedIndexChanged += new System.EventHandler(this.MakeComboBox_SelectedIndexChanged);
            var displayList = new CarSearchDisplayList();
            List<int> pageNumberList = displayList.TotalCarPageNumber(null, null,
                null, null, null);
            this.pageNumberComboBox.SelectedIndexChanged -= new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
            RefreshGrid();
        }
        /// <summary>
        /// The New Car Button to generate a new car details screen
        /// </summary>
        /// <param name="sender"> new car button </param>
        /// <param name="e"> click event </param>

        private void NewCarButton_Click(object sender, EventArgs e)
        {
            using (var detailsWindow = new CarDetailsScreen())
            {
                detailsWindow.CarSaved += this.DetailsWindow_CarSaved;
                detailsWindow.ShowDialog();
            }
        }
        /// <summary>
        /// Show Details in car details screen of the selected row in data grid
        /// </summary>
        /// <param name="sender"> data grid view row </param>
        /// <param name="e"> click event </param>
        private void CarDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var carSearch = carDataGridView.Rows[e.RowIndex].DataBoundItem as
                    AddtionalModelsOrBusinessClass.Task_7.CarScreen.CarSearchDisplayList;
                var carSearchDisplayList = new CarModelDetails();
                CarModelDetails car = carSearchDisplayList.CarDetailsToDisplay(carSearch.RegistrationNumber);
                using (var detailsWindow = new CarDetailsScreen(car))
                {
                    detailsWindow.CarSaved += this.DetailsWindow_CarSaved;
                    detailsWindow.ShowDialog();
                }
            }
            else
            {
                _ColumnIndex = e.ColumnIndex; 
                var carSearchDisplayList = new CarSearchDisplayList();
                var carDisplayList = carSearchDisplayList.SearchCarList(
                    _RegistrationNumberSearch, _ModelSearch, _MakeSearch,
                    _FirstNameSearch, _LastNameSearch, 1, _ColumnIndex);
                this.pageNumberComboBox.SelectedIndex = 0;
                this.carDataGridView.DataSource = carDisplayList;
            }
        }
        /// <summary>
        /// Search the car with details in sesarch list and populate in data grid view
        /// </summary>
        /// <param name="sender"> search button </param>
        /// <param name="e"> click event </param>
        private void CarSearchButton_Click(object sender, EventArgs e)
        {
            if (InputCheck() == false)
            {
                return;
            }
            _RegistrationNumberSearch = this.registrationNumberTextBox.Text;
            _ModelSearch = this.modelComboBox.SelectedItem;
            _MakeSearch = this.makeComboBox.SelectedItem;
            _FirstNameSearch = this.firstNameTextBox.Text;
            _LastNameSearch = this.lastNameTextBox.Text;
            var carSearchDisplayList = new CarSearchDisplayList();
            var carDisplayList = carSearchDisplayList.SearchCarList(
                _RegistrationNumberSearch, _ModelSearch, _MakeSearch,
                _FirstNameSearch, _LastNameSearch, 1, 0);
            this.carDataGridView.DataSource = carDisplayList;
            _ColumnIndex = 0;
            var generate = new GenerateComboBoxOption();
            this.modelComboBox.DataSource = generate.AvaliableModel();
            this.modelComboBox.SelectedItem = null;
            this.makeComboBox.SelectedItem = null;
            _IsModelChanged = false;
            List<int> pageNumberList = carSearchDisplayList.TotalCarPageNumber(
                    _RegistrationNumberSearch, _ModelSearch, _MakeSearch,
                    _FirstNameSearch, _LastNameSearch);
            this.pageNumberComboBox.SelectedIndexChanged -= new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// Populate all car details in grid view 
        /// </summary>
        private void RefreshGrid()
        {
            var searchDisplayList = new CarSearchDisplayList();
            var car = searchDisplayList.SearchCarList(
                null, null, null, null, null, 1, 0);
            this.carDataGridView.DataSource = car;
            _ColumnIndex = 0;
            List<int> pageNumberList = searchDisplayList.TotalCarPageNumber(
                    null, null, null, null, null);
            this.pageNumberComboBox.SelectedIndexChanged -= new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.PageNumberComboBox_SelectedIndexChanged);
            _RegistrationNumberSearch = null;
            _ModelSearch = null;
            _MakeSearch = null;
            _FirstNameSearch = null;
            _LastNameSearch = null;
        }
        /// <summary>
        /// Check the input in search field is valid or not
        /// </summary>
        /// <returns> true or false base on validity </returns>
        private bool InputCheck()
        {
            if (this.registrationNumberTextBox.Text.Length > 7 ||
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
                return false;
            }
            return true;
        }
        /// <summary>
        /// Refreshes the grid when a car is saved on a child car details window.
        /// </summary>
        private void DetailsWindow_CarSaved(object sender, EventArgs e)
        {
            this.RefreshGrid();
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
                var carSearchDisplayList = new CarSearchDisplayList();
                var carDisplayList = carSearchDisplayList.SearchCarList(
                    _RegistrationNumberSearch, _ModelSearch, _MakeSearch,
                    _FirstNameSearch, _LastNameSearch,
                    (int)pageNumberComboBox.SelectedValue, _ColumnIndex);
                this.carDataGridView.DataSource = carDisplayList;
            }
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
        /// Button navigate back to main menu
        /// </summary>
        /// <param name="sender"> back button </param>
        /// <param name="e"> button click </param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            //var mainScreen = Application.OpenForms["MainMenu"];
            //mainScreen.Show();
        }
    }
}
