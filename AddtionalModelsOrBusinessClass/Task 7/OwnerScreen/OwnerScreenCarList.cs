/*==============================================================================
 *
 * Owner Screen Car Search Display List Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.OwnerScreen
{
    public class OwnerScreenCarList
    {
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }

        public string Make { get; set; }
        public string Colour { get; set; }
        private int _PageSize = 5;
        /// <summary>
        /// Get Car List for specific owner 
        /// </summary>
        /// <param name="ownerId"> ownerId of the owner </param>
        /// <param name="pageNumber"> page number to display </param>
        /// <param name="columnIndex"> column Index to be sorted </param>
        /// <returns> list of car to display in that page </returns>
        public List<OwnerScreenCarList> GetCarList(int ownerId, int pageNumber, int columnIndex)
        {
            using (var context = new DVLAEntities())
            {
                var cars = context.Owners.Select(
                o => new { o.Cars, o.OwnerId }).Where(o => o.OwnerId == ownerId).SingleOrDefault();
                List<Car> carList = new List<Car>();
                switch (columnIndex)
                {
                    case 0:
                        carList = (cars.Cars.OrderBy(c => c.RegistrationNumber).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                        break;
                    case 1:
                        carList = (cars.Cars.OrderBy(c => c.Model.Name).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                        break;
                    case 2:
                        carList = (cars.Cars.OrderBy(c => c.Model.Make.Name).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                        break;
                    case 3:
                        carList = (cars.Cars.OrderBy(c => c.Colour.Name).Skip((pageNumber - 1) * _PageSize).Take(_PageSize)).ToList();
                        break;
                }
                var carDisplayList = new List<OwnerScreenCarList>();
                foreach (var car in carList)
                {
                    var carDisplay = new OwnerScreenCarList
                    {
                        RegistrationNumber = car.RegistrationNumber,
                        Model = car.Model.Name,
                        Make = car.Model.Make.Name,
                        Colour = car.Colour.Name,
                    };
                    carDisplayList.Add(carDisplay);
                }
                return carDisplayList;
            }
        }/// <summary>
        /// find the total page number to display
        /// </summary>
        /// <param name="ownerId"> owernid of the owner</param>
        /// <returns> a list of page number </returns>
        public List<int> TotalCarPageNumber(int ownerId)
        {
            int totalPageNumber;
            using (var context = new DVLAEntities())
            {
                var cars = context.Owners.Select(
                o => new { o.Cars, o.OwnerId }).Where(o => o.OwnerId == ownerId).SingleOrDefault();
                totalPageNumber = cars.Cars.Count();
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
