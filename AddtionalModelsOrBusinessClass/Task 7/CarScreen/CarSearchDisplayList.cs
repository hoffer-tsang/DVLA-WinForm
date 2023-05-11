/*==============================================================================
 *
 * Car Search Display List Class
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

namespace AddtionalModelsOrBusinessClass.Task_7.CarScreen
{
    public class CarSearchDisplayList
    {
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }

        public string Make { get; set; }
        public string Owner { get; set; }

        private const int V = 0;
        private int _PageSize = 5;

        /// <summary>
        /// Perform the Search in database based on search field to update car list
        /// </summary>
        /// <param name="registrationNumberSearch"> registration number to search </param>
        /// <param name="modelSearch"> model to search </param>
        /// <param name="makeSearch"> make to search </param>
        /// <param name="firstNameSearch"> firstname to search </param>
        /// <param name="lastNameSearch"> lastname to search </param>
        /// <param name="pageNumber"> page number to be display </param>
        /// <param name="columnIndex"> column Index to be sorted </param>
        /// <returns> A list of car base on search field to display </returns>
        public List<CarSearchDisplayList> SearchCarList(
        string registrationNumberSearch,
        object modelSearch,
        object makeSearch,
        string firstNameSearch,
        string lastNameSearch,
        int pageNumber,
        int columnIndex)
        {
            using (var context = new DVLAEntities())
            {
                IQueryable<CarSearch> car = context.Cars.Select(
                c => new CarSearch
                {
                    RegistrationNumber = c.RegistrationNumber,
                    Model = c.Model.Name,
                    Make = c.Model.Make.Name,
                    OwnerFirstName = c.Owner.FirstName,
                    OwnerLastName = c.Owner.LastName,
                });

                if (!string.IsNullOrWhiteSpace(registrationNumberSearch))
                {
                    car = car.Where(c => c.RegistrationNumber == registrationNumberSearch);
                }
                if (modelSearch != null)
                {
                    car = car.Where(c => c.Model == modelSearch.ToString());
                }
                if (makeSearch != null)
                {
                    car = car.Where(c => c.Make == makeSearch.ToString());
                }
                if (!string.IsNullOrWhiteSpace(firstNameSearch))
                {
                    car = car.Where(c => c.OwnerFirstName == firstNameSearch);
                }
                if (!string.IsNullOrWhiteSpace(lastNameSearch))
                {
                    car =car.Where(c => c.OwnerLastName == lastNameSearch);
                }
                switch (columnIndex)
                {
                    case V:
                        car = car.OrderBy(c => c.RegistrationNumber).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 1:
                        car = car.OrderBy(c => c.Model).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 2:
                        car = car.OrderBy(c => c.Make).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 3:
                        car = car.OrderBy(c => c.OwnerFirstName).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                }
                List<CarSearchDisplayList> carDisplayList = new List<CarSearchDisplayList>();
                foreach (var ca in car)
                {
                    var carDisplay = new CarSearchDisplayList
                    {
                        RegistrationNumber = ca.RegistrationNumber,
                        Model = ca.Model,
                        Make = ca.Make,
                        Owner = ca.OwnerFirstName + " " + ca.OwnerLastName,
                    };
                    carDisplayList.Add(carDisplay);
                }
                return carDisplayList;
            }
        }
        /// <summary>
        /// Get total page number required for car display list 
        /// </summary>
        /// <param name="registrationNumberSearch"> registration number to search </param>
        /// <param name="modelSearch"> model to search </param>
        /// <param name="makeSearch"> make to search </param>
        /// <param name="firstNameSearch"> firstname to search </param>
        /// <param name="lastNameSearch"> lastname to search </param>>
        /// <returns> list of page number </returns>
        public List<int> TotalCarPageNumber(string registrationNumberSearch,
        object modelSearch,
        object makeSearch,
        string firstNameSearch,
        string lastNameSearch)
        {
            int totalPageNumber;
            List<int> pageNumberList = new List<int>();
            using (var context = new DVLAEntities())
            {
                var car = context.Cars.Select(
                c => new CarSearch
                {
                    RegistrationNumber = c.RegistrationNumber,
                    Model = c.Model.Name,
                    Make = c.Model.Make.Name,
                    OwnerFirstName = c.Owner.FirstName,
                    OwnerLastName = c.Owner.LastName,
                });

                if (!string.IsNullOrWhiteSpace(registrationNumberSearch))
                {
                    car = car.Where(c => c.RegistrationNumber == registrationNumberSearch);
                }
                if (modelSearch != null)
                {
                    car = car.Where(c => c.Model == modelSearch.ToString());
                }
                if (makeSearch != null)
                {
                    car = car.Where(c => c.Make == makeSearch.ToString());
                }
                if (!string.IsNullOrWhiteSpace(firstNameSearch))
                {
                    car = car.Where(c => c.OwnerFirstName == firstNameSearch);
                }
                if (!string.IsNullOrWhiteSpace(lastNameSearch))
                {
                    car = car.Where(c => c.OwnerLastName == lastNameSearch);
                }
                if ((car.ToList().Count() / _PageSize) == 0)
                {
                    pageNumberList.Add(1);
                    return pageNumberList;
                }
                else
                {
                    int leftover = car.ToList().Count() % _PageSize;
                    totalPageNumber = car.ToList().Count() / _PageSize;
                    if( leftover > 0)
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
