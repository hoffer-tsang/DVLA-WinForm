/*==============================================================================
 *
 * Camera Search Display List Class
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
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.CameraScreen
{
    public class CameraSearchDisplayList
    {
        public string CameraType { get; set; }

        public string RoadName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        private int _PageSize = 5;
        /// <summary>
        /// Get the camera list based on search field (all null when page refresh)
        /// </summary>
        /// <param name="longitudeFrom"> longtitude from search field </param>
        /// <param name="longitudeTo"> longtitude to search field </param>
        /// <param name="latitudeFrom"> latitude from search field </param>
        /// <param name="latitudeTo"> latitude to search field </param>
        /// <param name="pageNumber"> page number to be display </param>
        /// <param name="columnIndex"> Index of column to be sorted </param>
        /// <returns> a list of filtered camera search display </returns>
        public List<CameraSearchDisplayList> SearchCameraList(
        string longitudeFrom, string longitudeTo,
        string latitudeFrom, string latitudeTo, int pageNumber, int columnIndex)
        {
            using (var context = new DVLAEntities())
            {
                IQueryable<EntityFrameWorkModel.Camera> cameraDisplayList = context.Cameras;
                if (!string.IsNullOrWhiteSpace(longitudeFrom))
                {
                    decimal longtitudeFromValue = decimal.Parse(longitudeFrom);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Longitude >= longtitudeFromValue);
                }
                if (!string.IsNullOrWhiteSpace(longitudeTo))
                {
                    decimal longtitudeToValue = decimal.Parse(longitudeTo);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Longitude <= longtitudeToValue);
                }
                if (!string.IsNullOrWhiteSpace(latitudeFrom))
                {
                    decimal latitudeFromValue = decimal.Parse(latitudeFrom);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Latitude >= latitudeFromValue);
                }
                if (!string.IsNullOrWhiteSpace(latitudeTo))
                {
                    decimal latitudeToValue = decimal.Parse(latitudeTo);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Latitude <= latitudeToValue);
                }
                switch (columnIndex)
                {
                    case 0:
                        var speedCameraDisplayList = cameraDisplayList.Where(c => c.SpeedCamera != null);
                        cameraDisplayList = speedCameraDisplayList.Concat(cameraDisplayList.Where(c => c.SpeedCamera == null));
                        cameraDisplayList = cameraDisplayList.OrderBy(c => c.TrafficLightCamera.CameraId).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 1:
                        cameraDisplayList = cameraDisplayList.OrderBy(c => c.RoadName).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 2:
                        cameraDisplayList = cameraDisplayList.OrderBy(c => c.Longitude).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 3:
                        cameraDisplayList = cameraDisplayList.OrderBy(c => c.Latitude).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                }
                List<CameraSearchDisplayList> cameraList = new List<CameraSearchDisplayList>();
                foreach (var camera in cameraDisplayList)
                {
                    CameraSearchDisplayList cameraDetails = new CameraSearchDisplayList()
                    {
                        RoadName = camera.RoadName,
                        Longitude = camera.Longitude,
                        Latitude = camera.Latitude,
                    };
                    var cameraType = context.SpeedCameras.Find(camera.CameraId);
                    if (cameraType != null)
                    {
                        cameraDetails.CameraType = "Speed Camera";
                    }
                    else
                    {
                        cameraDetails.CameraType = "Traffic Light Camera";
                    }
                    cameraList.Add(cameraDetails);

                }
                return cameraList.ToList();
            }
        }
        /// <summary>
        /// Find the total number of page number required 
        /// to be show in page selection drop box
        /// </summary>
        /// <param name="longitudeFrom"> longtitude from search field </param>
        /// <param name="longitudeTo"> longtitude to search field </param>
        /// <param name="latitudeFrom"> latitude from search field </param>
        /// <param name="latitudeTo"> latitude to search field </param>
        /// <returns> list of page number </returns>
        public List<int> TotalCameraPageNumber(string longitudeFrom, string longitudeTo,
        string latitudeFrom, string latitudeTo)
        {
            using (var context = new DVLAEntities())
            {
                int totalPageNumber;
                List<int> pageNumberList = new List<int>();
                IQueryable<EntityFrameWorkModel.Camera> cameraDisplayList = context.Cameras;
                if (!string.IsNullOrWhiteSpace(longitudeFrom))
                {
                    decimal longtitudeFromValue = decimal.Parse(longitudeFrom);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Longitude >= longtitudeFromValue);
                }
                if (!string.IsNullOrWhiteSpace(longitudeTo))
                {
                    decimal longtitudeToValue = decimal.Parse(longitudeTo);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Longitude <= longtitudeToValue);
                }
                if (!string.IsNullOrWhiteSpace(latitudeFrom))
                {
                    decimal latitudeFromValue = decimal.Parse(latitudeFrom);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Latitude >= latitudeFromValue);
                }
                if (!string.IsNullOrWhiteSpace(latitudeTo))
                {
                    decimal latitudeToValue = decimal.Parse(latitudeTo);
                    cameraDisplayList = cameraDisplayList.Where(c => c.Latitude <= latitudeToValue);
                }
                if ((cameraDisplayList.ToList().Count() / _PageSize) == 0)
                {
                    pageNumberList.Add(1);
                    return pageNumberList;
                }
                else
                {
                    int leftover = cameraDisplayList.ToList().Count() % _PageSize;
                    totalPageNumber = (cameraDisplayList.ToList().Count() / _PageSize);
                    if (leftover > 0)
                    {
                        totalPageNumber += 1;
                    }
                    for (int i = 0; i < totalPageNumber; i++)
                    {
                        pageNumberList.Add(i + 1);
                    }
                    return pageNumberList;
                }
            }
        }
    }
}
