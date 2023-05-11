/*==============================================================================
 *
 * Camera Screen Class
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
using AddtionalModelsOrBusinessClass.Task_7.CameraScreen;

namespace Task_7
{
    /// <summary>
    /// Camera Screen Class
    /// </summary>
    public partial class CameraScreen : Form
    {
        private string _LongitudeFrom;
        private string _LatitudeFrom;
        private string _LatitudeTo;
        private string _LongitudeTo;
        private int _ColumnIndex = 1;
        /// <summary>
        /// Constructor of Camera Screen that initalize the component, and refresh 
        /// the datagrid, and update pagenumber list
        /// </summary>
        public CameraScreen()
        {
            InitializeComponent();
            var displayList = new CameraSearchDisplayList();
            _ColumnIndex = 1;
            List<int> pageNumberList = displayList.TotalCameraPageNumber(null, null, null, null);
            this.pageNumberComboBox.SelectedIndexChanged -= PageNumberComboBox_SelectedIndexChanged;
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += PageNumberComboBox_SelectedIndexChanged;
            RefreshGrid();
        }
        /// <summary>
        /// The New speed Camera Button to generate a new Camera details screen
        /// </summary>
        /// <param name="sender"> new Camera button </param>
        /// <param name="e"> click event </param>
        private void NewSpeedCameraButton_Click(object sender, EventArgs e)
        {
            using (var detailsWindow = new CameraDetailsScreen(true))
            {
                detailsWindow.CameraSaved += this.DetailsWindow_CarSaved;
                detailsWindow.ShowDialog();
            }
        }
        /// <summary>
        /// The New traffic light Camera Button to generate a new Camera details screen
        /// </summary>
        /// <param name="sender"> new Camera button </param>
        /// <param name="e"> click event </param>
        private void NewTrafficLightCameraButton_Click(object sender, EventArgs e)
        {
            using (var detailsWindow = new CameraDetailsScreen(false))
            {
                detailsWindow.CameraSaved += this.DetailsWindow_CarSaved;
                detailsWindow.ShowDialog();
            }
        }
        /// <summary>
        /// Show Details in camera details screen of the selected row in data grid
        /// </summary>
        /// <param name="sender"> data grid view row </param>
        /// <param name="e"> click event </param>
        private void CameraDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var cameraSearch = cameraDataGridView.Rows[e.RowIndex].DataBoundItem as
                    AddtionalModelsOrBusinessClass.Task_7.CameraScreen.CameraSearchDisplayList;
                var cameraSearchDisplayList = new CameraModelDetails();
                CameraModelDetails camera = cameraSearchDisplayList.CameraDetailsToDisplay(cameraSearch);
                using (var detailsWindow = new CameraDetailsScreen(camera, cameraSearch.CameraType))
                {
                    detailsWindow.CameraSaved += this.DetailsWindow_CarSaved;
                    detailsWindow.ShowDialog();
                }              
            }
            else
            {
                _ColumnIndex = e.ColumnIndex;
                var cameraSearchDisplayList = new CameraSearchDisplayList();
                var cameraDisplayList = cameraSearchDisplayList.SearchCameraList(
                     _LongitudeFrom, _LongitudeTo, _LatitudeFrom, 
                     _LatitudeTo, 1, _ColumnIndex);
                this.cameraDataGridView.DataSource = cameraDisplayList;
                this.pageNumberComboBox.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Search the camera with details in sesarch list and populate in data grid view
        /// </summary>
        /// <param name="sender"> search button </param>
        /// <param name="e"> click event </param>
        private void CameraSearchButton_Click(object sender, EventArgs e)
        {
            if (InputCheck() == false)
            {
                return;
            }
            _LongitudeFrom = this.longitudeFromTextBox.Text;
            _LongitudeTo = this.longitudeToTextBox.Text;
            _LatitudeFrom = this.latitudeFromTextBox.Text;
            _LatitudeTo = this.latitudeToTextBox.Text;
            var cameraSearchDisplayList = new CameraSearchDisplayList();
            var cameraList = cameraSearchDisplayList.SearchCameraList(
                _LongitudeFrom,
                _LongitudeTo,
                _LatitudeFrom,
                _LatitudeTo, 1, 1);
            _ColumnIndex = 1;
            this.cameraDataGridView.DataSource = cameraList;
            List<int> pageNumberList = cameraSearchDisplayList.TotalCameraPageNumber(
                _LongitudeFrom,
                _LongitudeTo,
                _LatitudeFrom,
                _LatitudeTo);
            this.pageNumberComboBox.SelectedIndexChanged -= PageNumberComboBox_SelectedIndexChanged;
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += PageNumberComboBox_SelectedIndexChanged;
        }
        /// <summary>
        /// Populate all camera details in grid view 
        /// </summary>
        private void RefreshGrid()
        {
            var searchDisplayList = new CameraSearchDisplayList();
            var camera = searchDisplayList.SearchCameraList(null, null, null, null, 1, 1);
            _ColumnIndex = 1;
            this.cameraDataGridView.DataSource = camera;
            List<int> pageNumberList = searchDisplayList.TotalCameraPageNumber(null, null, null, null);
            this.pageNumberComboBox.SelectedIndexChanged -= PageNumberComboBox_SelectedIndexChanged;
            this.pageNumberComboBox.DataSource = pageNumberList;
            this.pageNumberComboBox.SelectedIndex = 0;
            this.pageNumberComboBox.SelectedIndexChanged += PageNumberComboBox_SelectedIndexChanged;
            _LongitudeFrom = null;
            _LongitudeTo = null;
            _LatitudeFrom = null;
            _LatitudeTo = null;
        }
        /// <summary>
        /// Check the input in search field is valid or not
        /// </summary>
        /// <returns> true or false base on validity </returns>
        private bool InputCheck()
        {
            decimal[] tryParseResult = new decimal[4];
            if (!string.IsNullOrWhiteSpace(this.longitudeFromTextBox.Text) &&
                (!decimal.TryParse(this.longitudeFromTextBox.Text, out tryParseResult[0])))
            {
                MessageBox.Show("Longitude From input is not a decimal!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.longitudeToTextBox.Text) &&
                (!decimal.TryParse(this.longitudeToTextBox.Text, out tryParseResult[1])))
            {
                MessageBox.Show("Longitude To input is not a decimal!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeFromTextBox.Text) &&
                (!decimal.TryParse(this.latitudeFromTextBox.Text, out tryParseResult[2])))
            {
                MessageBox.Show("Latitude From input is not a decimal!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeToTextBox.Text) &&
                (!decimal.TryParse(this.latitudeToTextBox.Text, out tryParseResult[3])))
            {
                MessageBox.Show("Latitude To input is not a decimal!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.longitudeFromTextBox.Text) &&
                this.longitudeFromTextBox.Text.Length > 8)
            {
                MessageBox.Show("Longitude From input need to be less than 8 digits!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.longitudeToTextBox.Text) &&
                this.longitudeToTextBox.Text.Length > 8)
            {
                MessageBox.Show("Longitude To input need to be less than 8 digits!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeFromTextBox.Text) &&
                this.latitudeFromTextBox.Text.Length > 8)
            {
                MessageBox.Show("Latitude From input need to be less than 8 digits!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeToTextBox.Text) &&
                this.latitudeToTextBox.Text.Length > 8)
            {
                MessageBox.Show("Latitude To input need to be less than 8 digits!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.longitudeFromTextBox.Text) &&
                ((int)(tryParseResult[0] * 1000000 % 10)) != 0)
            {
                MessageBox.Show("5 Decimal Maximum for longitude from input!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.longitudeToTextBox.Text) &&
                ((int)(tryParseResult[1] * 1000000 % 10)) != 0)
            {
                MessageBox.Show("5 Decimal Maximum for longitude to input!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeFromTextBox.Text) &&
                ((int)(tryParseResult[2] * 1000000 % 10)) != 0)
            {
                MessageBox.Show("5 Decimal Maximum for latitude from input!", "Error");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(this.latitudeToTextBox.Text) &&
                ((int)(tryParseResult[3] * 1000000 % 10)) != 0)
            {
                MessageBox.Show("5 Decimal Maximum for latitude to input!", "Error");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Refreshes the grid when a camera is saved on a child camera details window.
        /// </summary>
        private void DetailsWindow_CarSaved(object sender, EventArgs e)
        {
            this.RefreshGrid();
        }
        /// <summary>
        /// get the other page selected to display in data grid view
        /// </summary>
        /// <param name="sender"> selected page number drop box </param>
        /// <param name="e"> click drop box </param>
        private void PageNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pageNumber = this.pageNumberComboBox.SelectedItem;
            if (pageNumber != null)
            {
                var cameraSearchDisplayList = new CameraSearchDisplayList();
                var cameraList = cameraSearchDisplayList.SearchCameraList(
                    _LongitudeFrom,
                    _LongitudeTo,
                    _LatitudeFrom,
                    _LatitudeTo, (int)this.pageNumberComboBox.SelectedValue, _ColumnIndex);
                this.cameraDataGridView.DataSource = cameraList;
            }
        }
        /// <summary>
        /// Button navigate back to main menu
        /// </summary>
        /// <param name="sender"> back button </param>
        /// <param name="e"> button click </param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            var mainScreen = Application.OpenForms["MainMenu"];
            mainScreen.Show();
        }
    }
}
