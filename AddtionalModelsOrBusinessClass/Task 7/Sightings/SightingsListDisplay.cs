/*==============================================================================
 *
 * Sightings Display List Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.Sightings
{
    public class SightingsListDisplay
    {
        public DateTime SightingTime { get; set; }
        public byte? SecondsAfterRedLight { get; set; }
        public byte? Speed { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? DatePaid { get; set; }
        private int _PageSize = 5;
        /// <summary>
        /// get the list of sighting for corresponding carid or camera id
        /// </summary>
        /// <param name="id"> car id or camera id </param>
        /// <param name="pageNumber"> page number to display </param>
        /// <param name="isCarScreen"> true for car id, false for camera id </param>
        /// <param name="columnIndex"> column to be sorted </param>
        /// <returns> a list of sightings to be display </returns>
        public List<SightingsListDisplay> GetSightingsList(int id, int pageNumber, bool isCarScreen, int columnIndex)
        {
            if (isCarScreen == true)
            {
                using (var context = new DVLAEntities())
                {
                    var sightings = context.Cars.Select(
                    c => new { c.Sightings, c.CarId }).Where(c => c.CarId == id).SingleOrDefault();
                    List<Sighting> sightingList = new List<Sighting>();
                    switch (columnIndex)
                    {
                        case 0:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SightingTime).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 1:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SecondsAfterRedLight.HasValue).ThenBy(s => s.SecondsAfterRedLight).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 2:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SpeedMph.HasValue).ThenBy(s => s.SpeedMph).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 3:
                            IEnumerable<Sighting> sightingListQuery = sightings.Sightings.Where(s => s.Fine != null).OrderBy(s => s.Fine.DateIssued);
                            sightingListQuery = sightingListQuery.Concat(sightings.Sightings.Where(s => s.Fine == null).OrderBy(s => s.SightingTime));
                            sightingList = sightingListQuery.Skip((pageNumber - 1) * _PageSize).Take(_PageSize).ToList();
                            break;
                        case 4:
                            IEnumerable<Sighting> sightingListQueryPaid = sightings.Sightings.Where(s => s.Fine != null).OrderBy(s => s.Fine.DatePaid);
                            sightingListQueryPaid = sightingListQueryPaid.Concat(sightings.Sightings.Where(s => s.Fine == null).OrderBy(s => s.SightingTime));
                            sightingList = sightingListQueryPaid.Skip((pageNumber - 1) * _PageSize).Take(_PageSize).ToList();
                            break;
                    }
                    var sightingDisplayList = new List<SightingsListDisplay>();
                    foreach (var sighting in sightingList)
                    {
                        if(sighting.Fine != null)
                        {
                            var sightingDisplay = new SightingsListDisplay
                            {
                                SightingTime = sighting.SightingTime,
                                SecondsAfterRedLight = sighting.SecondsAfterRedLight,
                                Speed = sighting.SpeedMph,
                                DateIssued = sighting.Fine.DateIssued,
                                DatePaid = sighting.Fine.DatePaid
                            };
                            sightingDisplayList.Add(sightingDisplay);
                        }
                        else
                        {
                            var sightingDisplay = new SightingsListDisplay
                            {
                                SightingTime = sighting.SightingTime,
                                SecondsAfterRedLight = sighting.SecondsAfterRedLight,
                                Speed = sighting.SpeedMph,
                            };
                            sightingDisplayList.Add(sightingDisplay);
                        }
                    }
                    return sightingDisplayList;
                }
            }
            else
            {
                using (var context = new DVLAEntities())
                {
                    var sightings = context.Cameras.Select(
                    c => new { c.Sightings, c.CameraId }).Where(c => c.CameraId == id).SingleOrDefault();
                    List<Sighting> sightingList = new List<Sighting>();
                    switch (columnIndex)
                    {
                        case 0:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SightingTime).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 1:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SecondsAfterRedLight).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 2:
                            sightingList = (sightings.Sightings.OrderBy(s => s.SpeedMph).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                            break;
                        case 3:
                            IEnumerable<Sighting> sightingListQuery = sightings.Sightings.Where(s => s.Fine != null).OrderBy(s => s.Fine.DateIssued);
                            sightingListQuery = sightingListQuery.Concat(sightings.Sightings.Where(s => s.Fine == null).OrderBy(s => s.SightingTime));
                            sightingList = sightingListQuery.Skip((pageNumber - 1) * _PageSize).Take(_PageSize).ToList();
                            break;
                        case 4:
                            IEnumerable<Sighting> sightingListQueryPaid = sightings.Sightings.Where(s => s.Fine != null).OrderBy(s => s.Fine.DatePaid);
                            sightingListQueryPaid = sightingListQueryPaid.Concat(sightings.Sightings.Where(s => s.Fine == null).OrderBy(s => s.SightingTime));
                            sightingList = sightingListQueryPaid.Skip((pageNumber - 1) * _PageSize).Take(_PageSize).ToList();
                            break;
                    }
                    var sightingDisplayList = new List<SightingsListDisplay>();
                    foreach (var sighting in sightingList)
                    {
                        if (sighting.Fine != null)
                        {
                            var sightingDisplay = new SightingsListDisplay
                            {
                                SightingTime = sighting.SightingTime,
                                SecondsAfterRedLight = sighting.SecondsAfterRedLight,
                                Speed = sighting.SpeedMph,
                                DateIssued = sighting.Fine.DateIssued,
                                DatePaid = sighting.Fine.DatePaid
                            };
                            sightingDisplayList.Add(sightingDisplay);
                        }
                        else
                        {
                            var sightingDisplay = new SightingsListDisplay
                            {
                                SightingTime = sighting.SightingTime,
                                SecondsAfterRedLight = sighting.SecondsAfterRedLight,
                                Speed = sighting.SpeedMph,
                            };
                            sightingDisplayList.Add(sightingDisplay);
                        }
                    }
                    return sightingDisplayList;
                }
            }
        }
        /// <summary>
        /// the total number of page for sightings
        /// </summary>
        /// <param name="id"> car id or camera id </param>
        /// <param name="isCarScreen"> true for car id, false for camera id </param>
        /// <returns> list of page number </returns>
        public List<int> TotalSightingsPageNumber(int id, bool isCarScreen)
        {
            if(isCarScreen == true)
            {
                int totalPageNumber;
                using (var context = new DVLAEntities())
                {
                    var sightings = context.Cars.Select(
                        c => new { c.Sightings, c.CarId }).Where(c => c.CarId == id).SingleOrDefault();
                    totalPageNumber = sightings.Sightings.Count();
                }
                List<int> pageNumberList = new List<int>();
                if (totalPageNumber == 0)
                {
                    pageNumberList.Add(1);
                    return pageNumberList;
                }
                else
                {
                    int leftover = totalPageNumber % _PageSize;
                    totalPageNumber = (totalPageNumber / _PageSize);
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
            else
            {
                int totalPageNumber;
                using (var context = new DVLAEntities())
                {
                    var sightings = context.Cameras.Select(
                        c => new { c.Sightings, c.CameraId }).Where(c => c.CameraId == id).SingleOrDefault();
                    totalPageNumber = sightings.Sightings.Count();
                }
                List<int> pageNumberList = new List<int>();
                if (totalPageNumber == 0)
                {
                    pageNumberList.Add(1);
                    return pageNumberList;
                }
                else
                {
                    int leftover = totalPageNumber % _PageSize;
                    totalPageNumber = (totalPageNumber / _PageSize);
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
