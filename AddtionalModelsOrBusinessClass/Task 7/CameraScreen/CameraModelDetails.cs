/*==============================================================================
 *
 * Camera Details Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AddtionalModelsOrBusinessClass.Data;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.CameraScreen
{
    public class CameraModelDetails
    {
        public int CameraId { get; set; }
        public string RoadName { get; set; }
        public string RoadNumber { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public byte CameraType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public ICollection<Car> CarList { get; set; }

        /// <summary>
        /// Display Camera Detail screen
        /// </summary>
        /// <param name="displayList"> the click detial to display </param>
        /// <returns> the details for camera details screen </returns>
        public CameraModelDetails CameraDetailsToDisplay(CameraSearchDisplayList displayList)
        {
            using (var context = new DVLAEntities())
            {
                var camera = context.Cameras.Select(
                c => new CameraModelDetails
                {
                    CameraId = c.CameraId,
                    RoadName = c.RoadName,
                    RoadNumber = c.RoadNumber,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Line1 = c.Address.Line1,
                    Line2 = c.Address.Line2,
                    Line3 = c.Address.Line3,
                    City = c.Address.City,
                    County = c.Address.County,
                    Country = c.Address.Country,
                    PostCode = c.Address.PostalCode,
                }).Where(c => c.RoadName == displayList.RoadName &&
                c.Latitude == displayList.Latitude &&
                c.Longitude == displayList.Longitude).ToList()[0];
                var cameraType = context.SpeedCameras.Find(camera.CameraId);
                if (cameraType != null)
                {
                    camera.CameraType = cameraType.SpeedMphLimit;
                }
                else
                {
                    var cameraTraffic = context.TrafficLightCameras.Find(camera.CameraId);
                    camera.CameraType = cameraTraffic.SecondsAfterRedLightThreshold;
                }
                return camera;
            }
        }
        /// <summary>
        /// Save Camera Details on the screen 
        /// </summary>
        /// <param name="input"> boolean check new camera (true) or update camera(false) </param>
        /// <param name="roadName"> Road Name input </param>
        /// <param name="roadNumber"> Road Number input </param>
        /// <param name="longitudeString"> longitude input </param>
        /// <param name="latitudeString"> latitude input </param>
        /// <param name="cameraType"> the details of camera type
        /// either seconds after red light threshold or speed limit </param>
        /// <param name="addressEdit"> boolean of address edit checkbox </param>
        /// <param name="line1"> line1 of address</param>
        /// <param name="line2"> line2 of address </param> 
        /// <param name="line3"> line3 of address </param>
        /// <param name="city"> city of address </param>
        /// <param name="county"> county of address </param>
        /// <param name="country"> country of address </param>
        /// <param name="postcode"> postcode of address </param>
        /// <param name="cameraId"> cameraId to be save for update </param>
        /// <param name="cameraTypeString"> camera type of the camera </param>
        /// <returns> integer 1 for success save, -1 for non exisiting address </returns>
        public int SaveCameraDetails(bool input, string roadName, string roadNumber,
            string longitudeString, string latitudeString, string cameraType, bool addressEdit,
            string line1, string line2, string line3, string city, string county,
            string country, string postcode, int cameraId, string cameraTypeString)
        {
            int cameraIdCopy = cameraId;
            decimal longitude = decimal.Parse(longitudeString);
            decimal latitude = decimal.Parse(latitudeString);

            bool isSpeedCamera;
            int addressId = -1;
            if(addressEdit == true)
            {
                addressId = GetAddressId(line1, line2,
                line3, city, county, country, postcode);
                if (addressId == -1)
                {
                    return -1;
                }
            }
            if(cameraTypeString == "Speed Limit (mph):")
            {
                isSpeedCamera = true;
            }
            else
            {
                isSpeedCamera = false;
            }
            using (var context = new DVLAEntities())
            {
                var camera = new EntityFrameWorkModel.Camera();
                if (input == false)
                {
                    camera = context.Cameras.Find(cameraId);
                }
                else
                {
                    camera = new EntityFrameWorkModel.Camera();
                    context.Cameras.Add(camera);
                }
                camera.RoadName = roadName;
                camera.Longitude = longitude;
                camera.Latitude = latitude;
                if(addressId != -1)
                {
                    camera.AddressId = addressId;
                }
                if (!string.IsNullOrWhiteSpace(roadNumber))
                {
                    camera.RoadNumber = roadNumber;
                }
                context.SaveChanges();
            }
            if(input == true)
            {
                using(var context = new DVLAEntities())
                {
                    var camera = context.Cameras.Select(
                        c => new AddtionalModelsOrBusinessClass.Data.Camera
                        {
                            CameraId = c.CameraId,
                            RoadName = c.RoadName,
                            Longitude = c.Longitude,
                            Latitude = c.Latitude,
                        }).Where(c => c.RoadName == roadName 
                        && c.Longitude == longitude
                        && c.Latitude == latitude).ToList()[0].CameraId;
                    cameraIdCopy = camera;
                    SaveCameraDetails(cameraIdCopy, int.Parse(cameraType), isSpeedCamera, input);
                }
            }
            else
            {
                SaveCameraDetails(cameraIdCopy, int.Parse(cameraType), isSpeedCamera, input);
            }
            return 1;
        }
        /// <summary>
        /// get the address Id to be save 
        /// </summary>
        /// <param name="line1"> line1 of address</param>
        /// <param name="line2"> line2 of address </param> 
        /// <param name="line3"> line3 of address </param>
        /// <param name="city"> city of address </param>
        /// <param name="county"> county of address </param>
        /// <param name="country"> country of address </param>
        /// <param name="postcode"> postcode of address </param>
        /// <returns> address Id, or -1 if fail to found address id </returns>
        private int GetAddressId(string line1, string line2,
            string line3, string city, string county,
            string country, string postcode)
        {
            using (var context = new DVLAEntities())
            {
                var addresses = context.Addresses.Select(
                a => new AddtionalModelsOrBusinessClass.Data.Address
                {
                    AddressId = a.AddressId,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    Line3 = a.Line3,
                    City = a.City,
                    County = a.County,
                    Country = a.Country,
                    PostCode = a.PostalCode,

                }).Where(a => (a.Line1 == line1 && a.City == city && a.County == county && a.Country == country && a.PostCode == postcode));
                if (!string.IsNullOrWhiteSpace(line2))
                {
                    addresses = addresses.Where(a => a.Line2 == line2);
                }
                if (!string.IsNullOrWhiteSpace(line3))
                {
                    addresses = addresses.Where(a => a.Line3 == line3);
                }
                if (addresses.Count() == 0)
                {
                    return -1;
                }
                return addresses.ToList()[0].AddressId;
            }
        }
        /// <summary>
        /// Save Camera Detials to coressponding camera type
        /// </summary>
        /// <param name="cameraId"> camera Id of camera</param>
        /// <param name="cameraDetails"> camera details </param>
        /// <param name="isSpeedCamera"> Is Speed Camera or not </param>
        /// <param name="input"> new input or not </param>
        private void SaveCameraDetails(int cameraId, int cameraDetails, bool isSpeedCamera, bool input)
        {
            using (var context = new DVLAEntities())
            {
                if (isSpeedCamera == true && input == true)
                {
                    var cameraType = new EntityFrameWorkModel.SpeedCamera();
                    context.SpeedCameras.Add(cameraType);
                    cameraType.CameraId = cameraId;
                    cameraType.SpeedMphLimit = (byte)cameraDetails;
                }
                else if(isSpeedCamera == false && input == true)
                {
                    var cameraType = new EntityFrameWorkModel.TrafficLightCamera();
                    context.TrafficLightCameras.Add(cameraType);
                    cameraType.CameraId = cameraId;
                    cameraType.SecondsAfterRedLightThreshold = (byte)cameraDetails;
                }
                if (isSpeedCamera == true && input == false)
                {
                    var cameraType = context.SpeedCameras.Find(cameraId);
                    cameraType.SpeedMphLimit = (byte)cameraDetails;
                }
                else if (isSpeedCamera == false && input == false)
                {
                    var cameraType = context.TrafficLightCameras.Find(cameraId);
                    cameraType.SecondsAfterRedLightThreshold = (byte)cameraDetails;
                }
                context.SaveChanges();
            }
        }
    }
}
